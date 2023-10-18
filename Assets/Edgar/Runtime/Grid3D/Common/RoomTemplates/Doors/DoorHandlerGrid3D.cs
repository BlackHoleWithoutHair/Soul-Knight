using Edgar.Unity.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Component that marks a door of a room template.
    /// </summary>
    /// <remarks>
    /// Why this name instead of just "DoorGrid3D"?
    /// The shorter name just does not feel right to me. This handler component is referenced/store in multiple
    /// place and it would be confusing if it was named just "Door" instead of "DoorHandler".
    /// </remarks>
    [AddComponentMenu("Edgar/Grid3D/Door Handler (Grid3D)")]
    public class DoorHandlerGrid3D : MonoBehaviour
    {
        /// <summary>
        /// Width of the door (in tiles).
        /// </summary>
        public int Width = 1;

        /// <summary>
        /// Height of the door (in tiles).
        /// </summary>
        public int Height = 1;

        /// <summary>
        /// How many times should the door be repeated to also cover neighboring tiles.
        /// This is similar to hybrid doors in the 2D version of the asset.
        /// </summary>
        public int Repeat = 0;

        /// <summary>
        /// Door socket which control which doors can be connected together.
        /// </summary>
        public DoorSocketBase Socket;

        /// <summary>
        /// Direction of the door that is used if a level graph is configured to be directed.
        /// The direction specifies whether the door is an entrance, exit or both.
        /// </summary>
        public DoorDirection Direction = DoorDirection.Undirected;

        /// <summary>
        /// Generator settings.
        /// </summary>
        public GeneratorSettingsGrid3D GeneratorSettings;

        /// <summary>
        /// List of blockers that are used in place of door positions that are not used.
        /// If more than one is provided, a random one is picked.
        /// </summary>
        public List<GameObject> Blockers;

        /// <summary>
        /// List of connectors that are used in place of door positions that are used.
        /// If more than one is provided, a random one is picked.
        /// </summary>
        public List<GameObject> Connectors;

        /// <summary>
        /// Internal direction vector of the door.
        /// The direction is clockwise on the outline of the room template.
        /// </summary>
        [HideInInspector]
        public Vector3Int DirectionVector = new Vector3Int(1, 0, 0);

        /// <summary>
        /// Gets the door line that this door handler represents.
        /// </summary>
        /// <param name="checkIfValid">Throws errors if true and the door is not valid.</param>
        /// <returns></returns>
        public DoorLineGrid3D GetDoorLine(bool checkIfValid = true)
        {
            Vector3Int from;

            // Will throw exceptions if there is any problem with the door and checkIfValid is true
            if (checkIfValid)
            {
                from = GeneratorSettings.LocalToCell(transform.localPosition);

                if (!IsDirectionValid())
                {
                    throw new DoorRotationOutOfSyncException();
                }
            }
            // Will not throw exception if used for gizmo drawing
            else
            {
                var fromInterpolated = GeneratorSettings.LocalToCellInterpolated(transform.localPosition);
                from = GridUtilsGrid3D.SnapInterpolatedToCell(fromInterpolated);

                // If this method is only used to draw gizmos, prevent errors caused by an incorrect direction
                if (!DoorUtilsGrid3D.AllowedDirections.Contains(DirectionVector))
                {
                    DirectionVector = DoorUtilsGrid3D.AllowedDirections[0];
                }
            }

            // Compute From and To positions based on the current position of the game object and its rotation
            if (DirectionVector.x < 0)
            {
                var offset = new Vector3Int(-1, 0, -1);
                from += offset;
            }
            else if (DirectionVector.z > 0)
            {
                var offset = new Vector3Int(-1, 0, 0);
                from += offset;
            }
            else if (DirectionVector.z < 0)
            {
                var offset = new Vector3Int(0, 0, -1);
                from += offset;
            }

            var to = from + DirectionVector * Repeat;

            return new DoorLineGrid3D(from, to, Width, Height, this, DirectionVector, Socket, Direction);
        }

        public bool IsDirectionValid()
        {
            // Make sure that the DirectionVector is valid
            var directionIndex = DoorUtilsGrid3D.AllowedDirections.IndexOf(DirectionVector);

            if (directionIndex == -1)
            {
                return false;
            }

            // If the direction vector is valid, make sure that the actual rotation is in sync with it
            var expectedRotation = DoorUtilsGrid3D.AllowedRotations[directionIndex].normalized;
            var actualRotation = transform.localRotation.normalized;

            if (Math.Abs(Quaternion.Angle(expectedRotation, actualRotation)) > 0.001)
            {
                return false;
            }

            return true;
        }

        public void SyncRotation()
        {
            var directionIndex = DoorUtilsGrid3D.AllowedDirections.IndexOf(DirectionVector);

            if (directionIndex == -1)
            {
                DirectionVector = DoorUtilsGrid3D.AllowedDirections[0];
                directionIndex = 0;
            }

            transform.localRotation = DoorUtilsGrid3D.AllowedRotations[directionIndex];
        }

        public void Rotate90(bool clockwise)
        {
            DirectionVector = DoorUtilsGrid3D.RotateDirection(DirectionVector, clockwise ? 1 : -1);
            var directionIndex = DoorUtilsGrid3D.AllowedDirections.IndexOf(DirectionVector);
            transform.localRotation = DoorUtilsGrid3D.AllowedRotations[directionIndex];
        }

        private void OnValidate()
        {
            Width = Math.Max(Width, 1);
            Height = Math.Max(Height, 1);
            Repeat = Math.Max(Repeat, 0);
        }

        public void DisableConnectorsAndBlockers()
        {
            foreach (var go in Connectors.Concat(Blockers))
            {
                if (go == null || IsPrefab(go))
                {
                    continue;
                }

                go.SetActive(false);
            }
        }

        public void ProcessConnectorsAndBlockers(ConnectorsAndBlockersContextGrid3D context)
        {
            DisableConnectorsAndBlockers();

            var list = context.DoorLine.GetTiles();
            for (var i = 0; i < list.Count; i++)
            {
                var tile = list[i];

                if (tile.IsUsed && tile.IndexInsideDoor != 0)
                {
                    continue;
                }

                if (tile.IsUsed && !context.AddConnectors)
                {
                    var door = tile.Door;
                    var otherRoom = door.ConnectedRoomInstance;
                    var bothNonCorridors = !context.RoomInstance.IsCorridor && !otherRoom.IsCorridor;

                    if (bothNonCorridors && context.ConnectorsMode == ConnectorsModeGrid3D.PreferCorridors)
                    {
                        // The door leads right to another room without a corridor between them
                        // With the PreferCorridors mode, a connector should be added inside that door
                    }
                    else
                    {
                        continue;
                    }
                }

                if (!tile.IsUsed && !context.AddBlockers)
                {
                    continue;
                }

                var pickOne = tile.IsUsed ? Connectors : Blockers;
                var blocker = pickOne.Count > 0 ? pickOne[context.Random.Next(pickOne.Count)] : null;

                if (blocker == null)
                {
                    continue;
                }

                var instance = Instantiate(blocker);
                instance.transform.parent = blocker.transform.parent;
                //instance.transform.parent = level.RootGameObject.transform;
                //instance.transform.parent = roomInstance.RoomTemplateInstance.transform;
                instance.SetActive(true);
                instance.transform.localRotation = Quaternion.identity;
                instance.transform.localPosition = tile.Position; // - new Vector3Int(3, 0, 3);

                // TODO(Grid3D)
                var directionVector = new Vector3(1, 0, 0);
                var roomTemplate = context.RoomInstance.RoomTemplateInstance.GetComponent<RoomTemplateSettingsGrid3D>();
                if (roomTemplate.GeneratorSettings != null)
                {
                    directionVector = roomTemplate.GeneratorSettings.CellToLocal(directionVector);
                }

                instance.transform.localPosition = blocker.transform.localPosition + directionVector * i; // - new Vector3Int(3, 0, 3);
            }
        }

        private bool IsPrefab(GameObject gameObject)
        {
            return gameObject.scene.name == null;
        }
    }
}
using System;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Builtin post-processing logic for the Grid3D generator.
    /// </summary>
    public static class PostProcessingUtilsGrid3D
    {
        /// <summary>
        /// Processes connector and blockers of a given level.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="addConnectors"></param>
        /// <param name="addBlockers"></param>
        public static void ProcessConnectorsAndBlockers(DungeonGeneratorLevelGrid3D level, ConnectorsModeGrid3D addConnectors, bool addBlockers)
        {
            foreach (var roomInstance in level.RoomInstances)
            {
                ProcessConnectorsAndBlockers(level, roomInstance, addConnectors, addBlockers);
            }
        }

        /// <summary>
        /// Process connectors and blockers of a given room instance.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="roomInstance"></param>
        /// <param name="addConnectors"></param>
        /// <param name="addBlockers"></param>
        public static void ProcessConnectorsAndBlockers(DungeonGeneratorLevelGrid3D level, RoomInstanceGrid3D roomInstance, ConnectorsModeGrid3D addConnectors, bool addBlockers)
        {
            var currentRoomAddConnectors =
                addConnectors == ConnectorsModeGrid3D.RoomsAndCorridors
                || (roomInstance.IsCorridor && (addConnectors == ConnectorsModeGrid3D.CorridorsOnly || addConnectors == ConnectorsModeGrid3D.PreferCorridors))
                || (!roomInstance.IsCorridor && addConnectors == ConnectorsModeGrid3D.RoomsOnly);

            foreach (var doorLine in roomInstance.DoorLines)
            {
                var doorHandler = doorLine.DoorHandler;
                var context = new ConnectorsAndBlockersContextGrid3D(
                    doorLine,
                    roomInstance,
                    level,
                    currentRoomAddConnectors,
                    addBlockers,
                    addConnectors
                );

                doorHandler.ProcessConnectorsAndBlockers(context);
            }
        }

        /// <summary>
        /// Gets the center of a given level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Vector3 GetLevelCenter(DungeonGeneratorLevelGrid3D level)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach (var roomInstance in level.RoomInstances)
            {
                var outline = roomInstance.OutlinePolygon;
                var boundingBox = outline.GetGridPolygon().BoundingRectangle;

                minX = Math.Min(minX, boundingBox.A.X);
                maxX = Math.Max(maxX, boundingBox.B.X);
                minY = Math.Min(minY, boundingBox.A.Y);
                maxY = Math.Max(maxY, boundingBox.B.Y);
            }

            var offset = new Vector3((maxX + minX) / 2f, 0, (maxY + minY) / 2f);
            offset = level.GeneratorSettings.CellToLocal(offset);

            return offset;
        }

        /// <summary>
        /// Moves the level so that it is centered.
        /// </summary>
        /// <param name="level"></param>
        public static void CenterLevel(DungeonGeneratorLevelGrid3D level)
        {
            var center = GetLevelCenter(level);

            foreach (Transform transform in level.RootGameObject.transform)
            {
                transform.position -= center;
            }
        }
    }
}
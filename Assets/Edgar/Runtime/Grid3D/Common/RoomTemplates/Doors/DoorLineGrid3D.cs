using Edgar.Geometry;
using System;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// A simple data class that holds information about a single door line.
    /// </summary>
    /// <remarks>
    /// The door line is represented as tiles rather than points.
    /// 
    /// Why not use DoorHandlerGrid3D directly?
    /// DoorHandlerGrid3D is a MonoBehaviour which means that it is not easy to store it and create new instance.
    /// </remarks>
    [Serializable]
    public class DoorLineGrid3D : IDoorLine
    {
        /// <summary>
        /// The first tile of the door.
        /// </summary>
        public Vector3Int From => from;

        [SerializeField]
        private Vector3Int from;

        /// <summary>
        /// The last tile of the door.
        /// </summary>
        /// <remarks>
        /// It can be equal to the From field if the width is equal to 1.
        /// </remarks>
        public Vector3Int To => to;

        [SerializeField]
        private Vector3Int to;

        /// <summary>
        /// Width of the door (number of tiles).
        /// </summary>
        public int Width => width;

        [SerializeField]
        private int width;

        /// <summary>
        /// Width of the door (number of tiles).
        /// </summary>
        public int Height => height;

        [SerializeField]
        private int height;

        public DoorHandlerGrid3D DoorHandler => doorHandler;

        [SerializeField]
        private DoorHandlerGrid3D doorHandler;

        /// <summary>
        /// Direction vector of the door.
        /// If you go clockwise on the outline of the room template and assign a direction to each side of polygon,
        /// the direction of the door is equal to the direction of the polygon side to which it belongs.
        /// </summary>
        /// <remarks>
        /// It holds that From + Direction * (Width - 1) == To.
        /// The Up direction is the same as Vector3Int.Up.
        /// </remarks>
        public Vector3Int DirectionVector => directionVector;

        [SerializeField]
        private Vector3Int directionVector;

        /// <summary>
        /// Socket of the door, can be null.
        /// </summary>
        public DoorSocketBase Socket => socket;

        [SerializeField]
        private DoorSocketBase socket;

        /// <summary>
        /// Direction of the door when it comes to entrances/exits.
        /// Each door is undirected by default, meaning it can serve both as an entrance and an exit.
        /// </summary>
        public DoorDirection Direction => direction;

        [SerializeField]
        private DoorDirection direction;

        // TODO(Grid3D): Width vs Length
        int IDoorLine.Length => Width;

        public DoorLineGrid3D(Vector3Int from, Vector3Int to, int width, int height, DoorHandlerGrid3D doorHandler, Vector3Int directionVector, DoorSocketBase socket, DoorDirection direction)
        {
            this.from = from;
            this.to = to;
            this.width = width;
            this.doorHandler = doorHandler;
            this.directionVector = directionVector;
            this.socket = socket;
            this.direction = direction;
            this.height = height;
        }

        /// <summary>
        /// Computes the internal representation of the door line in point coordinates.
        /// </summary>
        /// <returns></returns>
        internal GraphBasedGenerator.Grid2D.DoorLineGrid2D GetInternalPointRepresentation()
        {
            Vector3Int transformedFrom;
            Vector3Int transformedTo;

            if (directionVector.z > 0)
            {
                transformedFrom = @from;
                transformedTo = to;
            }
            else if (directionVector.z < 0)
            {
                var offset = new Vector3Int(1, 0, 1);
                transformedFrom = from + offset;
                transformedTo = to + offset;
            }
            else if (directionVector.x > 0)
            {
                var offset = new Vector3Int(0, 0, 1);
                transformedFrom = @from + offset;
                transformedTo = to + offset;
            }
            else if (directionVector.x < 0)
            {
                var offset = new Vector3Int(1, 0, 0);
                transformedFrom = @from + offset;
                transformedTo = to + offset;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            var doorLine = new OrthogonalLineGrid2D(
                new EdgarVector2Int(transformedFrom.x, transformedFrom.z),
                new EdgarVector2Int(transformedTo.x, transformedTo.z),
                DoorUtilsGrid3D.VectorToDirection(new EdgarVector2Int(directionVector.x, directionVector.z))
            );

            return new GraphBasedGenerator.Grid2D.DoorLineGrid2D(
                doorLine,
                Width,
                Socket,
                DoorsGrid2D.GetDoorType(Direction)
            );
        }

        /// <summary>
        /// Computes the bounds of the door in the 3D space.
        /// Mostly used for gizmo purposes.
        /// </summary>
        /// <remarks>
        /// The bounds are represented as points bounding the door (which is described in tiles).
        /// </remarks>
        /// <returns></returns>
        public BoundsInt Get3DBounds(bool includeRepeat = true)
        {
            Vector3Int transformedFrom;
            Vector3Int transformedTo;

            var to = includeRepeat ? this.to : this.from;

            if (directionVector.z > 0)
            {
                transformedFrom = @from;
                transformedTo = to + directionVector;
            }
            else if (directionVector.z < 0)
            {
                var offset = new Vector3Int(1, 0, 0);
                transformedFrom = @from + offset - directionVector;
                transformedTo = to + offset;
            }
            else if (directionVector.x > 0)
            {
                var offset = new Vector3Int(0, 0, 1);
                transformedFrom = @from + offset;
                transformedTo = to + offset + directionVector;
            }
            else if (directionVector.x < 0)
            {
                transformedFrom = @from - directionVector;
                transformedTo = to;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            transformedTo -= GetFacingDirection();
            transformedTo += Vector3Int.up * Height;
            transformedTo += DirectionVector * (Width - 1);

            return CreateBounds(transformedFrom, transformedTo);
        }

        private BoundsInt CreateBounds(Vector3Int from, Vector3Int to)
        {
            var min = Vector3Int.Min(from, to);
            var max = Vector3Int.Max(from, to);
            var size = max - min;

            return new BoundsInt(min.x, min.y, min.z, size.x, size.y, size.z);
        }

        /// <summary>
        /// Gets the facing direction of the door.
        /// "Facing" means the direction that points outside the current room.
        /// </summary>
        /// <returns></returns>
        public Vector3Int GetFacingDirection()
        {
            return DoorUtilsGrid3D.RotateDirection(directionVector, -1);
        }
    }
}
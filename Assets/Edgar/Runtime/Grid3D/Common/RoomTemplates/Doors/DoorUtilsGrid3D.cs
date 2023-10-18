using Edgar.Geometry;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Collection of utils related to doors.
    /// </summary>
    public static class DoorUtilsGrid3D
    {
        /// <summary>
        /// Which door directions are the allowed ones.
        /// </summary>
        public static readonly IReadOnlyList<Vector3Int> AllowedDirections = new List<Vector3Int>()
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 0, -1),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1),
        };

        /// <summary>
        /// Which door object rotations are allowed.
        /// </summary>
        public static readonly IReadOnlyList<Quaternion> AllowedRotations = new List<Quaternion>()
        {
            Quaternion.Euler(new Vector3(0, 0, 0)),
            Quaternion.Euler(new Vector3(0, 90, 0)),
            Quaternion.Euler(new Vector3(0, 180, 0)),
            Quaternion.Euler(new Vector3(0, 270, 0)),
        };

        /// <summary>
        /// Rotates a given door direction by a given multiple of 90 degrees.
        /// </summary>
        /// <param name="direction">The original direction.</param>
        /// <param name="amount">How many 90 degrees rotations should be applied.</param>
        /// <returns></returns>
        public static Vector3Int RotateDirection(Vector3Int direction, int amount)
        {
            var currentDirectionIndex = AllowedDirections.IndexOf(direction);

            if (currentDirectionIndex == -1)
            {
                throw new ArgumentException("Invalid door direction provided.");
            }

            var nextIndex = currentDirectionIndex + amount;

            if (nextIndex < 0)
            {
                nextIndex += AllowedDirections.Count;
            }

            nextIndex %= AllowedDirections.Count;

            return AllowedDirections[nextIndex];
        }

        /// <summary>
        /// Converts DotNet vector to DotNet line direction.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        internal static OrthogonalLineGrid2D.Direction VectorToDirection(EdgarVector2Int vector)
        {
            if (vector == new EdgarVector2Int(1, 0))
            {
                return OrthogonalLineGrid2D.Direction.Right;
            }
            else if (vector == new EdgarVector2Int(-1, 0))
            {
                return OrthogonalLineGrid2D.Direction.Left;
            }
            else if (vector == new EdgarVector2Int(0, 1))
            {
                return OrthogonalLineGrid2D.Direction.Top;
            }
            else if (vector == new EdgarVector2Int(0, -1))
            {
                return OrthogonalLineGrid2D.Direction.Bottom;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
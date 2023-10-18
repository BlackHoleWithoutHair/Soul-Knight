using Edgar.Unity.Exceptions;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Utility functions for conversions between cell and local positions.
    /// </summary>
    public static class GridUtilsGrid3D
    {
        public static Vector3 CellToLocal(Vector3 cellPosition, Vector3 cellSize)
        {
            return Vector3.Scale(cellPosition, cellSize);
        }

        public static Vector3 LocalToCellInterpolated(Vector3 localPosition, Vector3 cellSize)
        {
            var cellPosition = new Vector3(
                localPosition.x / cellSize.x,
                localPosition.y / cellSize.y,
                localPosition.z / cellSize.z
            );

            return cellPosition;
        }

        public static Vector3Int LocalToCell(Vector3 worldPosition, Vector3 cellSize)
        {
            var cellPositionInterpolated = LocalToCellInterpolated(worldPosition, cellSize);
            var cellPosition = SnapInterpolatedToCell(cellPositionInterpolated);

            if (!IsSnappedToCell(cellPositionInterpolated, cellPosition))
            {
                throw new MisalignedPositionException();
            }

            return cellPosition;
        }

        public static bool IsSnappedToCell(Vector3 cellPositionInterpolated, Vector3Int cellPosition)
        {
            return Vector3.Distance(cellPositionInterpolated, cellPosition) < 0.001f;
        }

        public static Vector3Int SnapInterpolatedToCell(Vector3 cellPositionInterpolated)
        {
            return cellPositionInterpolated.PrecisionRound().Floor();
        }

        public static Vector3Int SnapInterpolatedToCellRound(Vector3 cellPositionInterpolated)
        {
            return cellPositionInterpolated.PrecisionRound().Round();
        }
    }
}
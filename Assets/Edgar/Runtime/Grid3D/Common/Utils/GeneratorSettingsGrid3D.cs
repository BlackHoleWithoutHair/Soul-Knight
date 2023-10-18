using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Settings of the dungeon generator.
    /// The main purpose right now is to configure the cell size of the grid.
    /// </summary>
    [CreateAssetMenu(fileName = "GeneratorSettings", menuName = "Edgar (Grid3D)/Generator settings")]
    public class GeneratorSettingsGrid3D : ScriptableObject
    {
        /// <summary>
        /// Cell size of the grid that room templates and generated levels are placed on.
        /// </summary>
        // TODO(Grid3D): Make sure there are no negative components
        public Vector3 CellSize = new Vector3(1, 1, 1);

        /// <summary>
        /// When blocks are computed directly from colliders, this property specifies the tolerance of this computation.
        /// </summary>
        public float ColliderSizeTolerance = 0.1f;

        public RoomTemplateOutlineComputationModeGrid3D OutlineComputationMode = RoomTemplateOutlineComputationModeGrid3D.AtRuntime;

        public Vector3 CellToLocal(Vector3 localPosition)
        {
            return GridUtilsGrid3D.CellToLocal(localPosition, CellSize);
        }

        public Vector3Int LocalToCell(Vector3 localPosition)
        {
            return GridUtilsGrid3D.LocalToCell(localPosition, CellSize);
        }

        public Vector3 LocalToCellInterpolated(Vector3 localPosition)
        {
            return GridUtilsGrid3D.LocalToCellInterpolated(localPosition, CellSize);
        }
    }
}
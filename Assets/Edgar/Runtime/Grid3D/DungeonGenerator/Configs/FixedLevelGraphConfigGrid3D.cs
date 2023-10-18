using System;

namespace Edgar.Unity
{
    [Serializable]
    public class FixedLevelGraphConfigGrid3D
    {
        /// <summary>
        /// Level graph that will be used in the generator.
        /// </summary>
        public LevelGraph LevelGraph;

        /// <summary>
        /// Whether to add corridors between individual rooms in the level graph.
        /// </summary>
        public bool UseCorridors = true;

        /// <summary>
        /// Global override for the "Allow rotation" setting on individual room templates.
        /// </summary>
        public AllowRotationOverrideGrid3D AllowRotationOverride = AllowRotationOverrideGrid3D.NoOverride;

        public bool FixElevationsInsideCycles = false;
    }
}
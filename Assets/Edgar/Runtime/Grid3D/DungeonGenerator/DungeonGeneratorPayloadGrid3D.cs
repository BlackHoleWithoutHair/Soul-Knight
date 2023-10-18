using System;

namespace Edgar.Unity
{
    public class DungeonGeneratorPayloadGrid3D
    {
        public LevelDescriptionGrid3D LevelDescription { get; set; }

        public DungeonGeneratorLevelGrid3D GeneratedLevel { get; set; }

        public Random Random { get; set; }

        public GeneratorStats GeneratorStats { get; set; }

        public DungeonGeneratorGrid3D DungeonGenerator { get; set; }
    }
}
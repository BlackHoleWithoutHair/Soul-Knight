using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Edgar.Unity
{
    /// <inheritdoc />
    public class DungeonGeneratorLevelGrid3D : GeneratedLevelBase<RoomInstanceGrid3D, LevelDescriptionGrid3D>
    {
        /// <summary>
        /// Instance of the random numbers generator.
        /// </summary>
        public Random Random { get; }

        /// <summary>
        /// Generator settings.
        /// </summary>
        public GeneratorSettingsGrid3D GeneratorSettings { get; }

        public DungeonGeneratorLevelGrid3D(Dictionary<RoomBase, RoomInstanceGrid3D> roomInstances, GameObject rootGameObject, LevelDescriptionGrid3D levelDescription, Random random, GeneratorSettingsGrid3D generatorSettings) : base(roomInstances, rootGameObject, levelDescription)
        {
            Random = random;
            GeneratorSettings = generatorSettings;
        }
    }
}
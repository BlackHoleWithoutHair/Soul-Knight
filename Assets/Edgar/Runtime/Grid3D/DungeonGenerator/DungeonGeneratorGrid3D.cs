using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity
{
    [AddComponentMenu("Edgar/Grid3D/Dungeon Generator (Grid3D)")]
    public class DungeonGeneratorGrid3D : LevelGeneratorBase<DungeonGeneratorPayloadGrid3D>
    {
        public DungeonGeneratorInputTypeGrid2D InputType;

        [ExpandableScriptableObject]
        public DungeonGeneratorInputBaseGrid3D CustomInput;

        /// <summary>
        /// Whether to use a random seed.
        /// </summary>
        public bool UseRandomSeed = true;

        /// <summary>
        /// Which seed should be used for the random numbers generator.
        /// Is used only when UseRandomSeed is false.
        /// </summary>
        public int RandomGeneratorSeed;

        protected override bool ThrowExceptionImmediately => false;

        [Expandable]
        public FixedLevelGraphConfigGrid3D FixedLevelGraphConfig;

        [Expandable]
        public DungeonGeneratorConfigGrid3D GeneratorConfig;

        [Expandable]
        public PostProcessingConfigGrid3D PostProcessingConfig;

        [ExpandableScriptableObject(CanFold = false)]
        public List<DungeonGeneratorPostProcessingGrid3D> CustomPostProcessingTasks;

        /// <summary>
        /// Whether to generate a level on enter play mode.
        /// </summary>
        public bool GenerateOnStart = true;

        /// <summary>
        /// Disable all custom post-processing tasks.
        /// </summary>
        public bool DisableCustomPostProcessing = false;

        public void Start()
        {
            if (GenerateOnStart)
            {
                Generate();
            }
        }

        protected override (List<IPipelineTask<DungeonGeneratorPayloadGrid3D>> pipelineItems, DungeonGeneratorPayloadGrid3D payload) GetPipelineItemsAndPayload()
        {
            var payload = new DungeonGeneratorPayloadGrid3D()
            {
                Random = GetRandomNumbersGenerator(UseRandomSeed, RandomGeneratorSeed),
                DungeonGenerator = this,
            };

            var postProcessingTasks = !DisableCustomPostProcessing
                ? CustomPostProcessingTasks
                : new List<DungeonGeneratorPostProcessingGrid3D>();

            var postProcessingComponents = !DisableCustomPostProcessing
                ? GetComponents<DungeonGeneratorPostProcessingComponentGrid3D>().ToList()
                : new List<DungeonGeneratorPostProcessingComponentGrid3D>();

            var pipelineItems = new List<IPipelineTask<DungeonGeneratorPayloadGrid3D>>
            {
                InputType == DungeonGeneratorInputTypeGrid2D.FixedLevelGraph
                    ? new FixedLevelGraphInputGrid3D(FixedLevelGraphConfig)
                    :  CustomInput,
                new DungeonGeneratorTaskGrid3D(GeneratorConfig),
                new PostProcessingTaskGrid3D(PostProcessingConfig, postProcessingTasks, postProcessingComponents)
            };

            return (pipelineItems, payload);
        }
    }
}
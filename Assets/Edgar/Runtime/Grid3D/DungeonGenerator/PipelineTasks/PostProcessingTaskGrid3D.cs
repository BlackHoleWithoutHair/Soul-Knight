using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Edgar.Unity
{
    public delegate void DungeonGeneratorPostProcessingCallbackGrid3D(DungeonGeneratorLevelGrid3D level);

    /// <summary>
    /// Handles individual post-processing steps.
    /// </summary>
    internal class PostProcessingTaskGrid3D : PipelineTaskGrid3D
    {
        private readonly PostProcessingConfigGrid3D config;

        private readonly List<DungeonGeneratorPostProcessingGrid3D> customPostProcessingTasks;
        private readonly List<DungeonGeneratorPostProcessingComponentGrid3D> customPostProcessingComponents;

        public PostProcessingTaskGrid3D(
            PostProcessingConfigGrid3D config,
            List<DungeonGeneratorPostProcessingGrid3D> customPostProcessingTasks,
            List<DungeonGeneratorPostProcessingComponentGrid3D> customPostProcessingComponents)
        {
            this.config = config;
            this.customPostProcessingTasks = customPostProcessingTasks;
            this.customPostProcessingComponents = customPostProcessingComponents;
        }

        public override IEnumerator Process()
        {
            var callbacks = new DungeonGeneratorCallbacksGrid3D();

            // Register default callbacks
            RegisterCallbacks(callbacks);

            var tasksAndComponents = customPostProcessingTasks
                .Cast<IDungeonGeneratorPostProcessing<DungeonGeneratorLevelGrid3D, DungeonGeneratorCallbacksGrid3D>>()
                .Concat(customPostProcessingComponents);

            // Register custom callbacks from scriptable objects and components
            foreach (var postProcessingTask in tasksAndComponents)
            {
                if (postProcessingTask == null)
                {
                    continue;
                }

                postProcessingTask.SetRandomGenerator(Payload.Random);
                callbacks.RegisterAfterAll(postProcessingTask.Run);
                postProcessingTask.RegisterCallbacks(callbacks);
            }

            // Run callbacks
            foreach (var callback in callbacks.GetCallbacks())
            {
                callback(Payload.GeneratedLevel);
                yield return null;
            }
        }

        private void RegisterCallbacks(DungeonGeneratorCallbacksGrid3D callbacks)
        {
            if (config.CenterLevel)
            {
                callbacks.RegisterCallback(PostProcessPrioritiesGrid3D.CenterLevel,
                    PostProcessingUtilsGrid3D.CenterLevel);
            }

            if (config.ProcessConnectorsAndBlockers)
            {
                callbacks.RegisterCallback(PostProcessPrioritiesGrid3D.ProcessConnectorsAndBlockers,
                    level => PostProcessingUtilsGrid3D.ProcessConnectorsAndBlockers(level, config.AddConnectors, config.AddBlockers));
            }
        }
    }
}
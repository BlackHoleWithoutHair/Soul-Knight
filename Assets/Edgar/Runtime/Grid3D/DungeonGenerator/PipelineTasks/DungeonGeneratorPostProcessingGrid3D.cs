using UnityEngine;
using Random = System.Random;

namespace Edgar.Unity
{
    /// <summary>
    /// Base class for custom post-processing logic.
    /// </summary>
    public abstract class DungeonGeneratorPostProcessingGrid3D : ScriptableObject, IDungeonGeneratorPostProcessing<DungeonGeneratorLevelGrid3D, DungeonGeneratorCallbacksGrid3D>
    {
        public Random Random { get; private set; }

        /// <summary>
        /// Runs the post-processing logic with a given generated level and corresponding level description.
        /// </summary>
        public virtual void Run(DungeonGeneratorLevelGrid3D level)
        {
        }

        public void SetRandomGenerator(Random random)
        {
            Random = random;
        }

        public virtual void RegisterCallbacks(DungeonGeneratorCallbacksGrid3D callbacks)
        {
            /* empty, prepared to be overriden */
        }
    }
}
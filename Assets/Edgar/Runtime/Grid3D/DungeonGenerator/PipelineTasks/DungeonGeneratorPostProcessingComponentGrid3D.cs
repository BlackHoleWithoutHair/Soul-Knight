using UnityEngine;
using Random = System.Random;

namespace Edgar.Unity
{
    /// <summary>
    /// Base class for post-processing logic implemented as a MonoBehaviour.
    /// </summary>
    public abstract class DungeonGeneratorPostProcessingComponentGrid3D : MonoBehaviour, IDungeonGeneratorPostProcessing<DungeonGeneratorLevelGrid3D, DungeonGeneratorCallbacksGrid3D>
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
using UnityEngine;
using Random = System.Random;

namespace Edgar.Unity
{
    public delegate void DungeonGeneratorPostProcessCallbackGrid2D(DungeonGeneratorLevelGrid2D level);

    /// <summary>
    /// Base class for custom post-processing logic.
    /// </summary>
    public class DungeonGeneratorPostProcessingGrid2D : ScriptableObject, IDungeonGeneratorPostProcessing<DungeonGeneratorLevelGrid2D, DungeonGeneratorCallbacksGrid2D>
    {
        public Random Random { get; private set; }

        /// <inheritdoc />
        public virtual void Run(DungeonGeneratorLevelGrid2D level)
        {
        }

        public virtual void RegisterCallbacks(DungeonGeneratorCallbacksGrid2D callbacks)
        {
            /* empty */
        }

        public void SetRandomGenerator(Random random)
        {
            Random = random;
        }
    }
}
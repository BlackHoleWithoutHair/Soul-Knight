using System.Collections;

namespace Edgar.Unity
{
    /// <summary>
    /// Interface that represents a level generator.
    /// </summary>
    public interface ILevelGenerator
    {
        IEnumerator GenerateCoroutine();

        /// <summary>
        /// Generates and returns a level.
        /// </summary>
        object Generate();
    }
}
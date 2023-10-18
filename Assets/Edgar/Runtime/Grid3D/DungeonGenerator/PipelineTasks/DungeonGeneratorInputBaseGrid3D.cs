using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Edgar.Unity
{
    /// <summary>
    /// Base class for all input handlers.
    /// </summary>
    public abstract class DungeonGeneratorInputBaseGrid3D : ScriptableObject, IPipelineTask<DungeonGeneratorPayloadGrid3D>
    {
        public Random Random { get; private set; }

        DungeonGeneratorPayloadGrid3D IPipelineTask<DungeonGeneratorPayloadGrid3D>.Payload { get; set; }

        public IEnumerator Process()
        {
            var payload = ((IPipelineTask<DungeonGeneratorPayloadGrid3D>)this).Payload;

            Random = payload.Random;
            payload.LevelDescription = GetLevelDescription();

            yield return null;
        }

        protected abstract LevelDescriptionGrid3D GetLevelDescription();
    }
}
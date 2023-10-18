using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Edgar.Unity
{
    public abstract class DungeonGeneratorInputBaseGrid2D : ScriptableObject, IPipelineTask<DungeonGeneratorPayloadGrid2D>
    {
        [Obsolete("Access the Random instance via the Random property.")]
        public DungeonGeneratorPayloadGrid2D Payload { get; set; }

        public Random Random { get; private set; }

        public IEnumerator Process()
        {
#pragma warning disable CS0618
            Random = Payload.Random;
            Payload.LevelDescription = GetLevelDescription();
#pragma warning restore CS0618

            yield return null;
        }

        protected abstract LevelDescriptionGrid2D GetLevelDescription();
    }
}
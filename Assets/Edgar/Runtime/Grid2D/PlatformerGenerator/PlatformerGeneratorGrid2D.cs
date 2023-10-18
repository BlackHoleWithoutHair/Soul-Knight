using System;
using UnityEngine;

namespace Edgar.Unity
{
    [AddComponentMenu("Edgar/Grid2D/Platformer Generator (Grid2D)")]
    public class PlatformerGeneratorGrid2D : DungeonGeneratorBaseGrid2D
    {
        protected override Func<ITilemapLayersHandlerGrid2D> GetTilemapLayersHandler()
        {
            return () => new PlatformerTilemapLayersHandlerGrid2D();
        }
    }
}
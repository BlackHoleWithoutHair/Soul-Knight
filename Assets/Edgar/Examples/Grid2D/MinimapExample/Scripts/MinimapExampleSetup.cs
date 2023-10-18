using System;
using UnityEngine;

namespace Edgar.Unity.Examples.MinimapExample
{
    [CreateAssetMenu(menuName = "Edgar/Examples/Minimap/Setup", fileName = "MinimapExampleSetup")]
    public class MinimapExampleSetup : DungeonGeneratorPostProcessingGrid2D
    {
        public MinimapPostProcessGrid2D MinimapPostProcess;
        private const string MinimapLayer = "Minimap";

        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            var layer = LayerMask.NameToLayer(MinimapLayer);

            if (layer == -1)
            {
                Debug.LogError("The Minimap example scene needs a layer called \"Minimap\" to function properly. Please create the layer and try again.");
                throw new InvalidOperationException();
            }

            if (MinimapPostProcess != null)
            {
                MinimapPostProcess.Layer = layer;
            }

            var minimapGameObject = GameObject.Find("Minimap");
            var camera = minimapGameObject?.GetComponentInChildren<Camera>();

            if (camera != null)
            {
                camera.cullingMask = LayerMask.GetMask(MinimapLayer);
            }

            Camera.main.cullingMask = ~LayerMask.GetMask(MinimapLayer);
        }
    }
}
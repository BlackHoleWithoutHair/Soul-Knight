using UnityEngine;

namespace Edgar.Unity
{
    [AddComponentMenu("Edgar/Grid2D/Fog Of War Additional Camera (Grid2D)")]
    public class FogOfWarAdditionalCameraGrid2D : MonoBehaviour
    {
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            FogOfWarGrid2D.Instance?.OnRenderImage(source, destination, GetComponent<Camera>());
        }
    }
}
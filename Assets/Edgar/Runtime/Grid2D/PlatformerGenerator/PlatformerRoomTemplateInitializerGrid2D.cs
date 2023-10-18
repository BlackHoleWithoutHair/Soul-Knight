using UnityEditor;
using UnityEngine;

namespace Edgar.Unity
{
    [AddComponentMenu("Edgar/Grid2D/Platformer Room Template Initializer (Grid2D)")]
    public class PlatformerRoomTemplateInitializerGrid2D : RoomTemplateInitializerBaseGrid2D
    {
        protected override void InitializeTilemaps(GameObject tilemapsRoot)
        {
            var tilemapLayersHandlers = new PlatformerTilemapLayersHandlerGrid2D();
            tilemapLayersHandlers.InitializeTilemaps(tilemapsRoot);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Edgar/Platformer room template")]
        public static void CreateRoomTemplatePrefab()
        {
            RoomTemplateInitializerUtilsGrid2D.CreateRoomTemplatePrefab<PlatformerRoomTemplateInitializerGrid2D>();
        }
#endif
    }
}
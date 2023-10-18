using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Edgar.Unity.Examples.Metroidvania
{
    public class MetroidvaniaRoomTemplateInitializer : RoomTemplateInitializerBaseGrid2D
    {
        public override void Initialize()
        {
            base.Initialize();

            var outlineHandler = gameObject.AddComponent<BoundingBoxOutlineHandlerGrid2D>();
            outlineHandler.PaddingTop = 3;
        }

        protected override void InitializeTilemaps(GameObject tilemapsRoot)
        {
            var tilemapLayersHandlers = new PlatformerTilemapLayersHandlerGrid2D();
            tilemapLayersHandlers.InitializeTilemaps(tilemapsRoot);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Edgar/Examples/Metroidvania/Room template outside")]
        public static void CreateRoomTemplatePrefab()
        {
            RoomTemplateInitializerUtilsGrid2D.CreateRoomTemplatePrefab<MetroidvaniaRoomTemplateInitializer>();
        }
#endif
    }
}
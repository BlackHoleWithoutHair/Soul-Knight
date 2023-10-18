using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Edgar.Unity.Examples.Isometric1
{
    public class Isometric1RoomTemplateInitializer : RoomTemplateInitializerBaseGrid2D
    {
        protected override void InitializeTilemaps(GameObject tilemapsRoot)
        {
            var tilemapLayersHandlers = ScriptableObject.CreateInstance<Isometric1TilemapLayersHandler>();
            tilemapLayersHandlers.InitializeTilemaps(tilemapsRoot);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Edgar/Examples/Isometric 1/Room template")]
        public static void CreateRoomTemplatePrefab()
        {
            RoomTemplateInitializerUtilsGrid2D.CreateRoomTemplatePrefab<Isometric1RoomTemplateInitializer>();
        }
#endif
    }
}
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Edgar.Unity.Examples.Gungeon
{
    public class GungeonRoomTemplateInitializer : RoomTemplateInitializerBaseGrid2D
    {
        public override void Initialize()
        {
            base.Initialize();

            // Custom behaviour
            gameObject.AddComponent<GungeonRoomManager>();
        }

        protected override void InitializeTilemaps(GameObject tilemapsRoot)
        {
            var tilemapLayersHandler = ScriptableObject.CreateInstance<GungeonTilemapLayersHandler>();
            tilemapLayersHandler.InitializeTilemaps(tilemapsRoot);

            // Custom behaviour
            tilemapsRoot.transform.Find("Floor").gameObject.AddComponent<GungeonCurrentRoomHandler>();
        }

        protected override void InitializeDoors()
        {
            base.InitializeDoors();

            //var doors = gameObject.GetComponent<Doors>();
            //doors.DistanceFromCorners = 2;
            //doors.DoorLength = 2;
        }


#if UNITY_EDITOR
        [MenuItem("Assets/Create/Edgar/Examples/Gungeon/Room template")]
        public static void CreateRoomTemplatePrefab()
        {
            RoomTemplateInitializerUtilsGrid2D.CreateRoomTemplatePrefab<GungeonRoomTemplateInitializer>();
        }
#endif
    }
}
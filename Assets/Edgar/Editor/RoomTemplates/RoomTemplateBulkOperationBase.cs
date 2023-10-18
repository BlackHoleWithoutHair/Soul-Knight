using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    public class RoomTemplateBulkOperationBase
    {
        private bool IsPrefab(GameObject gameObject)
        {
            return gameObject.scene.name == null;
        }

        private bool Run(GameObject gameObject)
        {
            //if (gameObject.GetComponent<GungeonRoomManager>() != null)
            //{
            //    Object.DestroyImmediate(gameObject.GetComponent<GungeonRoomManager>(), true);
            //}

            //foreach (var gungeonCurrentRoomHandler in gameObject.GetComponentsInChildren<GungeonCurrentRoomHandler>())
            //{
            //    Object.DestroyImmediate(gungeonCurrentRoomHandler, true);
            //}

            //foreach (var tilemap in gameObject.GetComponentsInChildren<Tilemap>())
            //{
            //    tilemap.gameObject.layer = LayerMask.NameToLayer("Default");
            //}

            //var spawnPositions = new GameObject("EnemySpawnPoints");
            //spawnPositions.transform.parent = gameObject.transform;

            //for (int i = 0; i < 3; i++)
            //{
            //    var spawnPosition = new GameObject();
            //    spawnPosition.transform.parent = spawnPositions.transform;
            //}

            return true;
        }

        public void Run(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (IsPrefab(gameObject))
                {
                    if (Run(gameObject))
                    {
                        EditorUtility.SetDirty(gameObject);
                    }
                }
            }
        }

        //[MenuItem("Assets/Create/Dungeon generator/Test")]
        //public static void CreateRoomTemplatePrefab()
        //{
        //    var selectedGameObjects = Selection.gameObjects;

        //    var operation = new RoomTemplateBulkOperationBase();
        //    operation.Run(selectedGameObjects);
        //}
    }
}
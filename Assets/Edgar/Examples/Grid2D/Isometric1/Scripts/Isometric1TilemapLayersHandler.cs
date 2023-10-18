using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity.Examples.Isometric1
{
    [CreateAssetMenu(menuName = "Edgar/Examples/Isometric 1/Tilemap layers handler", fileName = "TilemapLayersHandler")]
    public class Isometric1TilemapLayersHandler : TilemapLayersHandlerBaseGrid2D
    {
        public bool HideCollidersLayer = false;

        public override void InitializeTilemaps(GameObject gameObject)
        {
            var grid = gameObject.AddComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSize = new Vector3(1, 0.5f, 0.01f);

            var level0Floor = CreateTilemapGameObject("Level 0 - Floor", gameObject, 0, 0);
            var level0Walls = CreateTilemapGameObject("Level 0 - Walls", gameObject, 1, 0);

            var level05Floor = CreateTilemapGameObject("Level 0.5 - Floor", gameObject, 2, 0.5f);
            var level05Walls = CreateTilemapGameObject("Level 0.5 - Walls", gameObject, 3, 0.5f);

            var level1Floor = CreateTilemapGameObject("Level 1 - Floor", gameObject, 4, 1);
            var level1Walls = CreateTilemapGameObject("Level 1 - Walls", gameObject, 5, 1);

            var colliders = CreateTilemapGameObject("Colliders", gameObject, 6, 0, !HideCollidersLayer);
            AddCollider(colliders);
        }

        protected GameObject CreateTilemapGameObject(string name, GameObject parentObject, int sortingOrder, float level, bool addRenderer = true)
        {
            var tilemapObject = new GameObject(name);
            tilemapObject.transform.SetParent(parentObject.transform);
            var tilemap = tilemapObject.AddComponent<Tilemap>();
            tilemap.tileAnchor = new Vector3(1.23f, 1.23f, 0) * level;

            if (addRenderer)
            {
                var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
                tilemapRenderer.sortingOrder = sortingOrder;
                tilemapRenderer.mode = TilemapRenderer.Mode.Individual;
            }

            return tilemapObject;
        }

        protected void AddCollider(GameObject gameObject)
        {
            var tilemapCollider2D = gameObject.AddComponent<TilemapCollider2D>();
            tilemapCollider2D.usedByComposite = true;

            gameObject.AddComponent<CompositeCollider2D>();
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
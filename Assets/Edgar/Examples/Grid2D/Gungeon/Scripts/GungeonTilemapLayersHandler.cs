﻿using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity.Examples.Gungeon
{
    [CreateAssetMenu(menuName = "Edgar/Examples/Gungeon/Tilemap layers handler", fileName = "TilemapLayersHandler")]
    public class GungeonTilemapLayersHandler : TilemapLayersHandlerBaseGrid2D
    {
        /// <summary>
        ///     Initializes individual tilemap layers.
        /// </summary>
        /// <param name="gameObject"></param>
        public override void InitializeTilemaps(GameObject gameObject)
        {
            gameObject.AddComponent<Grid>();

            var wallsTilemapObject = CreateTilemapGameObject("Walls", gameObject, 0);
            AddCompositeCollider(wallsTilemapObject);

            var floorTilemapObject = CreateTilemapGameObject("Floor", gameObject, 1);
            AddCompositeCollider(floorTilemapObject, true);

            var collideableTilemapObject = CreateTilemapGameObject("Collideable", gameObject, 2);
            AddCompositeCollider(collideableTilemapObject);

            var other1TilemapObject = CreateTilemapGameObject("Other 1", gameObject, 3);

            var other2TilemapObject = CreateTilemapGameObject("Other 2", gameObject, 4);

            var other3TilemapObject = CreateTilemapGameObject("Other 3", gameObject, 5);
        }

        protected GameObject CreateTilemapGameObject(string name, GameObject parentObject, int sortingOrder)
        {
            var tilemapObject = new GameObject(name);
            tilemapObject.transform.SetParent(parentObject.transform);
            var tilemap = tilemapObject.AddComponent<Tilemap>();
            var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
            tilemapRenderer.sortingOrder = sortingOrder;

            return tilemapObject;
        }

        protected void AddCompositeCollider(GameObject gameObject, bool isTrigger = false)
        {
            var tilemapCollider2D = gameObject.AddComponent<TilemapCollider2D>();
            tilemapCollider2D.usedByComposite = true;

            var compositeCollider2d = gameObject.AddComponent<CompositeCollider2D>();
            compositeCollider2d.geometryType = CompositeCollider2D.GeometryType.Polygons;
            compositeCollider2d.isTrigger = isTrigger;

            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
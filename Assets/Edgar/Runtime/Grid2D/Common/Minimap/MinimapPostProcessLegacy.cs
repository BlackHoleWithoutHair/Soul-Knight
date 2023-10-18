using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity
{
    [CreateAssetMenu(menuName = "Edgar/Features/Minimap post-process (legacy)", fileName = "MinimapPostProcess")]
    public class MinimapPostProcessLegacy : DungeonGeneratorPostProcessingGrid2D
    {
        [Layer]
        public int Layer = 0;

        public string WallsTilemaps = "Walls";

        public string FloorTilemaps = "Floor";

        [Range(0, 1)]
        public float WallSize = 0.5f;

        public Color WallsColor = new Color(0.72f, 0.72f, 0.72f);

        public Color FloorColor = new Color(0.18f, 0.2f, 0.34f);

        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            // Create new tilemap layer for the level map
            var tilemapsRoot = level.RootGameObject.transform.Find(GeneratorConstantsGrid2D.TilemapsRootName);
            var tilemapObject = new GameObject("Minimap");
            tilemapObject.transform.SetParent(tilemapsRoot);
            tilemapObject.transform.localPosition = Vector3.zero;
            var tilemap = tilemapObject.AddComponent<Tilemap>();
            var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
            tilemapRenderer.sortingOrder = 20;

            // TODO: check that the layer exists
            // Assign special layer
            CheckLayerValidity();

            tilemapObject.layer = Layer;

            // Copy wall tiles
            CopyTilesToLevelMap(level, WallsTilemaps.Split(','), tilemap, CreateTileFromColor(WallsColor));

            var floorPpu = 1 / (1 + (1 - WallSize) * 2);
            CopyTilesToLevelMap(level, FloorTilemaps.Split(','), tilemap, CreateTileFromColor(FloorColor, floorPpu));
        }

        private void CheckLayerValidity()
        {
            if (Layer == 0)
            {
                Debug.LogWarning($"It seems like you are using the Default layer in the MinimapPostProcessing script. That is usually considered an error, please choose a layer that is intended for the minimap.");
            }

            if (string.IsNullOrEmpty(LayerMask.LayerToName(Layer)))
            {
                Debug.LogWarning($"It seems like you are using an unknown layer in the MinimapPostProcessing script. Please check that the Layer field is properly set and points to an existing layer.");
            }
        }

        private TileBase CreateTileFromColor(Color color, float pixelsPerUnit = 1)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            tile.sprite = sprite;

            return tile;
        }

        /// <summary>
        /// Copy tiles from given source tilemaps to the level map tilemap.
        /// Instead of using the original tiles, we use a given level map tile (which is usually only a single color).
        /// If we want to copy only some of the tiles, we can provide a tile filter function.
        /// </summary>
        private void CopyTilesToLevelMap(DungeonGeneratorLevelGrid2D level, ICollection<string> sourceTilemapNames, Tilemap levelMapTilemap, TileBase levelMapTile, Predicate<TileBase> tileFilter = null)
        {
            // Go through the tilemaps with the correct name
            foreach (var sourceTilemap in level.GetSharedTilemaps().Where(x => sourceTilemapNames.Contains(x.name)))
            {
                // Go through positions inside the bounds of the tilemap
                foreach (var tilemapPosition in sourceTilemap.cellBounds.allPositionsWithin)
                {
                    // Check if there is a tile at a given position
                    var originalTile = sourceTilemap.GetTile(tilemapPosition);

                    if (originalTile != null)
                    {
                        // If a tile filter is provided, use it to check if the predicate holds
                        if (tileFilter != null)
                        {
                            if (tileFilter(originalTile))
                            {
                                levelMapTilemap.SetTile(tilemapPosition, levelMapTile);
                            }
                        }
                        // Otherwise set the levelMapTile to the correct position
                        else
                        {
                            levelMapTilemap.SetTile(tilemapPosition, levelMapTile);
                        }
                    }
                }
            }
        }
    }
}
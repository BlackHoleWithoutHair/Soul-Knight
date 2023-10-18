using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity
{
    /// <summary>
    /// Post-processing task that computes tiles for the minimap.
    /// </summary>
    [CreateAssetMenu(menuName = "Edgar/Features/Minimap post-process", fileName = "MinimapPostProcess")]
    public class MinimapPostProcessGrid2D : DungeonGeneratorPostProcessingGrid2D
    {
        [Layer]
        public int Layer = 0;

        public bool UseMultipleTilemaps = true;

        private readonly MinimapOverwriteMode overwriteMode = MinimapOverwriteMode.AllowMultiple;

        [HelpBox("If you need to change the order of layers, please open this post-processing task in a standalone inspector window and there the layers can be reordered.")]
        [Expandable]
        public MinimapLayer[] Layers = new[]
        {
            new MinimapLayer()
            {
                DisplayMode = MinimapDisplayMode.Color,
                Tilemaps = "Walls",
                Color = new Color(0.72f, 0.72f, 0.72f),
            },
            new MinimapLayer()
            {
                DisplayMode = MinimapDisplayMode.Color,
                Tilemaps = "Floor",
                Color = new Color(0.18f, 0.2f, 0.34f),
            },
        };

        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            // Check if custom layer exists
            CheckLayerValidity();

            // Create new tilemap layers for the minimap
            var firstMaterial = Layers.Select(x => x.Material).FirstOrDefault(x => x != null);
            var mainTilemap = !UseMultipleTilemaps ? CreateTilemap("Minimap", level, 0, firstMaterial) : null;

            var usedTiles = new HashSet<Vector3Int>();

            // Create tilemaps
            var tilemaps = new List<Tilemap>();
            foreach (var minimapLayer in Layers)
            {
                // Create a new minimap if UseMultipleTilemaps is set to true
                var tilemap = UseMultipleTilemaps
                    ? CreateTilemap($"Minimap {minimapLayer.Tilemaps}", level, -20 + tilemaps.Count, minimapLayer.Material)
                    : mainTilemap;

                tilemaps.Add(tilemap);
            }

            // Go through individual minimap layers
            for (var i = 0; i < Layers.Length; i++)
            {
                var index = i;

                if (overwriteMode == MinimapOverwriteMode.PreferLowerLayers)
                {
                    index = Layers.Length - i - 1;
                }

                var minimapLayer = Layers[index];
                var tilemap = tilemaps[index];
                TileBase tile = null;

                // Choose tile based on DisplayMode
                switch (minimapLayer.DisplayMode)
                {
                    case MinimapDisplayMode.Color:
                        var ppu = 1 / minimapLayer.Size;
                        tile = CreateTileFromColor(minimapLayer.Color, ppu);
                        break;
                    case MinimapDisplayMode.CustomTile:
                        tile = minimapLayer.Tile;

                        if (tile == null)
                        {
                            throw new ArgumentException($"{nameof(MinimapLayer.Tile)} must not be null when using {nameof(MinimapLayer.DisplayMode)} set to {nameof(MinimapDisplayMode.CustomTile)}");
                        }

                        break;
                    case MinimapDisplayMode.OriginalTiles:
                        /* empty - null tile is used to indicate that the original tiles should be used */
                        break;
                }

                var tiles = CopyTilesToLevelMap(level, minimapLayer.Tilemaps.Split(','), tilemap, tile, usedTiles);

                if (overwriteMode != MinimapOverwriteMode.AllowMultiple)
                {
                    foreach (var usedTile in tiles)
                    {
                        usedTiles.Add(usedTile);
                    }
                }
            }
        }

        private Tilemap CreateTilemap(string tilemapName, DungeonGeneratorLevelGrid2D level, int sortingOrder, Material material)
        {
            var tilemapsRoot = level.RootGameObject.transform.Find(GeneratorConstantsGrid2D.TilemapsRootName);
            var tilemapObject = new GameObject(tilemapName);
            tilemapObject.layer = Layer;
            tilemapObject.transform.SetParent(tilemapsRoot);
            tilemapObject.transform.localPosition = Vector3.zero;
            var tilemap = tilemapObject.AddComponent<Tilemap>();
            var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
            tilemapRenderer.sortingOrder = sortingOrder;

            if (material != null)
            {
                tilemapRenderer.material = material;
            }

            return tilemap;
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
        private HashSet<Vector3Int> CopyTilesToLevelMap(DungeonGeneratorLevelGrid2D level, ICollection<string> sourceTilemapNames, Tilemap levelMapTilemap, TileBase levelMapTile, HashSet<Vector3Int> ignoredTiles = null)
        {
            var usedTiles = new HashSet<Vector3Int>();

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
                        if (ignoredTiles != null && !ignoredTiles.Contains(tilemapPosition))
                        {
                            levelMapTilemap.SetTile(tilemapPosition, levelMapTile ?? originalTile);
                            usedTiles.Add(tilemapPosition);
                        }
                    }
                }
            }

            return usedTiles;
        }

        private void OnValidate()
        {
            foreach (var minimapLayer in Layers)
            {
                if (minimapLayer.Size < 1)
                {
                    minimapLayer.Size = 1;
                }
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(MinimapPostProcessGrid2D), true)]
        public class Inspector : UnityEditor.Editor
        {
            private ReorderableList layersList;

            public void OnEnable()
            {
                layersList = new ReorderableList(new UnityEditorInternal.ReorderableList(serializedObject,
                    serializedObject.FindProperty(nameof(Layers)),
                    true, true, true, true), "Layers");
            }

            public override void OnInspectorGUI()
            {
                DrawPropertiesExcluding(serializedObject, nameof(Layers));
                layersList.DoLayoutList();

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}
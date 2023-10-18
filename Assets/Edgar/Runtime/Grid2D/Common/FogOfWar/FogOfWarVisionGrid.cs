using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Holds data about which tiles are currently visible when the fog of war is applied.
    /// </summary>
    internal class FogOfWarVisionGrid
    {
        private readonly Dictionary<Vector2Int, TileInfo> values = new Dictionary<Vector2Int, TileInfo>();
        private readonly HashSet<Vector2Int> changedTiles = new HashSet<Vector2Int>();
        private bool hasChanges;
        private Texture2D texture;
        private Texture2D textureInterpolated;

        private Texture2D defaultTexture;
        private Texture2D defaultTextureInterpolated;

        private RectInt boundingRectangle;

        private readonly TextureFormat textureFormat = TextureFormat.RGBA32;
        private readonly TextureFormat textureFormatInterpolated = TextureFormat.RGBA32;

        private readonly int bytesPerColor = 4;
        private readonly int bytesPerColorInterpolated = 4;
        private readonly float initialFogValue;

        public FogOfWarVisionGrid(float initialFogValue = 0)
        {
            this.initialFogValue = initialFogValue;

            // Save some CPU time by using smaller texture formats if possible
            if (SystemInfo.SupportsTextureFormat(TextureFormat.RGB24))
            {
                textureFormat = TextureFormat.RGB24;
                bytesPerColor = 3;
            }

            if (SystemInfo.SupportsTextureFormat(TextureFormat.R8))
            {
                textureFormatInterpolated = TextureFormat.R8;
                bytesPerColorInterpolated = 1;
            }
        }

        /// <summary>
        /// Check if any tile changed in the vision grid.
        /// </summary>
        /// <returns></returns>
        public bool HasChanges()
        {
            return hasChanges;
        }

        /// <summary>
        /// Reset changes tracker. After calling this function, the HasChanges function always returns false.
        /// </summary>
        public void ResetHasChanges()
        {
            hasChanges = false;
            changedTiles.Clear();
        }

        /// <summary>
        /// Sets vision information of a given tile.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="info"></param>
        public void SetTile(Vector2Int position, TileInfo info)
        {
            values[position] = info;
            changedTiles.Add(position);
            hasChanges = true;
        }

        /// <summary>
        /// Gets vision information of a given tile.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public TileInfo GetTile(Vector2Int position)
        {
            if (values.TryGetValue(position, out var info))
            {
                return info;
            }

            return GetNewTileInfo();
        }

        private Texture2D CreateMainTexture(int width, int height)
        {
            return new Texture2D(width, height, textureFormat, false)
            {
                filterMode = FilterMode.Point,
            };
        }

        private Texture2D CreateInterpolationTexture(int width, int height)
        {
            return new Texture2D(width, height, textureFormatInterpolated, false);
        }

        /// <summary>
        /// Recreates vision textures when a tile does not fit in the original textures.
        /// </summary>
        private void ResizeTextures()
        {
            // Compute the bounds of used tiles
            var minX = values.Keys.Min(x => x.x);
            var minY = values.Keys.Min(x => x.y);
            var maxX = values.Keys.Max(x => x.x);
            var maxY = values.Keys.Max(x => x.y);

            var min = new Vector2Int(minX, minY);
            var max = new Vector2Int(maxX, maxY);

            // Add a margin so that the textures do not have to be recreated too often
            var margin = Math.Max(10, (int)(0.25 * Math.Max(max.x - min.x, max.y - min.y)));
            min -= new Vector2Int(margin, margin);
            max += new Vector2Int(margin, margin);

            // Compute the new bounding rectangle
            var newBoundingRectangle = new RectInt(min, new Vector2Int(max.x - min.x, max.y - min.y));

            var width = newBoundingRectangle.width;
            var height = newBoundingRectangle.height;

            // Initialize colors of the textures
            var colors = new Color[width * height];
            var colorsInterpolated = new Color[width * height];

            // Get an initial tile info and colors
            var tileInfo = GetNewTileInfo();
            var initialColor = GetColor(tileInfo);
            var initialColorInterpolated = GetColorInterpolated(tileInfo);

            for (int i = 0; i < width * height; i++)
            {
                colors[i] = initialColor;
                colorsInterpolated[i] = initialColorInterpolated;
            }

            var newTexture = CreateMainTexture(width, height);
            var newTextureInterpolated = CreateInterpolationTexture(width, height);

            newTexture.SetPixels(colors);
            newTextureInterpolated.SetPixels(colorsInterpolated);

            // Copy values from old textures to the new textures
            if (texture != null)
            {
                var oldColors = texture.GetPixels();
                var oldColorsInterpolated = textureInterpolated.GetPixels();
                var oldBoundingRectangle = boundingRectangle;

                var offsetDifference = oldBoundingRectangle.min - newBoundingRectangle.min;

                newTexture.SetPixels(offsetDifference.x, offsetDifference.y, oldBoundingRectangle.width, oldBoundingRectangle.height, oldColors);
                newTextureInterpolated.SetPixels(offsetDifference.x, offsetDifference.y, oldBoundingRectangle.width, oldBoundingRectangle.height, oldColorsInterpolated);
            }

            // Apply changes
            newTexture.Apply();
            newTextureInterpolated.Apply();

            texture = newTexture;
            textureInterpolated = newTextureInterpolated;
            boundingRectangle = newBoundingRectangle;
        }

        /// <summary>
        /// Gets vision textures that represent the current state of the vision grid.
        /// </summary>
        /// <returns></returns>
        public VisionTextures GetVisionTextures()
        {
            // If we have no information about tiles, return default textures (everything is hidden)
            if (values.Count == 0)
            {
                if (defaultTexture == null)
                {
                    defaultTexture = CreateMainTexture(1, 1);
                    defaultTexture.SetPixel(0, 0, Color.black);
                    defaultTextureInterpolated = CreateInterpolationTexture(1, 1);
                    defaultTextureInterpolated.SetPixel(0, 0, Color.black);
                }

                return new VisionTextures()
                {
                    Texture = defaultTexture,
                    TextureInterpolated = defaultTextureInterpolated,
                };
            }

            // If the textures are not initialized or if some of the changed files do not fit in the textures, resize the textures.
            if (texture == null || changedTiles.Any(x => !boundingRectangle.Contains(x)))
            {
                ResizeTextures();
            }

            // Get the raw texture data for fast access
            var textureData = texture.GetRawTextureData<byte>();
            var textureInterpolatedData = textureInterpolated.GetRawTextureData<byte>();

            // Go through changed tiles and update their vision info
            foreach (var pos in changedTiles)
            {
                var value = values[pos];

                var x = (pos.x - boundingRectangle.xMin);
                var y = (pos.y - boundingRectangle.yMin);

                var color = GetColor(value);
                var colorInterpolated = GetColorInterpolated(value);

                textureData[bytesPerColor * (y * boundingRectangle.width + x)] = (byte)(color.r * 255);
                textureData[bytesPerColor * (y * boundingRectangle.width + x) + 1] = (byte)(color.g * 255);

                textureInterpolatedData[bytesPerColorInterpolated * (y * boundingRectangle.width + x)] = (byte)(colorInterpolated.r * 255);
            }

            // Apply new changes
            texture.Apply();
            textureInterpolated.Apply();

            return new VisionTextures()
            {
                Offset = boundingRectangle.min,
                Texture = texture,
                TextureInterpolated = textureInterpolated,
            };
        }

        private Color GetColor(TileInfo tileInfo)
        {
            return new Color(tileInfo.Value, tileInfo.IsInterpolated ? 1 : 0, 0);
        }

        private Color GetColorInterpolated(TileInfo tileInfo)
        {
            return new Color(tileInfo.ValueInterpolated, 0, 0);
        }

        private TileInfo GetNewTileInfo()
        {
            return new TileInfo(value: initialFogValue, valueInterpolated: initialFogValue);
        }

        /// <summary>
        /// Class that holds information about a single tile in the vision grid.
        /// </summary>
        public class TileInfo
        {
            /// <summary>
            /// Fog value of the tile. 0 means completely covered in fog, 1 means completely visible. Values in (0,1) represent a partially covered tile.
            /// </summary>
            public float Value { get; }

            /// <summary>
            /// Same as Value but used in the interpolated texture.
            /// </summary>
            public float ValueInterpolated { get; }

            /// <summary>
            /// Whether the shader should sample from the interpolated texture or not.
            /// </summary>
            public bool IsInterpolated { get; }

            /// <summary>
            /// Whether the tile is at least partially revealed or completely hidden.
            /// </summary>
            public bool IsRevealed { get; }

            public TileInfo(float value = 0, float valueInterpolated = 0, bool isInterpolated = false, bool isRevealed = false)
            {
                Value = value;
                ValueInterpolated = valueInterpolated;
                IsInterpolated = isInterpolated;
                IsRevealed = isRevealed;
            }
        }

        /// <summary>
        /// Class that holds information about textures that are used in the Fog of War shader.
        /// </summary>
        public class VisionTextures
        {
            public Texture2D Texture;

            public Texture2D TextureInterpolated;

            public Vector2Int Offset;
        }
    }
}
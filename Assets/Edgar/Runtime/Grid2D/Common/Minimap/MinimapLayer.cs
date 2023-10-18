using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity
{
    /// <summary>
    /// Represents a single minimap layer which can consist of one or more tilemap layers.
    /// </summary>
    [Serializable]
    public class MinimapLayer
    {
        /// <summary>
        /// Name(s) of one or more tilemap layers. Multiple tilemap layers should be delimited by a comma.
        /// </summary>
        public string Tilemaps;

        /// <summary>
        /// How should the layer be displayed.
        /// </summary>
        public MinimapDisplayMode DisplayMode;

        /// <summary>
        /// Color of the tiles.
        /// </summary>
        [ConditionalHide(nameof(IsColorsMode))]
        public Color Color;

        /// <summary>
        /// Size of the tiles.
        /// </summary>
        [ConditionalHide(nameof(IsColorsMode))]
        public float Size = 1;

        /// <summary>
        /// Custom tile.
        /// </summary>
        [ConditionalHide(nameof(IsCustomTiles))]
        public TileBase Tile;

        /// <summary>
        /// Material for the layer.
        /// </summary>
        public Material Material;

        private bool IsColorsMode => DisplayMode == MinimapDisplayMode.Color;

        private bool IsCustomTiles => DisplayMode == MinimapDisplayMode.CustomTile;
    }
}
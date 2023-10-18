namespace Edgar.Unity
{
    /// <summary>
    /// Controls how a given minimap layer will be displayed in the game.
    /// </summary>
    public enum MinimapDisplayMode
    {
        /// <summary>
        /// A single-colored tile will be used to display the layer.
        /// </summary>
        Color = 0,

        /// <summary>
        /// A custom tile will be used to display the layer.
        /// </summary>
        CustomTile = 1,

        /// <summary>
        /// Original tiles will be used to display the layer.
        /// </summary>
        OriginalTiles = 2,
    }
}
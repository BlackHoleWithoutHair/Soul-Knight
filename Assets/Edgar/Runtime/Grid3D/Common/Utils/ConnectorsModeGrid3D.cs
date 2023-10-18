namespace Edgar.Unity
{
    /// <summary>
    /// Controls how to handle room template connectors.
    /// </summary>
    public enum ConnectorsModeGrid3D
    {
        /// <summary>
        /// Connectors are never added.
        /// </summary>
        Never = 0,

        /// <summary>
        /// Only room connectors are added.
        /// </summary>
        RoomsOnly = 1,

        /// <summary>
        /// Only corridor connectors are added.
        /// </summary>
        CorridorsOnly = 2,

        /// <summary>
        /// Both room and corridor connectors are added.
        /// </summary>
        RoomsAndCorridors = 3,

        /// <summary>
        /// Prefers to use corridors for connectors but if there is no corridor, uses room connectors.
        /// </summary>
        PreferCorridors = 4,
    }
}
namespace Edgar.Unity
{
    /// <summary>
    /// Mode of Fog of War.
    /// </summary>
    public enum FogOfWarMode
    {
        /// <summary>
        /// The fog moves as a wave. The tiles that are closest to the player are revealed first.
        /// </summary>
        Wave = 0,

        /// <summary>
        /// All tiles fade in uniformly.
        /// </summary>
        FadeIn = 1,
    }
}
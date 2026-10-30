namespace KDisplayMode
{
    /// <summary>
    /// Platform-independent window mode shown to players.
    /// Mapped to <see cref="UnityEngine.FullScreenMode"/> per platform by <see cref="FullScreenModeMapper"/>.
    /// </summary>
    /// <remarks>Values are fixed so they can be stored as integers in save data.</remarks>
    public enum WindowMode
    {
        /// <summary>Exclusive fullscreen on Windows, maximized (menu bar and Dock hidden) on macOS.</summary>
        FullScreen = 0,

        /// <summary>Borderless window covering the whole display.</summary>
        Borderless = 1,

        /// <summary>Standard movable window.</summary>
        Windowed = 2,
    }
}

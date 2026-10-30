using System;
using UnityEngine;

namespace KDisplayMode
{
    /// <summary>Converts between <see cref="WindowMode"/> and Unity's <see cref="FullScreenMode"/> for a platform.</summary>
    public static class FullScreenModeMapper
    {
        /// <summary>
        /// FullScreen: ExclusiveFullScreen on Windows, MaximizedWindow on macOS, FullScreenWindow elsewhere.
        /// Borderless: FullScreenWindow. Windowed: Windowed.
        /// </summary>
        public static FullScreenMode ToFullScreenMode(WindowMode mode, RuntimePlatform platform) => mode switch
        {
            WindowMode.FullScreen => ToDesktopOS(platform) switch
            {
                DesktopOS.Windows => FullScreenMode.ExclusiveFullScreen,
                DesktopOS.MacOS => FullScreenMode.MaximizedWindow,
                _ => FullScreenMode.FullScreenWindow,
            },
            WindowMode.Borderless => FullScreenMode.FullScreenWindow,
            WindowMode.Windowed => FullScreenMode.Windowed,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

        /// <summary>
        /// Inverse of <see cref="ToFullScreenMode"/>. MaximizedWindow on Windows is a maximized window with a title bar,
        /// so it is reported as Windowed there.
        /// </summary>
        public static WindowMode ToWindowMode(FullScreenMode mode, RuntimePlatform platform) => mode switch
        {
            FullScreenMode.ExclusiveFullScreen => WindowMode.FullScreen,
            FullScreenMode.FullScreenWindow => WindowMode.Borderless,
            FullScreenMode.MaximizedWindow => ToDesktopOS(platform) == DesktopOS.MacOS
                ? WindowMode.FullScreen
                : WindowMode.Windowed,
            FullScreenMode.Windowed => WindowMode.Windowed,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

        private enum DesktopOS
        {
            Other,
            Windows,
            MacOS,
        }

        private static DesktopOS ToDesktopOS(RuntimePlatform platform) => platform switch
        {
            RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor => DesktopOS.Windows,
            RuntimePlatform.OSXPlayer or RuntimePlatform.OSXEditor => DesktopOS.MacOS,
            _ => DesktopOS.Other,
        };
    }
}

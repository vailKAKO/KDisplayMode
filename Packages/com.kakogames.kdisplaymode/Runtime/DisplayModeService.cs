using System.Collections.Generic;
using UnityEngine;

namespace KDisplayMode
{
    /// <summary>
    /// <see cref="IDisplayModeService"/> backed by <see cref="Screen"/>.
    /// Holds no state, so creating several instances is fine; register one as a singleton if you use a DI container.
    /// </summary>
    public sealed class DisplayModeService : IDisplayModeService
    {
        private readonly RuntimePlatform platform;

        public DisplayModeService() : this(Application.platform)
        {
        }

        /// <param name="platform">The platform whose FullScreenMode mapping is used.</param>
        public DisplayModeService(RuntimePlatform platform)
        {
            this.platform = platform;
        }

        public DisplaySettings Current => new(
            Screen.width,
            Screen.height,
            FullScreenModeMapper.ToWindowMode(Screen.fullScreenMode, platform));

        public void GetAvailableResolutions(List<DisplayResolution> results) =>
            DisplayResolutionList.Collect(Screen.resolutions, results);

        public void Apply(DisplaySettings settings) =>
            Screen.SetResolution(
                settings.Width,
                settings.Height,
                FullScreenModeMapper.ToFullScreenMode(settings.WindowMode, platform));
    }
}

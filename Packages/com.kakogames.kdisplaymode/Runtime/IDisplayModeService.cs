using System.Collections.Generic;

namespace KDisplayMode
{
    /// <summary>Reads and applies the resolution and window mode of the player window.</summary>
    public interface IDisplayModeService
    {
        /// <summary>
        /// The settings in effect now. After <see cref="Apply"/>, this keeps returning the previous value
        /// until the next frame, because Unity applies the change at the end of the frame.
        /// </summary>
        DisplaySettings Current { get; }

        /// <summary>
        /// Clears <paramref name="results"/> and fills it with the resolutions the current display supports, largest first.
        /// </summary>
        void GetAvailableResolutions(List<DisplayResolution> results);

        /// <summary>
        /// Requests the resolution and window mode. Unity applies it at the end of the current frame.
        /// In the Editor, only the Game view is affected.
        /// </summary>
        void Apply(DisplaySettings settings);
    }
}

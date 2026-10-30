using System;
using UnityEngine;

namespace KDisplayMode
{
    /// <summary>
    /// Resolution and window mode to apply. This package never saves it;
    /// store it however your project saves data (JsonUtility, PlayerPrefs, your own save file, ...).
    /// </summary>
    [Serializable]
    public struct DisplaySettings : IEquatable<DisplaySettings>
    {
        // Private fields with [SerializeField] instead of public fields: JsonUtility reads the fields,
        // other serializers (e.g. Newtonsoft) use the properties and the constructor whose parameter names match.
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private WindowMode windowMode;

        public int Width => width;
        public int Height => height;
        public WindowMode WindowMode => windowMode;
        public DisplayResolution Resolution => new(width, height);

        /// <exception cref="ArgumentOutOfRangeException">width or height is less than 1.</exception>
        public DisplaySettings(int width, int height, WindowMode windowMode)
        {
            if (width < 1) throw new ArgumentOutOfRangeException(nameof(width), width, "Width must be 1 or greater.");
            if (height < 1) throw new ArgumentOutOfRangeException(nameof(height), height, "Height must be 1 or greater.");

            this.width = width;
            this.height = height;
            this.windowMode = windowMode;
        }

        public DisplaySettings(DisplayResolution resolution, WindowMode windowMode)
            : this(resolution.Width, resolution.Height, windowMode)
        {
        }

        public DisplaySettings WithResolution(DisplayResolution resolution) => new(resolution, windowMode);
        public DisplaySettings WithWindowMode(WindowMode mode) => new(width, height, mode);

        public bool Equals(DisplaySettings other) =>
            width == other.width && height == other.height && windowMode == other.windowMode;

        public override bool Equals(object obj) => obj is DisplaySettings other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(width, height, (int)windowMode);
        public override string ToString() => $"{width}x{height} {windowMode}";

        public static bool operator ==(DisplaySettings left, DisplaySettings right) => left.Equals(right);
        public static bool operator !=(DisplaySettings left, DisplaySettings right) => !left.Equals(right);
    }
}

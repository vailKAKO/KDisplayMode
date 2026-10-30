using System;

namespace KDisplayMode
{
    /// <summary>A width and height in pixels.</summary>
    public readonly struct DisplayResolution : IEquatable<DisplayResolution>
    {
        public int Width { get; }
        public int Height { get; }

        public DisplayResolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(DisplayResolution other) => Width == other.Width && Height == other.Height;
        public override bool Equals(object obj) => obj is DisplayResolution other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Width, Height);
        public override string ToString() => $"{Width}x{Height}";

        public static bool operator ==(DisplayResolution left, DisplayResolution right) => left.Equals(right);
        public static bool operator !=(DisplayResolution left, DisplayResolution right) => !left.Equals(right);
    }
}

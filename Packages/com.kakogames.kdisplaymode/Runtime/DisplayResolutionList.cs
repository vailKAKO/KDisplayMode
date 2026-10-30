using System;
using System.Collections.Generic;
using UnityEngine;

namespace KDisplayMode
{
    /// <summary>Builds a resolution list for a settings menu from <see cref="Screen.resolutions"/>.</summary>
    public static class DisplayResolutionList
    {
        /// <summary>
        /// Clears <paramref name="results"/>, then fills it with the distinct sizes in <paramref name="source"/>
        /// (entries that differ only in refresh rate are merged), largest first.
        /// </summary>
        public static void Collect(ReadOnlySpan<Resolution> source, List<DisplayResolution> results)
        {
            results.Clear();
            foreach (var resolution in source)
            {
                var size = new DisplayResolution(resolution.width, resolution.height);
                if (!results.Contains(size)) results.Add(size);
            }
            results.Sort(CompareLargestFirst);
        }

        private static int CompareLargestFirst(DisplayResolution a, DisplayResolution b)
        {
            var byWidth = b.Width.CompareTo(a.Width);
            return byWidth != 0 ? byWidth : b.Height.CompareTo(a.Height);
        }
    }
}

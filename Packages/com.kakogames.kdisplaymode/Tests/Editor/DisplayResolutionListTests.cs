using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace KDisplayMode.Tests
{
    public class DisplayResolutionListTests
    {
        private static Resolution Make(int width, int height, uint refreshRate) => new()
        {
            width = width,
            height = height,
            refreshRateRatio = new RefreshRate { numerator = refreshRate, denominator = 1 },
        };

        [Test]
        public void MergesEntriesThatDifferOnlyInRefreshRate()
        {
            var source = new[] { Make(1920, 1080, 60), Make(1920, 1080, 144), Make(1280, 720, 60) };
            var results = new List<DisplayResolution>();

            DisplayResolutionList.Collect(source, results);

            Assert.That(results, Is.EqualTo(new[] { new DisplayResolution(1920, 1080), new DisplayResolution(1280, 720) }));
        }

        [Test]
        public void SortsLargestWidthFirst_ThenLargestHeight()
        {
            var source = new[] { Make(1280, 720, 60), Make(1920, 1200, 60), Make(1920, 1080, 60), Make(2560, 1440, 60) };
            var results = new List<DisplayResolution>();

            DisplayResolutionList.Collect(source, results);

            Assert.That(results, Is.EqualTo(new[]
            {
                new DisplayResolution(2560, 1440),
                new DisplayResolution(1920, 1200),
                new DisplayResolution(1920, 1080),
                new DisplayResolution(1280, 720),
            }));
        }

        [Test]
        public void ReplacesPreviousContentsOfResults()
        {
            var results = new List<DisplayResolution> { new(640, 480) };

            DisplayResolutionList.Collect(new[] { Make(1920, 1080, 60) }, results);

            Assert.That(results, Is.EqualTo(new[] { new DisplayResolution(1920, 1080) }));
        }
    }
}

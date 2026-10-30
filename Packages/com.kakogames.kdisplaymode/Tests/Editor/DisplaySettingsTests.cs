using System;
using NUnit.Framework;
using UnityEngine;

namespace KDisplayMode.Tests
{
    public class DisplaySettingsTests
    {
        [TestCase(0, 1080)]
        [TestCase(1920, 0)]
        [TestCase(-1, 1080)]
        public void Constructor_RejectsSizeBelowOne(int width, int height)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new DisplaySettings(width, height, WindowMode.Windowed));
        }

        [Test]
        public void SurvivesJsonUtilityRoundTrip_SoProjectsCanSaveItAsIs()
        {
            var original = new DisplaySettings(2560, 1440, WindowMode.Borderless);

            var restored = JsonUtility.FromJson<DisplaySettings>(JsonUtility.ToJson(original));

            Assert.That(restored, Is.EqualTo(original));
        }

        [Test]
        public void JsonUtility_WritesReadableFieldNames()
        {
            var json = JsonUtility.ToJson(new DisplaySettings(1280, 720, WindowMode.Windowed));

            Assert.That(json, Is.EqualTo("{\"width\":1280,\"height\":720,\"windowMode\":2}"));
        }

        [Test]
        public void WithResolution_KeepsWindowMode()
        {
            var settings = new DisplaySettings(1920, 1080, WindowMode.FullScreen)
                .WithResolution(new DisplayResolution(1280, 720));

            Assert.That(settings, Is.EqualTo(new DisplaySettings(1280, 720, WindowMode.FullScreen)));
        }

        [Test]
        public void WithWindowMode_KeepsResolution()
        {
            var settings = new DisplaySettings(1920, 1080, WindowMode.FullScreen)
                .WithWindowMode(WindowMode.Windowed);

            Assert.That(settings, Is.EqualTo(new DisplaySettings(1920, 1080, WindowMode.Windowed)));
        }

        [Test]
        public void Resolution_ExposesWidthAndHeight()
        {
            Assert.That(new DisplaySettings(1920, 1080, WindowMode.Borderless).Resolution,
                Is.EqualTo(new DisplayResolution(1920, 1080)));
        }

        [Test]
        public void SettingsWithDifferentWindowMode_AreNotEqual()
        {
            Assert.That(new DisplaySettings(1920, 1080, WindowMode.Borderless),
                Is.Not.EqualTo(new DisplaySettings(1920, 1080, WindowMode.Windowed)));
        }
    }
}

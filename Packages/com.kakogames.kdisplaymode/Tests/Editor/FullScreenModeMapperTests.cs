using System;
using NUnit.Framework;
using UnityEngine;

namespace KDisplayMode.Tests
{
    public class FullScreenModeMapperTests
    {
        [TestCase(RuntimePlatform.WindowsPlayer, FullScreenMode.ExclusiveFullScreen)]
        [TestCase(RuntimePlatform.WindowsEditor, FullScreenMode.ExclusiveFullScreen)]
        [TestCase(RuntimePlatform.OSXPlayer, FullScreenMode.MaximizedWindow)]
        [TestCase(RuntimePlatform.OSXEditor, FullScreenMode.MaximizedWindow)]
        [TestCase(RuntimePlatform.LinuxPlayer, FullScreenMode.FullScreenWindow)]
        [TestCase(RuntimePlatform.LinuxEditor, FullScreenMode.FullScreenWindow)]
        public void FullScreen_UsesEachPlatformsTrueFullscreen(RuntimePlatform platform, FullScreenMode expected)
        {
            Assert.That(FullScreenModeMapper.ToFullScreenMode(WindowMode.FullScreen, platform), Is.EqualTo(expected));
        }

        [TestCase(RuntimePlatform.WindowsPlayer)]
        [TestCase(RuntimePlatform.OSXPlayer)]
        [TestCase(RuntimePlatform.LinuxPlayer)]
        public void Borderless_IsFullScreenWindowOnEveryPlatform(RuntimePlatform platform)
        {
            Assert.That(FullScreenModeMapper.ToFullScreenMode(WindowMode.Borderless, platform),
                Is.EqualTo(FullScreenMode.FullScreenWindow));
        }

        [TestCase(RuntimePlatform.WindowsPlayer)]
        [TestCase(RuntimePlatform.OSXPlayer)]
        [TestCase(RuntimePlatform.LinuxPlayer)]
        public void Windowed_IsWindowedOnEveryPlatform(RuntimePlatform platform)
        {
            Assert.That(FullScreenModeMapper.ToFullScreenMode(WindowMode.Windowed, platform),
                Is.EqualTo(FullScreenMode.Windowed));
        }

        [TestCase(RuntimePlatform.WindowsPlayer)]
        [TestCase(RuntimePlatform.OSXPlayer)]
        public void OnWindowsAndMacOS_EveryWindowModeRoundTripsThroughFullScreenMode(RuntimePlatform platform)
        {
            foreach (WindowMode mode in Enum.GetValues(typeof(WindowMode)))
            {
                var fullScreenMode = FullScreenModeMapper.ToFullScreenMode(mode, platform);
                Assert.That(FullScreenModeMapper.ToWindowMode(fullScreenMode, platform), Is.EqualTo(mode), $"{mode} on {platform}");
            }
        }

        [Test]
        public void OnOtherPlatforms_FullScreenIsReportedAsBorderless_BecauseBothUseFullScreenWindow()
        {
            var fullScreenMode = FullScreenModeMapper.ToFullScreenMode(WindowMode.FullScreen, RuntimePlatform.LinuxPlayer);

            Assert.That(FullScreenModeMapper.ToWindowMode(fullScreenMode, RuntimePlatform.LinuxPlayer),
                Is.EqualTo(WindowMode.Borderless));
        }

        [Test]
        public void MaximizedWindowOnWindows_IsReportedAsWindowed_BecauseItKeepsTheTitleBar()
        {
            Assert.That(FullScreenModeMapper.ToWindowMode(FullScreenMode.MaximizedWindow, RuntimePlatform.WindowsPlayer),
                Is.EqualTo(WindowMode.Windowed));
        }

        [Test]
        public void MaximizedWindowOnMacOS_IsReportedAsFullScreen()
        {
            Assert.That(FullScreenModeMapper.ToWindowMode(FullScreenMode.MaximizedWindow, RuntimePlatform.OSXPlayer),
                Is.EqualTo(WindowMode.FullScreen));
        }

        [Test]
        public void ExclusiveFullScreen_IsReportedAsFullScreen()
        {
            Assert.That(FullScreenModeMapper.ToWindowMode(FullScreenMode.ExclusiveFullScreen, RuntimePlatform.WindowsPlayer),
                Is.EqualTo(WindowMode.FullScreen));
        }
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KDisplayMode.Samples
{
    /// <summary>
    /// Settings panel with a resolution dropdown, a window mode dropdown and an Apply button.
    /// It does not save anything: subscribe to <see cref="Applied"/> to store the settings,
    /// and call <see cref="SetSettings"/> to show stored settings.
    /// </summary>
    public sealed class DisplaySettingsPanel : MonoBehaviour
    {
        private static readonly WindowMode[] WindowModes = { WindowMode.FullScreen, WindowMode.Borderless, WindowMode.Windowed };
        private static readonly List<string> WindowModeLabels = new() { "Fullscreen", "Borderless", "Windowed" };

        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown windowModeDropdown;
        [SerializeField] private Button applyButton;
        [SerializeField] private TMP_Text statusText;

        private readonly List<DisplayResolution> resolutions = new();
        private readonly List<string> resolutionLabels = new();
        private IDisplayModeService service;

        private int shownWidth = -1;
        private int shownHeight = -1;
        private FullScreenMode shownFullScreenMode;

        /// <summary>Raised after the Apply button requested new settings. Save them here if you want to keep them.</summary>
        public event Action<DisplaySettings> Applied;

        // Created on first use rather than in a field initializer: Unity APIs can't be called from a MonoBehaviour constructor.
        private IDisplayModeService Service => service ??= new DisplayModeService();

        private void Start()
        {
            applyButton.onClick.AddListener(Apply);

            if (resolutions.Count == 0) SetSettings(Service.Current);
        }

        private void OnDestroy()
        {
            applyButton.onClick.RemoveListener(Apply);
        }

        private void Update()
        {
            if (Screen.width == shownWidth && Screen.height == shownHeight && Screen.fullScreenMode == shownFullScreenMode) return;

            shownWidth = Screen.width;
            shownHeight = Screen.height;
            shownFullScreenMode = Screen.fullScreenMode;
            statusText.text =
                $"Screen: {shownWidth}x{shownHeight}\n" +
                $"FullScreenMode: {shownFullScreenMode}\n" +
                $"WindowMode: {Service.Current.WindowMode}\n" +
                $"Platform: {Application.platform}";
        }

        /// <summary>Selects <paramref name="settings"/> in the dropdowns without applying them.</summary>
        public void SetSettings(DisplaySettings settings)
        {
            Service.GetAvailableResolutions(resolutions);
            // A window resized by hand or a stored size from another display may not be in the list.
            if (!resolutions.Contains(settings.Resolution)) resolutions.Insert(0, settings.Resolution);

            resolutionLabels.Clear();
            foreach (var resolution in resolutions) resolutionLabels.Add(resolution.ToString());

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resolutionLabels);
            resolutionDropdown.SetValueWithoutNotify(resolutions.IndexOf(settings.Resolution));

            // Filled here, not in Start, so that SetSettings works even when called before Start.
            windowModeDropdown.ClearOptions();
            windowModeDropdown.AddOptions(WindowModeLabels);
            windowModeDropdown.SetValueWithoutNotify(Array.IndexOf(WindowModes, settings.WindowMode));
        }

        private void Apply()
        {
            var settings = new DisplaySettings(resolutions[resolutionDropdown.value], WindowModes[windowModeDropdown.value]);
            Service.Apply(settings);
            Applied?.Invoke(settings);
        }
    }
}

# KDisplayMode Demo

- `KDisplayModeDemo.unity` — open it and build a player to try resolution and window mode switching. In the Editor, only the Game view changes.
- `DisplaySettingsPanel.prefab` — drop it into any Canvas to get a working settings panel.
- `DisplaySettingsPanel.cs` — the panel script.
  - `Applied` (event): raised with the new `DisplaySettings` after Apply. Save them here.
  - `SetSettings(DisplaySettings)`: shows stored settings in the dropdowns without applying them.

```csharp
panel.Applied += settings =>
{
    PlayerPrefs.SetString("DisplaySettings", JsonUtility.ToJson(settings));
    PlayerPrefs.Save();
};
```

Requires uGUI, TextMesh Pro (Essential Resources) and the Input System package.

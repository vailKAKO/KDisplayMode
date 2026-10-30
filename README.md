# KDisplayMode

[![Release](https://img.shields.io/github/v/release/vailKAKO/KDisplayMode)](https://github.com/vailKAKO/KDisplayMode/releases)
[![Unity 2022.3+](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity)](https://unity.com/releases/editor/archive)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

English | [日本語](README.ja.md)

Resolution and window mode switching for Unity standalone players on Windows and macOS.
You choose from three modes — Fullscreen, Borderless and Windowed — and the package maps each one to the right `FullScreenMode` for the platform.

![Demo scene: resolution and window mode dropdowns](Docs/images/demo.png)

## Features

- Three window modes that behave consistently on Windows and macOS (see [Window modes by platform](#window-modes-by-platform))
- A resolution list for settings menus: entries from `Screen.resolutions` that differ only in refresh rate are merged, and the list is sorted largest first
- Read the current resolution and window mode, or apply new ones, through `IDisplayModeService`
- A plain C# service with no MonoBehaviour, singleton or static state, so it can be registered in VContainer or another DI container
- **No saving.** `DisplaySettings` is a serializable struct, so each project can save it however it likes (see [Saving settings](#saving-settings))
- Sample: a settings panel prefab and a demo scene built with uGUI and TextMesh Pro

## Requirements

- Unity 2022.3 or later
- Git client 2.14.0 or later (Package Manager uses it to fetch the package)
- Windows or macOS standalone player (on other platforms, Fullscreen falls back to `FullScreenWindow`)
- The Demo sample additionally needs uGUI, TextMesh Pro and the Input System package

## Installation

### Package Manager (git URL)

Open **Window > Package Manager**, choose **+ > Install package from git URL** (**Add package from git URL...** in Unity 2022.3), and enter:

```
https://github.com/vailKAKO/KDisplayMode.git?path=Packages/com.kakogames.kdisplaymode
```

To pin a version, append a tag:

```
https://github.com/vailKAKO/KDisplayMode.git?path=Packages/com.kakogames.kdisplaymode#v0.1.0
```

### manifest.json

Alternatively, add this line to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.kakogames.kdisplaymode": "https://github.com/vailKAKO/KDisplayMode.git?path=Packages/com.kakogames.kdisplaymode#v0.1.0"
  }
}
```

## Usage

```csharp
using System.Collections.Generic;
using KDisplayMode;

IDisplayModeService service = new DisplayModeService();

// Resolutions the current display supports, largest first.
var resolutions = new List<DisplayResolution>();
service.GetAvailableResolutions(resolutions);

// Apply a resolution and a window mode.
service.Apply(new DisplaySettings(1920, 1080, WindowMode.Borderless));

// Read the settings in effect now.
DisplaySettings current = service.Current;
```

Change only one of the two values:

```csharp
service.Apply(service.Current.WithWindowMode(WindowMode.Windowed));
service.Apply(service.Current.WithResolution(new DisplayResolution(1280, 720)));
```

With VContainer:

```csharp
builder.Register<IDisplayModeService, DisplayModeService>(Lifetime.Singleton);
```

`Apply` takes effect at the end of the current frame. Until the next frame, `Current` still returns the previous values.

## Saving settings

**This package never saves anything.** Saving is left to your project, since everyone has their own preferred way to save data.

`DisplaySettings` is a `[Serializable]` struct with three values: `Width`, `Height` and `WindowMode`. Store it however you like.

Using JsonUtility and PlayerPrefs:

```csharp
const string Key = "DisplaySettings";

// Save
PlayerPrefs.SetString(Key, JsonUtility.ToJson(settings));
PlayerPrefs.Save();

// Load and apply at startup
if (PlayerPrefs.HasKey(Key))
{
    service.Apply(JsonUtility.FromJson<DisplaySettings>(PlayerPrefs.GetString(Key)));
}
```

`JsonUtility.ToJson` produces `{"width":1920,"height":1080,"windowMode":1}`.

Keeping the values in your own save data:

```csharp
[Serializable]
public class OptionData
{
    public int screenWidth;
    public int screenHeight;
    public int windowMode; // (int)WindowMode
}

// Save
var current = service.Current;
data.screenWidth = current.Width;
data.screenHeight = current.Height;
data.windowMode = (int)current.WindowMode;

// Load and apply at startup
service.Apply(new DisplaySettings(data.screenWidth, data.screenHeight, (WindowMode)data.windowMode));
```

The integer values of `WindowMode` are fixed (`FullScreen = 0`, `Borderless = 1`, `Windowed = 2`), so storing them as integers is safe.

Typical flow:

1. At startup, load the stored settings and call `Apply`
2. When the player changes the settings, call `Apply` and then save
3. If nothing is stored, leave the startup values from Player Settings as they are

## Window modes by platform

| `WindowMode` | Windows | macOS | Other platforms |
|---|---|---|---|
| `FullScreen` | `ExclusiveFullScreen` | `MaximizedWindow` (fullscreen with the menu bar and Dock hidden) | `FullScreenWindow` |
| `Borderless` | `FullScreenWindow` | `FullScreenWindow` | `FullScreenWindow` |
| `Windowed` | `Windowed` | `Windowed` | `Windowed` |

`Current` converts back the other way. On Windows, `MaximizedWindow` is a maximized window with a title bar, so it is reported as `Windowed`. On other platforms, `FullScreen` and `Borderless` both use `FullScreenWindow`, so `Current` reports `Borderless`.

`FullScreenModeMapper` exposes this mapping if you need it directly.

## Notes

- A resolution change is applied at the end of the frame, not immediately
- In the Editor, only the Game view resolution changes. Check the actual behaviour in a built player
- With multiple displays, only the primary display's resolution can be set
- Exclusive fullscreen is available only on Windows
- Refresh rate is not handled in this version. Unity can change the refresh rate only in exclusive fullscreen
- If **Resizable Window** is turned off in Player Settings, `Windowed` cannot be used
- In `Borderless` (and in `FullScreen` on macOS), Unity renders at the chosen resolution and scales it to the display, adding black bars when the aspect ratio differs

## Sample

Open **Window > Package Manager**, select KDisplayMode, and import **Demo** from the **Samples** tab.

- `KDisplayModeDemo.unity`: a scene for trying the resolution and window mode switching. Build a player to see the real behaviour
- `DisplaySettingsPanel.prefab`: drop it into any Canvas to get a working settings panel
- `DisplaySettingsPanel.cs`: subscribe to its `Applied` event to save the settings, and call `SetSettings` to show stored settings in the dropdowns

If the project does not have TextMesh Pro Essential Resources yet, Unity asks to import them when the sample opens. The scene's EventSystem uses `InputSystemUIInputModule`; replace it if your project uses a different input module.

## License

[MIT License](LICENSE) © 2026 Kako no Nikomi

The MIT License covers the package (`Packages/com.kakogames.kdisplaymode`) and the other files written for this repository.

## Third-party notices

The development project in this repository also contains files generated by Unity, which are not covered by the MIT License above:

- `Assets/Settings` (Universal Render Pipeline settings), `Assets/InputSystem_Actions.inputactions` and `ProjectSettings` were created from a Unity project template and are subject to the [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license)
- TextMesh Pro Essential Resources are not included. To open the development project, import them with **Window > TextMeshPro > Import TMP Essential Resources**

None of these files are part of the package that Package Manager installs.

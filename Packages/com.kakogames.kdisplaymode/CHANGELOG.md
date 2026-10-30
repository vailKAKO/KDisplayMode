# Changelog

All notable changes to this package are documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2026-10-30

### Added

- `WindowMode` (FullScreen / Borderless / Windowed) and `FullScreenModeMapper` that maps it to `FullScreenMode` per platform
- `DisplaySettings`, a serializable struct holding width, height and window mode
- `DisplayResolution` and `DisplayResolutionList` for building a resolution list from `Screen.resolutions`
- `IDisplayModeService` / `DisplayModeService` for reading and applying settings
- Demo sample: settings panel prefab and demo scene (uGUI + TextMesh Pro)

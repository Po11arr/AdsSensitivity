# ADS Sensitivity

Space Engineers client plugin for [Pulsar](https://github.com/SpaceGT/Pulsar): FOV-based aim-down-sights sensitivity, per-weapon zoom and smooth zoom.

## Features

- **FOV-based sensitivity.** Vanilla slows ironsight look speed by a flat 0.6x whatever your FOV. This plugin uses monitor distance matching instead: `atan(c * tan(zoomFov/2)) / atan(c * tan(hipFov/2))`, with `c` = 1.33 by default (the CoD / Battlefield default).
- **Per-weapon zoom.** One magnification per hand weapon, relative to your FOV. The weapon list comes from the loaded definitions, so modded guns appear too. Starting values: pistols 1.5x, launchers 1.75x, MR-8P 3x, everything else 2x.
- **Smooth zoom.** Eased zoom in and out, 0.15 s by default, instead of vanilla's linear 0.075 s.

Client-side only. Works on any server with no server install.

## Settings

Pulsar plugin list > ADS Sensitivity > Settings.

| Option | Default | Range |
|---|---|---|
| FOV-based sensitivity | on | off = vanilla 0.6x |
| Monitor distance coefficient | 1.33 | 0 (zoom ratio) to 2 |
| ADS multiplier | 1.00 | 0.1 to 2 |
| Smooth zoom | on | off = vanilla linear |
| Zoom time (s) | 0.15 | 0.05 to 0.4 |
| Per-weapon zoom | on | off = vanilla fixed 40 degrees |
| Default zoom (x) | 2.00 | 1 to 6 |
| One row per weapon (x) | starting value or default | 1 to 6 |

Saved to `%AppData%\SpaceEngineers\Storage\AdsSensitivity.cfg`.

## How it works

- Prefix on `MyCharacter.MoveAndRotate(Vector3, Vector2, float)`: while the local character is in ironsight, the rotation input is scaled by `target / ROTATION_SPEED_IRONSIGHTS` (read from the game, 0.6 fallback). Vanilla's own multiplier and the aim assist multiplier still apply.
- Prefix and postfix on `MyCameraZoomProperties.Update`: set the transition time and recompute the zoomed FOV in tan space, so magnification changes evenly. Sensitivity reads the resulting FOV, so it follows each weapon's zoom.
- The server receives the resulting orientation, not the raw input, so the plugin is multiplayer safe without a server side.

## Bug reports

Open an issue on this repository with your Pulsar `info.log` (`%AppData%\Pulsar\Legacy\info.log`) and your game log.

## Development

Built on the [CometWorks client plugin template](https://github.com/CometWorks/client-plugin-template). See `AGENTS.md` and the template README for build, deploy and debug setup.

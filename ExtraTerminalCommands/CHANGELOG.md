## V1.6.4

## Additions:

- Commands are now dynamically added/removed fully when joining a lobby
  - Commands will no longer display in the `Extra` command menu if disabled
  - Commands that are enabled by the host will automatically be enabled for clients even if they had it disabled before

## V1.6.3

### Bug Fixes:

- Reset Config to non-capitalized group names. I didn't think about it not carrying over properly.

# V1.6.2

## Additions:

- You can now customize aliases to nearly all commands via configuration
  - These aliases are client-side and won't be synced
- Horn is able to be cancelled by running it again, but only for the person who ran the command to start it.

## Bug Fixes:

- Prevent `random` from throwing an error if you repeatedly try to run it while it is still travelling.

# V1.6.1

## Bug Fixes:

- Prevent the aliased keywords from showing on the `other` commands menu.

# V1.6.0

## Additions:

- TP [player name] Command
- Flash Command
- Ping Command

## Removals:

- A ton of aliases
  - Commands, Extra Commands, Extra-Commands, EC
  - R, R W
  - Go
  - Teleport
  - ITeleport
  - InverseTeleport
  - Inverse Teleport
  - Inverse-Teleport
  - Light
  - Song, Intro Song, IntroSong, Intro-Song, GreatAsset, Great Asset, GA
  - CL

---

# Original Version

# V1.5.0

## Additions:

- Horn command (works with custom input)

## Bug fixes:

- Fixed Extra command mismatch (in the extra menu it used to show "random [money], whilst money is not a filter)

## V1.4.1

### Bug Fixes:

- No longer says "sw/s [player]" is an invalid command
- Random command now takes the last moon, previously didn't

## V1.4.0

### Additions:

- 's' command, does the same as the vanilla 'switch' command. Includes player

## V1.3.5

### Bug Fixes:

- Weather Filter condition broken

## V1.3.4

### Bug Fixes:

- Launch breaks first round if not owner

## V1.3.3

### Changes:

- New logo (@Narboic)

### Bug Fixes:

- Launch breaks if you're not the owner on the first round
- Random command now works when orbiting the company building

## V1.3.0

### Additions:

- Clear Command (@Narboic)
- Better Networking (example: copy host settings)

### Bug Fixes:

- All clients now copy host setting! (@Sebiann)
- Can now write random when in orbit around company building (@Narobic)
- Disabling the intro song stops the counter (@Sebiann)
- No longer crashes/bugs out when loading into a game for the second time (@Narboic)

## V1.2.0

### Additions:

- Random Command
- Random filter with weather command

### Bug Fixes:

- Intro song now plays for every player with the mod
- Intro song can only be played once

## V1.1.0

### Additions:

- Extra command & Config

### Bug Fixes:

- Intro song now comes out of speakers instead of terminal
- Doors now say "opened" and "closed" correctly, instead of having it swapped.

## V1.0.0

Initial release

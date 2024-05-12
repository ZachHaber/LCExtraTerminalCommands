
A continuation of https://github.com/Beauver/ExtraTerminalCommands.

This mod adds all kinds of commands to the terminal, each of these commands can be disabled in the config file.
- The new commands Ping/Flash and TP [Player] will be added to config/syncing in the next version.

If you encounter any bugs you can report them on the project github: 

# Commands
## Extra Commands
Shows the list of all commands that are enabled.

Aliases: Extra
## Random planet
This will send you to a random planet, you can specify these filters (Weather), more might be added in the future.

Aliases: Random, Random Weather
## Launch Ship
Pulls the launch lever to land or launch the ship.

Aliases: Launch, Start
## Teleporter
Activates the teleporter, respects teleporting cooldown. Include a player name to teleport a particular player.

Note: due to how the game works, TP [Player] switches the radar target to that player, then teleports them, then switches back.
This involves specific timeout delays that may or may not be enough depending on connection times.

Aliases: TP, TP [player]
## Inverse Teleporter
Activates the inverse teleporter, respects teleporting cooldown.

Aliases: ITP
## Flash Radar Booster
Flashes whatever radar booster is currently active on the radar, so you don't have to specify a name. 

Aliases: Flash
## Ping Radar Booster
Pings whatever radar booster is currently active on the radar, so you don't have to specify a name. 

Aliases: Ping
## Doors
Opens or closes the ship doors depending on its current state.

Aliases: Doors, Door, D
## Lights
Turns the lights on or off depending on its current state.

Aliases: Lights
## Horn [Time (in seconds)]
Sounds the horn for the specified amount of time, if ran without a time it will default to config default.
This can be a fractional time (e.g. 1.5).

Aliases: Horn, Horn [seconds]
## Intro Song
Plays the intro song (great great asset to the company 💃), it plays for everyone.

Aliases: Intro
## Time
Shows the current time it is on the moon.

Aliases: Time
## Switch
Switches the monitor view to the next person

Due to weird behavior, `S` doesn't allow for switching to specific players with how I have it set up.

Aliases: S, SW, SW [player]
## Clear
Clears the terminal commandline.

Aliases: Clear, CLS

# Planned (Maybe):
- Randomize moon command (with money filter)

# Credits
- Beauver - Who created the original version
- Lordfirespeed (ty for the .csproj and stuff 😭😭😭)
- Narobic (Thanks for the logo, much love!)

# Versions
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

## V1.5.0

### Additions:
- Horn command (works with custom input)


### Bug fixes:
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

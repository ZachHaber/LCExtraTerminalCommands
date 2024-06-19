using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;

namespace ExtraTerminalCommands
{
    public class Config
    {
        public static ConfigEntry<bool> configExtraCommandsList;
        public static ConfigEntry<bool> configLaunchCommand;
        public static ConfigEntry<bool> configAllowLaunchOnMoon;
        public static ConfigEntry<bool> configTimeCommand;
        public static ConfigEntry<bool> configTeleportCommand;
        public static ConfigEntry<bool> configTeleportPlayerCommand;
        public static ConfigEntry<bool> configInverseTeleportCommand;
        public static ConfigEntry<bool> configLightsCommand;
        public static ConfigEntry<bool> configDoorsCommand;
        public static ConfigEntry<bool> configIntroSongCommand;
        public static ConfigEntry<bool> configRandomMoonCommand;
        public static ConfigEntry<bool> configClearCommand;
        public static ConfigEntry<bool> configSwitchCommand;
        public static ConfigEntry<bool> configHornCommand;
        public static ConfigEntry<bool> configFlashCommand;
        public static ConfigEntry<bool> configPingCommand;
        public static ConfigEntry<bool> configAllowRandomWeatherFilter;
        public static ConfigEntry<bool> configHidePlanet;
        public static ConfigEntry<int> configRandomCommandPrice;
        public static ConfigEntry<int> configHornDefaultseconds;
        public static ConfigEntry<int> configHornMaxSeconds;

        public static ArrayConfigEntry launchCommandAliases = new("Aliases", "LaunchCommandAliases", "start", "Aliases for the Launch Command. Comma separated list of aliases.\nNote: Aliases are client-side only\nDefault: start");
        public static ArrayConfigEntry timeCommandAliases = new("Aliases", "TimeCommandAliases", "", "Aliases for the Time Command. Comma separated list of aliases");
        public static ArrayConfigEntry inverseTeleportCommandAliases = new("Aliases", "InverseTeleportCommandAliases", "", "Aliases for the InverseTeleport Command. Comma separated list of aliases");
        public static ArrayConfigEntry teleportCommandAliases = new("Aliases", "TeleportCommandAliases", "", "Aliases for the teleport Command. Comma separated list of aliases. Must be a single word with more than 1 character");
        public static ArrayConfigEntry lightsCommandAliases = new("Aliases", "LightsCommandAliases", "light", "Aliases for the Lights Command. Comma separated list of aliases\nDefault: light");
        public static ArrayConfigEntry doorsCommandAliases = new("Aliases", "DoorsCommandAliases", "door,d", "Aliases for the Doors Command. Comma separated list of aliases\nDefault: door,d");
        public static ArrayConfigEntry introSongCommandAliases = new("Aliases", "IntroSongCommandAliases", "", "Aliases for the IntroSong Command. Comma separated list of aliases");
        public static ArrayConfigEntry clearCommandAliases = new("Aliases", "ClearCommandAliases", "cls", "Aliases for the Clear Command. Comma separated list of aliases.\nDefault: cls");
        public static ArrayConfigEntry flashCommandAliases = new("Aliases", "FlashCommandAliases", "", "Aliases for the Flash Command. Comma separated list of aliases");
        public static ArrayConfigEntry pingCommandAliases = new("Aliases", "PingCommandAliases", "", "Aliases for the Ping Command. Comma separated list of aliases");
        public static ArrayConfigEntry hornCommandAliases = new("Aliases", "HornCommandAliases", "", "Aliases for the Horn Command. Comma separated list of aliases. Must be a single word with more than 1 character");
        public static ArrayConfigEntry switchCommandAliases = new("Aliases", "SwitchCommandAliases", "", "Aliases for the Switch Command. Comma separated list of aliases. Must be a single word with more than 1 character");

        public static void Bind()
        {
            ConfigFile config = ExtraTerminalCommandsBase.config;
            configExtraCommandsList = config.Bind("commands",
                                      "DisableCommandsList",
                                      false,
                                      "Disables the 'extra' command which shows the command list.");
            configLaunchCommand = config.Bind("commands",
                                         "DisableLaunch",
                                         false,
                                         "Disables the 'launch' command");
            configAllowLaunchOnMoon = config.Bind("launch",
                                         "AllowLaunch",
                                         true,
                                         "Allows the 'launch' command to be executed when on a moon. " +
                                         "\nIf this is set to false the 'launch' command only works in space.");
            configTimeCommand = config.Bind("commands",
                                         "DisableTime",
                                         false,
                                         "Disables the 'time' command");
            configTeleportCommand = config.Bind("commands",
                                         "DisableTeleport",
                                         false,
                                         "Disables the 'tp' command");
            configTeleportPlayerCommand = config.Bind("commands",
                                         "DisableTeleportPlayer",
                                         false,
                                         "Disables the 'tp [player]' command for teleporting a specific player instead of the current one." +
                                         "\nOnly works if the main teleport command is enabled");
            configInverseTeleportCommand = config.Bind("commands",
                                         "DisableInverseTeleport",
                                         false,
                                         "Disables the 'itp' command");
            configFlashCommand = config.Bind("commands",
                                         "DisableFlashCommand",
                                         false,
                                         "Disables the 'flash' command");
            configPingCommand = config.Bind("commands",
                                         "DisablePingCommand",
                                         false,
                                         "Disables the 'ping' command");
            configLightsCommand = config.Bind("commands",
                                         "DisableLights",
                                         false,
                                         "Disables the 'lights' command");
            configDoorsCommand = config.Bind("commands",
                                         "DisableDoors",
                                         false,
                                         "Disables the 'doors' command");
            configIntroSongCommand = config.Bind("commands",
                                         "DisableIntroSong",
                                         false,
                                         "Disables the 'intro' command which plays the intro song when run");
            configRandomMoonCommand = config.Bind("commands",
                                         "DisableRandomMoon",
                                         false,
                                         "Disables the 'random' command to go to a random moon.");
            configClearCommand = config.Bind("commands",
                                         "DisableClear",
                                         false,
                                         "Disables the 'clear' command which clears all lines in the console.");
            configSwitchCommand = config.Bind("commands",
                                         "DisableSwitch",
                                         false,
                                         "Disables the 'sw' command, which does the same as the vanilla 'switch' command. Except that it also allows specifying a player by name");
            configHornCommand = config.Bind("commands",
                                         "DisableHorn",
                                         false,
                                         "Disables the 'horn' command, which sounds the horn for X amount of seconds");

            configRandomCommandPrice = config.Bind("random",
                                         "RandomCommandPrice",
                                         100,
                                         "The price of the 'random' command. You will not receive a confirmation warning.");
            configAllowRandomWeatherFilter = config.Bind("random",
                                         "AllowWeatherFilter",
                                         true,
                                         "When enabled allows you to filter out weather when going to a random moon by typing 'random weather'");
            configHidePlanet = config.Bind("random",
                                         "AllowPlanetHide",
                                         true,
                                         "When enabled will not show what planet you're going to when writing 'random'");

            configHornDefaultseconds = config.Bind("horn",
                                         "SecondsEnabled",
                                         10,
                                         "This is the default amount of seconds the horn will continue to sound when running 'horn'");
            configHornMaxSeconds = config.Bind("horn",
                                         "MaxSeconds",
                                         30,
                                         "This is the maximum amount of seconds the horn can sound when running 'horn [time]' Be warned, the higher this number more lag may occur.");
            launchCommandAliases.Bind();
            timeCommandAliases.Bind();
            inverseTeleportCommandAliases.Bind();
            teleportCommandAliases.Bind();
            lightsCommandAliases.Bind();
            doorsCommandAliases.Bind();
            introSongCommandAliases.Bind();
            clearCommandAliases.Bind();
            flashCommandAliases.Bind();
            pingCommandAliases.Bind();
            hornCommandAliases.Bind();
            switchCommandAliases.Bind();
        }
    }
    public class ArrayConfigEntry
    {
        public string section;
        public string key;
        public string defaultValue;
        public string description;
        public ConfigEntry<string> config;
        public ArrayConfigEntry(string section, string key, string defaultValue, string description)
        {
            this.section = section;
            this.key = key;
            this.defaultValue = defaultValue;
            this.description = description;
        }
        public void Bind()
        {
            config = ExtraTerminalCommandsBase.config.Bind(section, key, defaultValue, description);
        }
        public List<string> Value
        {
            get => config.Value.Split(",", System.StringSplitOptions.RemoveEmptyEntries).Select((str) => str.Trim()).ToList();
            set
            {
                config.Value = string.Join(",", value);
            }
        }

    }
}

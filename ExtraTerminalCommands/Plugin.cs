using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
using ExtraTerminalCommands.Patches;
using ExtraTerminalCommands.TerminalCommands;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using TerminalApi.Classes;
using UnityEngine;

namespace ExtraTerminalCommands
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("atomic.terminalapi", MinimumDependencyVersion: "1.5.0")]
    public class ExtraTerminalCommandsBase : BaseUnityPlugin
    {
        public static ConfigEntry<bool> configExtraCommandsList;
        public static ConfigEntry<bool> configLaunchCommand;
        public static ConfigEntry<bool> configAllowLaunchOnMoon;
        public static ConfigEntry<bool> configTimeCommand;
        public static ConfigEntry<bool> configTeleportCommand;
        public static ConfigEntry<bool> configInverseTeleportCommand;
        public static ConfigEntry<bool> configLightsCommand;
        public static ConfigEntry<bool> configDoorsCommand;
        public static ConfigEntry<bool> configIntroSongCommand;
        public static ConfigEntry<bool> configRandomMoonCommand;
        public static ConfigEntry<bool> configClearCommand;
        public static ConfigEntry<bool> configSwitchCommand;
        public static ConfigEntry<bool> configHornCommand;

        public static ConfigEntry<bool> configAllowRandomWeatherFilter;
        public static ConfigEntry<bool> configHidePlanet;
        public static ConfigEntry<int> configRandomCommandPrice;
        public static ConfigEntry<int> configHornDefaultseconds;
        public static ConfigEntry<int> configHornMaxSeconds;

        public const string modGUID = MyPluginInfo.PLUGIN_GUID;
        public const string modName = MyPluginInfo.PLUGIN_NAME;
        public const string modVersion = MyPluginInfo.PLUGIN_VERSION;

        private readonly Harmony harmony = new Harmony(modGUID);

        public static AssetBundle MainAssetBundle;

        private static ExtraTerminalCommandsBase Instance;
        public static ManualLogSource mls;

        public static int daysJoined;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);


            LoadConfig();
            harmony.PatchAll();

            MainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "extraterminalcommandsnetwork"));
            NetcodePatcher();
            mls.LogInfo("Invoked NetcodePatcher");
            
            RegisterCommands();
            mls.LogInfo($"{modGUID} v{modVersion} has loaded!");
        }

        void RegisterCommands()
        {
            if (!configExtraCommandsList.Value) { ExtraCommands.extraCommands(); }
            if (!configLaunchCommand.Value) { LaunchCommand.launchCommand(); }
            if (!configDoorsCommand.Value) { DoorsCommand.doorsCommand(); }
            if (!configHornCommand.Value) { HornCommand.hornCommand(); }
            if (!configLightsCommand.Value) { LightsCommand.lightsCommand(); }
            if (!configTimeCommand.Value) { TimeCommand.timeCommand(); }
            if (!configSwitchCommand.Value) { SwitchCommand.switchCommand(); }
            if (!configTeleportCommand.Value) { TeleportCommand.teleportCommand(); }
            if (!configInverseTeleportCommand.Value) { InverseTeleportCommand.inverseTeleportCommand(); }
            RadarBoosterCommands.FlashCommand();
            RadarBoosterCommands.PingCommand();
            if (!configRandomMoonCommand.Value) { RandomMoonCommand.randomMoonCommand(); }
            if (!configClearCommand.Value) { ClearScreenCommand.clearScreenCommand(); }
            if (!configIntroSongCommand.Value) { IntroSongCommand introSongCommandClass = new IntroSongCommand(); introSongCommandClass.introSongCommand(); }
            daysJoined = 0;
        }

        private void LoadConfig()
        {
            configExtraCommandsList = Config.Bind("commands",
                                         "DisableCommandsList",
                                         false,
                                         "Disables the 'extra' command which shows the command list.");
            configLaunchCommand = Config.Bind("commands",
                                         "DisableLaunch",
                                         false,
                                         "Disables the 'launch' command in terminal");
            configAllowLaunchOnMoon = Config.Bind("launch",
                                         "AllowLaunch",
                                         true,
                                         "Allows the 'launch' command to be executed when on a moon. " +
                                         "\nIf this is set to false the 'launch' command only works in space.");
            configTimeCommand = Config.Bind("commands",
                                         "DisableTime",
                                         false,
                                         "Disables the 'time' command in terminal");
            configTeleportCommand = Config.Bind("commands",
                                         "DisableTeleport",
                                         false,
                                         "Disables the 'teleport' command in terminal");
            configInverseTeleportCommand = Config.Bind("commands",
                                         "DisableInverseTeleport",
                                         false,
                                         "Disables the 'itp' command in terminal");
            configLightsCommand = Config.Bind("commands",
                                         "DisableLights",
                                         false,
                                         "Disables the 'lights' command in terminal");
            configDoorsCommand = Config.Bind("commands",
                                         "DisableDoors",
                                         false,
                                         "Disables the 'doors' command in terminal");
            configIntroSongCommand = Config.Bind("commands",
                                         "DisableIntroSong",
                                         false,
                                         "Disables the 'intro' command which plays the intro song when run");
            configRandomMoonCommand = Config.Bind("commands",
                                         "DisableRandomMoon",
                                         false,
                                         "Disables the 'random' command to go to a random moon.");
            configClearCommand = Config.Bind("commands",
                                         "DisableClear",
                                         false,
                                         "Disables the 'clear' command which clears all lines in the console.");
            configSwitchCommand = Config.Bind("commands",
                                         "DisableSwitch",
                                         false,
                                         "Disables the 'sw' command, which does the same as the vanilla 'switch' command. Except that it also allows specifying a player by name");
            configHornCommand = Config.Bind("commands",
                                         "DisableHorn",
                                         false,
                                         "Disables the 'horn' command, which sounds the horn for X amount of seconds");

            configRandomCommandPrice = Config.Bind("random",
                                         "RandomCommandPrice",
                                         100,
                                         "The price of the 'random' command. You will not receive a confirmation warning.");
            configAllowRandomWeatherFilter = Config.Bind("random",
                                         "AllowWeatherFilter",
                                         true,
                                         "When enabled allows you to filter out weather when going to a random moon by typing 'random weather'");
            configHidePlanet = Config.Bind("random",
                                         "AllowPlanetHide",
                                         true,
                                         "When enabled will not show what planet you're going to when writing 'random'");

            configHornDefaultseconds = Config.Bind("horn",
                                         "SecondsEnabled",
                                         10,
                                         "This is the default amount of seconds the horn will continue to sound when running 'horn'");
            configHornMaxSeconds = Config.Bind("horn",
                                         "MaxSeconds",
                                         30,
                                         "This is the maximum amount of seconds the horn can sound when running 'horn [time]' Be warned, the higher this number more lag may occur.");
        }

        private static void NetcodePatcher()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            mls.LogInfo($"{modGUID} v{modVersion} was unloaded!");
        }
    }
}

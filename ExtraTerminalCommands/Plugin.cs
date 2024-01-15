using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ExtraTerminalCommands.TerminalCommands;
using HarmonyLib;
using System.Reflection;
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
        public static ConfigEntry<bool> introSongCommand;

        public const string modGUID = MyPluginInfo.PLUGIN_GUID;
        public const string modName = MyPluginInfo.PLUGIN_NAME;
        public const string modVersion = MyPluginInfo.PLUGIN_VERSION;

        private readonly Harmony harmony = new Harmony(modGUID);

        private static ExtraTerminalCommandsBase Instance;
        internal ManualLogSource mls;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            LoadConfig();
            harmony.PatchAll();

            //NetcodePatcher();
            //mls.LogInfo("Invoked NetcodePatcher");
            RegisterCommands();
            mls.LogInfo($"{modGUID} v{modVersion} has loaded!");
        }

        void RegisterCommands()
        {
            IntroSongCommand introSongCommandClass = new IntroSongCommand();

            if (!configExtraCommandsList.Value) { ExtraCommands.extraCommands(); }
            if (!configTimeCommand.Value) { TimeCommand.timeCommand(); }
            if (!configLaunchCommand.Value) { LaunchCommand.launchCommand(); }
            if (!configTeleportCommand.Value) { TeleportCommand.teleportCommand(); }
            if (!configInverseTeleportCommand.Value) { InverseTeleportCommand.inverseTeleportCommand(); }
            if (!configLightsCommand.Value) { LightsCommand.lightsCommand(); }
            if (!configDoorsCommand.Value) { DoorsCommand.doorsCommand(); }
            if (!introSongCommand.Value) { introSongCommandClass.introSongCommand(); }
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
            introSongCommand = Config.Bind("commands",
                                         "DisableIntroSong",
                                         false,
                                         "Plays the intro song when this command is run");
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

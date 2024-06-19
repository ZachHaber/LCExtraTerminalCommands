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
        public static ConfigFile config;
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
            mls = BepInEx.Logging.Logger.CreateLogSource(modName);

            config = Config;

            ExtraTerminalCommands.Config.Bind();

            harmony.PatchAll();

            MainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "extraterminalcommandsnetwork"));
            NetcodePatcher();
            mls.LogInfo("Invoked NetcodePatcher");

            RegisterCommands();
            mls.LogInfo($"{modGUID} v{modVersion} has loaded!");
        }

        void RegisterCommands()
        {
            if (!ExtraTerminalCommands.Config.configExtraCommandsList.Value) { ExtraCommands.extraCommands(); }
            if (!ExtraTerminalCommands.Config.configLaunchCommand.Value) { LaunchCommand.launchCommand(); }
            if (!ExtraTerminalCommands.Config.configDoorsCommand.Value) { DoorsCommand.doorsCommand(); }
            if (!ExtraTerminalCommands.Config.configHornCommand.Value) { HornCommand.hornCommand(); }
            if (!ExtraTerminalCommands.Config.configLightsCommand.Value) { LightsCommand.lightsCommand(); }
            if (!ExtraTerminalCommands.Config.configTimeCommand.Value) { TimeCommand.timeCommand(); }
            if (!ExtraTerminalCommands.Config.configSwitchCommand.Value) { SwitchCommand.switchCommand(); }
            if (!ExtraTerminalCommands.Config.configTeleportCommand.Value) { TeleportCommand.teleportCommand(); }
            if (!ExtraTerminalCommands.Config.configInverseTeleportCommand.Value) { InverseTeleportCommand.inverseTeleportCommand(); }
            if (!ExtraTerminalCommands.Config.configFlashCommand.Value) { RadarBoosterCommands.FlashCommand(); }
            if (!ExtraTerminalCommands.Config.configPingCommand.Value) { RadarBoosterCommands.PingCommand(); }
            if (!ExtraTerminalCommands.Config.configRandomMoonCommand.Value) { RandomMoonCommand.randomMoonCommand(); }
            if (!ExtraTerminalCommands.Config.configClearCommand.Value) { ClearScreenCommand.clearScreenCommand(); }
            if (!ExtraTerminalCommands.Config.configIntroSongCommand.Value) { IntroSongCommand introSongCommandClass = new IntroSongCommand(); introSongCommandClass.introSongCommand(); }
            daysJoined = 0;
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

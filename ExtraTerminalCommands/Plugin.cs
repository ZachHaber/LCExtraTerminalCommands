using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ExtraTerminalCommands.TerminalCommands;
using HarmonyLib;
using System.IO;
using System.Reflection;
using static TerminalApi.Events.Events;
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

        // public static bool firstRun = false;

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

            TerminalAwake += (object sender, TerminalEventArgs e) =>
            {
                RegisterCommands();
            };
            daysJoined = 0;

            mls.LogInfo($"{modGUID} v{modVersion} has loaded!");
        }

        private delegate void CommandFunc(bool forceDisable = false);

        public static void RegisterCommands()
        {

            mls.LogInfo("Adding Commands");
            ExtraCommands.extraCommands();
            LaunchCommand.launchCommand();
            DoorsCommand.doorsCommand();
            HornCommand.hornCommand();
            LightsCommand.lightsCommand();
            TimeCommand.timeCommand();
            SwitchCommand.switchCommand();
            TeleportCommand.teleportCommand();
            InverseTeleportCommand.inverseTeleportCommand();
            RadarBoosterCommands.FlashCommand();
            RadarBoosterCommands.PingCommand();
            RandomMoonCommand.randomMoonCommand();
            ClearScreenCommand.clearScreenCommand();
            IntroSongCommand.introSongCommand();
            mls.LogInfo("Added Commands");
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

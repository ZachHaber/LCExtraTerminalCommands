using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ExtraTerminalCommands.TerminalCommands;
using HarmonyLib;

namespace ExtraTerminalCommands
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("atomic.terminalapi", MinimumDependencyVersion: "1.5.0")]
    public class ExtraTerminalCommandsBase : BaseUnityPlugin
    {
        public ConfigEntry<bool> configLaunchCommand;
        public ConfigEntry<bool> configTimeCommand;
        public ConfigEntry<bool> configTeleportCommand;
        public ConfigEntry<bool> configInverseTeleportCommand;
        public ConfigEntry<bool> configLightsCommand;
        public ConfigEntry<bool> configDoorsCommand;
        public ConfigEntry<bool> introSongCommand;

        private const string modGUID = "Beauver.ExtraTerminalCommands";
        private const string modName = "Terminal Start Ship";
        private const string modVersion = "1.0.0";

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

            RegisterCommands();
            mls.LogInfo($"{modGUID} v{modVersion} has loaded!");
        }

        void RegisterCommands()
        {
            if (!configTimeCommand.Value) { TimeCommand.timeCommand(); }
            if (!configLaunchCommand.Value) { LaunchCommand.launchCommand(); }
            if (!configTeleportCommand.Value) { TeleportCommand.teleportCommand(); }
            if (!configInverseTeleportCommand.Value) { InverseTeleportCommand.inverseTeleportCommand(); }
            if (!configLightsCommand.Value) { LightsCommand.lightsCommand(); }
            if (!configDoorsCommand.Value) { DoorsCommand.doorsCommand(); }
            if (!introSongCommand.Value) { IntroSongCommand.introSongCommand(); }
        }

        private void LoadConfig()
        {
            configLaunchCommand = Config.Bind("commands",
                                         "DisableLaunch",
                                         false,
                                         "Disables the 'launch' command in terminal");
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

        private void OnDestroy()
        {
            mls.LogInfo($"{modGUID} v{modVersion} was unloaded!");
        }
    }
}

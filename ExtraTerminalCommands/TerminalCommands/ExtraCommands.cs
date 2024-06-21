using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
using TerminalApi.Classes;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class ExtraCommands
    {
        public static void extraCommands()
        {

            CommandInfo cmdInfo = new()
            {
                Category = "other",
                Description = "Shows extra commands.",
                DisplayTextSupplier = () => DisplayMenu("Extra")
            };
            Commands.AddCommandWithAliases("extra", cmdInfo, null, null, ETCNetworkHandler.Instance?.extraCmdDisabled ?? Config.configExtraCommandsList.Value);

            // Commands.Add("testing", (input) =>
            // {
            //     if (ETCNetworkHandler.Instance == null)
            //     {
            //         return "network not setup";
            //     }
            //     bool? newValue = null;
            //     switch (input)
            //     {
            //         case "extra":
            //             newValue = ETCNetworkHandler.Instance.extraCmdDisabled = !ETCNetworkHandler.Instance.extraCmdDisabled;
            //             break;
            //         case "launch":
            //             // Config.configLaunchCommand.Value = !Config.configLaunchCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.launchCmdDisabled = !ETCNetworkHandler.Instance.launchCmdDisabled;
            //             break;
            //         case "time":
            //             // Config.configTimeCommand.Value = !Config.configTimeCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.timeCmdDisabled = !ETCNetworkHandler.Instance.timeCmdDisabled;
            //             break;
            //         case "tp":
            //             // Config.configTeleportCommand.Value = !Config.configTeleportCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.tpCmdDisabled = !ETCNetworkHandler.Instance.tpCmdDisabled;
            //             break;
            //         case "tpPlayer":
            //             // Config.configTeleportPlayerCommand.Value = !Config.configTeleportPlayerCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.tpPlayerCmdDisabled = !ETCNetworkHandler.Instance.tpPlayerCmdDisabled;
            //             break;
            //         case "itp":
            //             // Config.configInverseTeleportCommand.Value = !Config.configInverseTeleportCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.itpCmdDisabled = !ETCNetworkHandler.Instance.itpCmdDisabled;
            //             break;
            //         case "lights":
            //             // Config.configLightsCommand.Value = !Config.configLightsCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.lightCmdDisabled = !ETCNetworkHandler.Instance.lightCmdDisabled;
            //             break;
            //         case "doors":
            //             // Config.configDoorsCommand.Value = !Config.configDoorsCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.doorCmdDisabled = !ETCNetworkHandler.Instance.doorCmdDisabled;
            //             break;
            //         case "intro":
            //             // Config.configIntroSongCommand.Value = !Config.configIntroSongCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.introCmdDisabled = !ETCNetworkHandler.Instance.introCmdDisabled;
            //             break;
            //         case "random":
            //             // Config.configRandomMoonCommand.Value = !Config.configRandomMoonCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.randomCmdDisabled = !ETCNetworkHandler.Instance.randomCmdDisabled;
            //             break;
            //         case "clear":
            //             // Config.configClearCommand.Value = !Config.configClearCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.clearCmdDisabled = !ETCNetworkHandler.Instance.clearCmdDisabled;
            //             break;
            //         case "switch":
            //             // Config.configSwitchCommand.Value = !Config.configSwitchCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.switchCmdDisabled = !ETCNetworkHandler.Instance.switchCmdDisabled;
            //             break;
            //         case "horn":
            //             // Config.configHornCommand.Value = !Config.configHornCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.hornCmdDisabled = !ETCNetworkHandler.Instance.hornCmdDisabled;
            //             break;
            //         case "flash":
            //             // Config.configFlashCommand.Value = !Config.configFlashCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.flashCmdDisabled = !ETCNetworkHandler.Instance.flashCmdDisabled;
            //             break;
            //         case "ping":
            //             // Config.configPingCommand.Value = !Config.configPingCommand.Value;
            //             newValue = ETCNetworkHandler.Instance.pingCmdDisabled = !ETCNetworkHandler.Instance.pingCmdDisabled;
            //             break;
            //     }
            //     if (newValue == null)
            //     {
            //         return $"{input} is not a valid option!";
            //     }
            //     ExtraTerminalCommandsBase.RegisterCommands();
            //     return $"{input} is now {newValue}";
            // });
        }

        public static string DisplayMenu(string category)
        {
            string menu = "";
            foreach (var command in Commands.CommandDisplays)
            {
                if (command.active && command.cmd_info?.Category == category)
                {
                    menu += $">{command.cmd_info.Title ?? command.cmd_string.ToUpper()}\n{command.cmd_info.Description ?? ""}\n\n";
                }
            }
            return menu.Trim() + "\n\n";
        }

    }
}

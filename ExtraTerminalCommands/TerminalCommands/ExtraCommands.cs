using ExtraTerminalCommands.Networking;
using System;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class ExtraCommands
    {
        public static void extraCommands()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "other",
                Description = "Shows every command from the \"ExtraCommands\" mod.",
                DisplayTextSupplier = onExtraCommands
            };
            AddCommand("EXTRA", cmdInfo);
            AddCommand("Extra", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = onExtraCommands });
            AddCommand("EC", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = onExtraCommands });
            AddCommand("Extra-Commands", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = onExtraCommands });
            AddCommand("Extra Commands", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = onExtraCommands });
        }

        private static string onExtraCommands()
        {
            ETCNetworkHandler NH = ETCNetworkHandler.Instance;
            if (NH.extraCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }

            String message = "Extra Terminal Commands:";

            if (!NH.tpCmdDisabled) { message += "\n\n>TP\n" + TeleportCommand.description; }
            if (!NH.itpCmdDisabled) { message += "\n\n>ITP\n" + InverseTeleportCommand.description; }
            if (!NH.randomCmdDisabled) { message += "\n\n>RANDOM [MONEY]\n" + RandomMoonCommand.description; }
            if (!NH.launchCmdDisabled) { message += "\n\n>LAUNCH\n" + LaunchCommand.description; }
            if (!NH.doorCmdDisabled) { message += "\n\n>DOORS\n" + DoorsCommand.description; }
            if (!NH.lightCmdDisabled) { message += "\n\n>LIGHTS\n" + LightsCommand.description; }
            if (!NH.introCmdDisabled) { message += "\n\n>INTRO\n" + IntroSongCommand.description; }
            if (!NH.timeCmdDisabled) { message += "\n\n>TIME\n" + TimeCommand.description; }
            if (!NH.clearCmdDisabled) { message += "\n\n>CLEAR\n" + ClearScreenCommand.description; }
            if (!NH.switchCmdDisabled) { message += "\n\n>S\n" + SwitchCommand.description; }

            return message + "\n\n";
        }
    }
}

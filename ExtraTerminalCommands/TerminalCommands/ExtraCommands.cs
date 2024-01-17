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
            String message = "Extra Terminal Commands:";

            if (!ExtraTerminalCommandsBase.configTeleportCommand.Value) { message += "\n\n>TP\n" + TeleportCommand.description; }
            if (!ExtraTerminalCommandsBase.configInverseTeleportCommand.Value) { message += "\n\n>ITP\n" + InverseTeleportCommand.description; }
            if (!ExtraTerminalCommandsBase.configRandomMoonCommand.Value) { message += "\n\n>RANDOM [MONEY/WEATHER/BOTH]\n" + RandomMoonCommand.description; }
            if (!ExtraTerminalCommandsBase.configLaunchCommand.Value) { message += "\n\n>LAUNCH\n" + LaunchCommand.description; }
            if (!ExtraTerminalCommandsBase.configDoorsCommand.Value) { message += "\n\n>DOORS\n" + DoorsCommand.description; }
            if (!ExtraTerminalCommandsBase.configLightsCommand.Value) { message += "\n\n>LIGHTS\n" + LightsCommand.description; }
            if (!ExtraTerminalCommandsBase.configIntroSongCommand.Value) { message += "\n\n>INTRO\n" + IntroSongCommand.description; }
            if (!ExtraTerminalCommandsBase.configTimeCommand.Value) { message += "\n\n>TIME\n" + TimeCommand.description; }

            return message + "\n\n";
        }
    }
}

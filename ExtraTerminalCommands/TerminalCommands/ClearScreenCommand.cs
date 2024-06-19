using ExtraTerminalCommands.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using ExtraTerminalCommands.Handlers;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class ClearScreenCommand
    {
        public static string description = "Clears the terminal view.";
        public static void clearScreenCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "Extra",
                Description = description,
                DisplayTextSupplier = onClear
            };

            Commands.AddCommandWithAliases("clear", cmdInfo, Config.clearCommandAliases.Value);
        }

        private static string onClear()
        {
            if(ETCNetworkHandler.Instance.clearCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }
            return "";
        }

    }
}

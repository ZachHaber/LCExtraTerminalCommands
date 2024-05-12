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
                Description = "Shows extra commands.",
            };
            AddCommand("extra", cmdInfo);
        }

    }
}

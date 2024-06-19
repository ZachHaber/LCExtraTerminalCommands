using ExtraTerminalCommands.Handlers;
using TerminalApi.Classes;

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
            Commands.AddCommandWithAliases("extra", cmdInfo);
        }

    }
}

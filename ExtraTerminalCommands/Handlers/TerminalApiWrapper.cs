using System.Collections.Generic;
using System.Linq;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.Handlers
{

    public delegate TResult FuncWI<out TResult>(string input = "");

    //public class CommandInfo : TerminalApi.Classes.CommandInfo { }
    public class Command
    {
        public CommandInfo cmd_info;
        public string cmd_string;
        public FuncWI<string> cmd_func { get; set; }
    }

    public class Commands
    {
        public static List<Command> CommandInfos = new List<Command>();
        public static void Add(string cmd_string, FuncWI<string> cmd_func, CommandInfo cmd_info = null)
        {
            cmd_string = cmd_string.ToLower();
            if (cmd_info == null)
                cmd_info = new CommandInfo()
                {
                    Category = "None"
                };

            cmd_info.DisplayTextSupplier = () =>
            {
                return cmd_func(GetTerminalInput());
            };
            AddCommand(cmd_string, cmd_info, "", true);

            Command command = new Command()
            {
                cmd_string = cmd_string,
                cmd_info = cmd_info,
                cmd_func = cmd_func
            };

            CommandInfos.Add(command);
        }
        public static string Execute(string cmd_text)
        {

            string[] cmd_array = cmd_text.Split(new char[1] { ' ' });
            string displayText = null;
            string cmd = cmd_array[0];
            string args = string.Join(" ", cmd_array.Skip(1).ToArray());
            Command commandInfo = CommandInfos.FirstOrDefault(cI => cI.cmd_string == cmd);
            if (commandInfo != null)
            {
                displayText = commandInfo.cmd_func(args);
            }
            return displayText.Trim();
        }

        /// <summary>
        /// 
        /// Add aliases to a basic command via a CommandInfo class and a string array of aliases
        /// Creates a new command title to add in the alias strings
        /// </summary>
        public static void AddCommandWithAliases(string command, CommandInfo commandInfo, List<string> aliases = null)
        {
            if (aliases != null && aliases.Count > 0)
            {
                var aliasPlural = aliases.Count > 1 ? "es" : "";
                var aliasText = $" (alias{aliasPlural}: {string.Join(", ", aliases.Select(alias => $"'{alias}'"))})";
                AddCommand(command, new CommandInfo
                {
                    Title = (commandInfo.Title ?? command.ToUpper()) + aliasText,
                    Category = commandInfo.Category,
                    Description = commandInfo.Description,
                    DisplayTextSupplier = commandInfo.DisplayTextSupplier
                });
                foreach (string alias in aliases)
                {
                    var aliasedCommandInfo = new CommandInfo { Description = commandInfo.Description, DisplayTextSupplier = commandInfo.DisplayTextSupplier };
                    ExtraTerminalCommandsBase.mls.LogInfo($"Command {command} - Adding alias {alias}.");
                    AddCommand(alias, aliasedCommandInfo);
                }
            }
            else
            {
                AddCommand(command, commandInfo);

            }
        }

    }

}

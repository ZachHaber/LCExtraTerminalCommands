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

    public class CommandDisplay
    {
        public CommandInfo cmd_info;
        public string cmd_string;
        public bool active;
    }

    public class Commands
    {
        public static List<CommandDisplay> CommandDisplays = [];
        public static List<Command> CommandInfos = [];
        public static void Add(string cmd_string, FuncWI<string> cmd_func, CommandInfo cmd_info = null, List<string> aliases = null, bool disable = false)
        {
            cmd_string = cmd_string.ToLower();
            if (cmd_info == null)
                cmd_info = new CommandInfo()
                {
                    Category = "None"
                };

            cmd_info.DisplayTextSupplier = () =>
            {
                return Execute(GetTerminalInput());
            };
            AddCommandWithAliases(cmd_string, cmd_info, aliases, "", disable);

            if (disable)
            {
                HashSet<string> allCommands = [cmd_string, .. (aliases ?? [])];
                CommandInfos.RemoveAll((command) => allCommands.Contains(command.cmd_string));
            }
            else
            {
                Command command = new Command()
                {
                    cmd_string = cmd_string,
                    cmd_info = cmd_info,
                    cmd_func = cmd_func
                };

                CommandInfos.Add(command);
                if (aliases != null)
                {

                    foreach (string alias in aliases)
                    {
                        CommandInfos.Add(new Command() { cmd_string = alias, cmd_info = cmd_info, cmd_func = cmd_func });
                    }
                }
            }
        }
        public static string Execute(string cmd_text)
        {

            string[] cmd_array = cmd_text.Split([' ']);
            string displayText = null;
            string cmd = cmd_array[0];
            string args = string.Join(" ", cmd_array.Skip(1).ToArray()).Trim();
            Command commandInfo = CommandInfos.FirstOrDefault(cI => cI.cmd_string == cmd);
            if (commandInfo != null)
            {
                displayText = commandInfo.cmd_func(args);
                return displayText.Trim() + "\n\n";
            }
            return displayText ?? "";
        }

        /// <summary>
        ///
        /// Add aliases to a basic command via a CommandInfo class and a string array of aliases
        /// Creates a new command title to add in the alias strings
        /// </summary>
        public static void AddCommandWithAliases(string command, CommandInfo commandInfo, List<string> aliases = null, string verbWord = null, bool disable = false)
        {
            if (aliases != null && aliases.Count > 0)
            {
                var aliasPlural = aliases.Count > 1 ? "es" : "";
                var aliasText = $" (alias{aliasPlural}: {string.Join(", ", aliases.Select(alias => $"'{alias}'"))})";
                AddRemoveCommand(disable, command, new CommandInfo
                {
                    Title = (commandInfo.Title ?? command.ToUpper()) + aliasText,
                    Category = commandInfo.Category,
                    Description = commandInfo.Description,
                    DisplayTextSupplier = commandInfo.DisplayTextSupplier
                }, verbWord);
                foreach (string alias in aliases)
                {
                    var aliasedCommandInfo = new CommandInfo { Category = "None", Description = commandInfo.Description, DisplayTextSupplier = commandInfo.DisplayTextSupplier };
                    AddRemoveCommand(disable, alias, aliasedCommandInfo, verbWord);
                }
            }
            else
            {
                AddRemoveCommand(disable, command, commandInfo, verbWord);
            }
        }
        public static void AddRemoveCommand(bool disable, string command, CommandInfo commandInfo, string verbWord = null)
        {
            if (commandInfo.Category == "Extra")
            {
                // Add the Extra Category to the displays list
                // that way it can be used for generating a stable list of commands
                // since TerminalAPI doesn't actually remove removed commands from its display...
                var display = CommandDisplays.Find(display => display.cmd_string == command);
                if (display == null)
                {
                    CommandDisplays.Add(new() { active = !disable, cmd_info = commandInfo, cmd_string = command });
                }
                else
                {
                    display.active = !disable;
                    display.cmd_info = commandInfo;
                }
            }
            var keyword = GetKeyword(command);
            if (disable)
            {
                // Remove the keyword!
                if (keyword == null) return;
                DeleteKeyword(command);
            }
            else
            {
                if (keyword != null) return;
                AddCommand(command, commandInfo, verbWord);
            }
        }
    }

}

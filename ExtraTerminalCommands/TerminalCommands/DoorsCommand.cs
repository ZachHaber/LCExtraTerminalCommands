using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
using System.ComponentModel;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class DoorsCommand
    {
        public static string description = "Toggles the door switch.";
        public static void doorsCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "Extra",
                Description = description,
                DisplayTextSupplier = onDoorCommand
            };
            Commands.AddCommandWithAliases("doors", cmdInfo, ["door", "d"]);
        }

        private static string onDoorCommand()
        {
            if(ETCNetworkHandler.Instance.doorCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }

            if (!StartOfRound.Instance.shipDoorsEnabled)
            {
                return "You are currently not on a moon, you can not toggle the doors.\n\n";
            }

            string doorResult;
            if (StartOfRound.Instance.hangarDoorsClosed)
            { doorResult = "opened."; }
            else
            { doorResult = "closed."; }

            InteractTrigger doorButton = GameObject.Find(StartOfRound.Instance.hangarDoorsClosed ? "StartButton" : "StopButton").GetComponentInChildren<InteractTrigger>();
            doorButton.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            return $"Doors {doorResult}\n\n";
        }
    }
}

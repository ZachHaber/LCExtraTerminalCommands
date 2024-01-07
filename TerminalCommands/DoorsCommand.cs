using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class DoorsCommand
    {
        public static void doorsCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "other",
                Description = "Toggles the door switch.",
                DisplayTextSupplier = onDoorCommand
            };
            CommandInfo cmdInfo2 = new CommandInfo
            {
                Category = "none",
                Description = "Toggles the door switch.",
                DisplayTextSupplier = onDoorCommand
            };
            AddCommand("doors", cmdInfo);
            AddCommand("door", cmdInfo2);
        }

        private static string onDoorCommand()
        {
            if (!StartOfRound.Instance.shipDoorsEnabled)
            {
                return "You are currently not on a moon, you can not toggle the doors.\n";
            }

            string doorResult;
            if (StartOfRound.Instance.hangarDoorsClosed)
            { doorResult = "closed."; }
            else
            { doorResult = "opened."; }

            InteractTrigger doorButton = GameObject.Find(StartOfRound.Instance.hangarDoorsClosed ? "StartButton" : "StopButton").GetComponentInChildren<InteractTrigger>();
            doorButton.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            return $"Doors {doorResult}\n";
        }
    }
}

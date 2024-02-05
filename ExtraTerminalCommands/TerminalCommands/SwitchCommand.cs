using ExtraTerminalCommands.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;
using static TerminalApi.Events.Events;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class SwitchCommand
    {
        public static string description = "Switches the camera view to the next or specified user";

        public static void switchCommand()
        {
            TerminalParsedSentence += onParsedPlayerSentance;
        }

        private static void onParsedPlayerSentance(object sender, TerminalParseSentenceEventArgs e)
        {
            if(ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                return;
            }

            string userInput = GetTerminalInput();
            string[] userInputParts = userInput.Split(' ');

            if (userInputParts.Length == 1 && (userInputParts[0] == "s" || userInputParts[0] == "sw"))
            {
                StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(callRPC: true);
                return;
            }
            else if(userInputParts.Length == 2 && (userInputParts[0] == "s" || userInputParts[0] == "sw"))
            {
                Terminal terminal = GameObject.FindObjectOfType<Terminal>();
                int playerNum = terminal.CheckForPlayerNameCommand("switch", userInputParts[1]);
                StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(playerNum);

            }
        }
    }
}

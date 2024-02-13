using ExtraTerminalCommands.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;
using static TerminalApi.Events.Events;
using GameNetcodeStuff;
using Unity.Netcode;
using ExtraTerminalCommands.Handlers;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class SwitchCommand
    {
        public static string description = "Switches the camera view to the next or specified user";

        public static void switchCommand()
        {
            TerminalParsedSentence += onParsedPlayerSentance;
            AddCommand("s", new CommandInfo { Category = "hide", Description = description, DisplayTextSupplier = returnText });
            AddCommand("sw", new CommandInfo { Category = "hide", Description = description, DisplayTextSupplier = returnText });
        }
        public static string returnText()
        {
            if (ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }
            return "Switched radar scan view\n";
        }

        private static void onParsedPlayerSentance(object sender, TerminalParseSentenceEventArgs e)
        {

            if(ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                return;
            }
            ParsedPlayerSentanceHandler.onParsedPlayerSentance(sender, e);
        }

        public static void switchNormal()
        {
            StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(callRPC: true);
        }
        public static void switchInput(string[] userInputParts)
        {
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            int playerNum = terminal.CheckForPlayerNameCommand("switch", userInputParts[1]);
            StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(playerNum);
        }
    }
}

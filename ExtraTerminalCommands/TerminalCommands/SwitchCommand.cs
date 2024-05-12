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

            string shortCommand2 = "sw";
            Commands.Add(shortCommand2, (string input) =>
            {
                input = input.Substring(shortCommand2.Length).Trim();
                return onSwitchCommand(input);
            }, new CommandInfo() { Category = "Extra", Description = description });

            string shortCommand = "s";
            Commands.Add(shortCommand, (string input) =>
            {
                input = input.Substring(shortCommand.Length).Trim();
                return onSwitchCommand(input);
            }, new CommandInfo() { Category = "Extra", Description = description });

            string shortCommand3 = "y";
            Commands.Add(shortCommand3, (string input) =>
            {
                input = input.Substring(shortCommand3.Length).Trim();
                return onSwitchCommand(input);
            }, new CommandInfo() { Category = "Extra", Description = description });
        }
        public static string returnText()
        {
            if (ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }
            return "Switched radar scan view\n";
        }

        //private static void onParsedPlayerSentance(object sender, TerminalParseSentenceEventArgs e)
        //{

        //    if (ETCNetworkHandler.Instance.switchCmdDisabled)
        //    {
        //        return;
        //    }
        //    ParsedPlayerSentanceHandler.onParsedPlayerSentance(sender, e);
        //}

        private static string onSwitchCommand(string input)
        {
            if (ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                return returnText();
            }
            if (input.Length == 0)
            {
                switchNormal();
                return returnText();
            }
            else
            {
                if(switchInput(["", input]))
                {
                return returnText();
                }
                else
                {
                    return $"Player '{input}' not found!\n";
                };
            }

        }

        public static bool switchNormal()
        {
            StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(callRPC: true);
            return true;
        }
        public static bool switchInput(string[] userInputParts)
        {
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            int playerNum = terminal.CheckForPlayerNameCommand("switch", userInputParts[1]);
            if (playerNum == -1)
            {
                return false;
            }
                StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(playerNum);
                return true;
        }
    }
}

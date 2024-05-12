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
        public static string basicDescription = "Switches the camera view to the next user. Does not allow specifying the user";

        public static void switchCommand()
        {

            string shortCommand = "sw";
            Commands.Add(shortCommand, (string input) =>
            {
                input = input.Substring(shortCommand.Length).Trim();
                return onSwitchCommand(input);
            }, new CommandInfo()
            {
                Title = $"{shortCommand.ToUpper()} [Player Name]?",
                Category = "Extra",
                Description = description
            });

            AddCommand("s", new CommandInfo
            {
                Category = "Extra",
                Description = basicDescription,
                DisplayTextSupplier = () => onSwitchCommand("")
            });
            // For some reason the Commands.Add isn't working with text of a single letter long.
            //string shortCommand = "s";
            //Commands.Add(shortCommand, (string input) =>
            //{
            //    input = input.Substring(shortCommand.Length).Trim();
            //    return onSwitchCommand(input);
            //}, new CommandInfo() { Category = "Extra", Description = description });


        }
        public static string returnText()
        {
            if (ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }
            return "Switched radar scan view\n\n";
        }

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
                if (switchInput(input))
                {
                    return returnText();
                }
                else
                {
                    return $"Player '{input}' not found!\n\n";
                };
            }

        }

        public static bool switchNormal()
        {
            StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(callRPC: true);
            return true;
        }
        public static bool switchInput(string userInput)
        {
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            int playerNum;
            if (int.TryParse(userInput, out playerNum))
            {
                // Convert 1 indexed number to 0 indexed number!
                playerNum -= 1;
            }
            else
            {
                playerNum = terminal.CheckForPlayerNameCommand("switch", userInput);

            }
            if (playerNum <0 || playerNum >= StartOfRound.Instance.mapScreen.radarTargets.Count)
            {
                return false;
            }
            StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(playerNum);
            return true;
        }
    }
}

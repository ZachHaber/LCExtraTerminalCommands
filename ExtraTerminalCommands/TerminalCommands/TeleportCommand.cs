using System;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using UnityEngine;
using System.Reflection;
using ExtraTerminalCommands.Networking;
using GameNetcodeStuff;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class TeleportCommand
    {
        public static string description = "Teleports the current selected player on the camera.";
        public static void teleportCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "none",
                Description = description,
                DisplayTextSupplier = OnTeleportCommand
            };
            CommandInfo cmdInfo2 = new CommandInfo
            {
                Category = "none",
                Description = description,
                DisplayTextSupplier = OnTeleportCommand
            };
            AddCommand("teleport", cmdInfo);
            AddCommand("tp", cmdInfo2);

        }


        public static string OnTeleportCommand()
        {
            if (ETCNetworkHandler.Instance.tpCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }

            if (GameObject.Find("Teleporter(Clone)") == null)
            {
                return "You do not own the teleporter.\n";
            }

            ShipTeleporter teleporter = GameObject.Find("Teleporter(Clone)").GetComponent<ShipTeleporter>();
            FieldInfo cooldownField = teleporter.GetType().GetField("cooldownTime", BindingFlags.NonPublic | BindingFlags.Instance);
            float cooldownTimef = (float)cooldownField.GetValue(teleporter);


            if (cooldownField == null)
            {
                return "Your teleporter does not have a cooldown.\n";
            }

            if (teleporter == null)
            {
                return "You do not own the teleporter. If you do, try restarting the game.\n";
            }

            if (!teleporter.buttonTrigger.interactable)
            {
                //bug, shows 10s default cooldown
                return "Teleporter is on cooldown, " + Math.Round(cooldownTimef, 1) + " seconds remain.\n";
            }
            else
            {
                string userInput = GetTerminalInput();
                ExtraTerminalCommandsBase.mls.LogInfo($"TP command called with '{userInput}'");
                string[] userInputParts = userInput.Split(' ');
                Terminal terminal = GameObject.FindObjectOfType<Terminal>();
                if (userInputParts.Length > 1)
                {
                    int playerNum = terminal.CheckForPlayerNameCommand("switch", userInputParts[1]);
                    PlayerControllerB currentPlayer = StartOfRound.Instance.mapScreen.targetedPlayer;
                    int curIndex = StartOfRound.Instance.mapScreen.radarTargets.FindIndex(target => target.name == currentPlayer.name);
                    if (playerNum == -1)
                    {
                        ExtraTerminalCommandsBase.mls.LogInfo($"{userInputParts[1]} was not a valid player! ");

                    }
                    else
                    {
                        StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(playerNum);

                        teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                        if (playerNum != curIndex)
                        {
                            StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(playerNum);
                        }
                        else if (curIndex == -1)
                        {
                            ExtraTerminalCommandsBase.mls.LogInfo($"the current player {currentPlayer.name} was not in the radar targets!");
                        }

                    }
                }
                else
                {
                    teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                }

                return "Teleporting player to ship.\n";
            }
        }
    }
}

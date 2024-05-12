using System;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using UnityEngine;
using System.Reflection;
using ExtraTerminalCommands.Networking;
using GameNetcodeStuff;
using TerminalApi;
using ExtraTerminalCommands.Handlers;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class TeleportCommand
    {
        public static string description = "Teleports the player name (or index) specified. Current selected if not specified.";

        public static void teleportCommand()
        {
            string shortCommand = "tp";
            Commands.Add(shortCommand, (string input) =>
            {
                input = input.Substring(shortCommand.Length).Trim();
                return OnTeleportCommand(input);
            }, new CommandInfo()
            {
                Title = $"{shortCommand.ToUpper()} [Player Name]?",
                Category = "Extra",
                Description = description
            });
        }

        public static async void TeleportOnMapSync(ShipTeleporter teleporter, string toTeleportUsername, int newIndex, int originalIndex)
        {
            //StartOfRound.Instance.mapScreen.index
            int numDelays = 0;
            while ((StartOfRound.Instance.mapScreen.syncingTargetPlayer || StartOfRound.Instance.mapScreen.targetedPlayer?.playerUsername != toTeleportUsername) && ++numDelays <= 10)
            {
                if(!StartOfRound.Instance.mapScreen.syncingTargetPlayer && (StartOfRound.Instance.mapScreen.targetTransformIndex!=0 && StartOfRound.Instance.mapScreen.targetTransformIndex==newIndex))
                {
                    // anther condition - Radar boosters don't have a `targetedPlayer` set, but will have `targetTransformIndex` set to their index instead...
                    break;
                }
                ExtraTerminalCommandsBase.mls.LogInfo($"Waiting for camera update to finish... Currently Targeting {StartOfRound.Instance.mapScreen.targetedPlayer?.playerUsername} != {toTeleportUsername}");
                await Task.Delay(100);
            }
            //await Task.Delay(20);
            if (numDelays > 10)
            {
                ExtraTerminalCommandsBase.mls.LogError("Syncing target players never finished!");
            }
            else
            {
                ExtraTerminalCommandsBase.mls.LogInfo($"Camera update finished!");
            }
            teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);



            if (originalIndex != -1)
            {
                StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(originalIndex);
            }
        }

        public static string OnTeleportCommand(string userInput = "")
        {
            if (ETCNetworkHandler.Instance.tpCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }

            if (GameObject.Find("Teleporter(Clone)") == null)
            {
                return "You do not own the teleporter.\n\n";
            }

            ShipTeleporter teleporter = GameObject.Find("Teleporter(Clone)").GetComponent<ShipTeleporter>();
            FieldInfo cooldownField = teleporter.GetType().GetField("cooldownTime", BindingFlags.NonPublic | BindingFlags.Instance);
            float cooldownTimef = (float)cooldownField.GetValue(teleporter);


            if (cooldownField == null)
            {
                return "Your teleporter does not have a cooldown.\n\n";
            }

            if (teleporter == null)
            {
                return "You do not own the teleporter. If you do, try restarting the game.\n\n";
            }

            if (!teleporter.buttonTrigger.interactable)
            {
                //bug, shows 10s default cooldown
                return "Teleporter is on cooldown, " + Math.Round(cooldownTimef, 1) + " seconds remain.\n\n";
            }
            else
            {
                ManualCameraRenderer mapScreen = StartOfRound.Instance.mapScreen;
                ExtraTerminalCommandsBase.mls.LogInfo($"TP command called with '{userInput}'");

                Terminal terminal = GameObject.FindObjectOfType<Terminal>();
                if (userInput.Length > 0)
                {
                    int playerNum;
                    if (int.TryParse(userInput.Split(" ")[0], out playerNum))
                            {
                        // Convert 1 indexed number to 0 indexed number!
                        playerNum -= 1;
                            }
                        else
                        {
                        playerNum = terminal.CheckForPlayerNameCommand("switch", userInput);
                        }


                    if (playerNum < 0 || playerNum >= mapScreen.radarTargets.Count || mapScreen.radarTargets[playerNum] == null)
                    {
                        ExtraTerminalCommandsBase.mls.LogInfo($"'{userInput}' was not a valid player! ");
                        return $"Player {userInput} was not a valid player!\n\n";
                    }

                    var controller = mapScreen.radarTargets[playerNum].transform.gameObject.GetComponent<PlayerControllerB>();
                    if (controller != null && !controller.isPlayerControlled && !controller.isPlayerDead && controller.redirectToEnemy == null)
                    {
                        ExtraTerminalCommandsBase.mls.LogInfo($"Teleport by player number: {playerNum} is an invalid target");
                        // Invalid target!
                        return $"Invalid target: {playerNum}\n\n";
                    }
                    TransformAndName tpTarget = mapScreen.radarTargets[playerNum];

                    //ExtraTerminalCommandsBase.mls.LogInfo($"Switching to {tpTarget.name} at index {playerNum}; Current transformIndex = {mapScreen.targetTransformIndex}");


                    PlayerControllerB currentPlayer = mapScreen.targetedPlayer;
                    int curIndex = mapScreen.targetTransformIndex > 0 ? mapScreen.targetTransformIndex : currentPlayer == null ? -1 : mapScreen.radarTargets.FindIndex(target => target.name == currentPlayer.playerUsername);
                    if (curIndex == -1)
                    {
                        ExtraTerminalCommandsBase.mls.LogInfo($"the current player {currentPlayer?.playerUsername} was not in the radar targets!");
                    }
                    if (curIndex != mapScreen.targetTransformIndex)
                    {
                        ExtraTerminalCommandsBase.mls.LogError($"The found player index ({curIndex}) doesn't match the targetTransformIndex ({mapScreen.targetTransformIndex})");
                    }



                    if (curIndex != playerNum)
                    {
                        ExtraTerminalCommandsBase.mls.LogInfo($"'{userInput}' gave playerNum {playerNum}. Radar is showing player '{currentPlayer?.playerUsername}' - index {curIndex}. Transform index is {mapScreen.targetTransformIndex}. Switching to {tpTarget.name} at index {playerNum}");
                        // This requires switching players!
                        mapScreen.SwitchRadarTargetAndSync(playerNum);
                        TeleportOnMapSync(teleporter, tpTarget.name, playerNum, curIndex);
                        return $"Teleporting player {tpTarget.name} {curIndex} to ship.\n\n";
                    }
                    else
                    {
                        ExtraTerminalCommandsBase.mls.LogInfo("Already on the correct player! No switching needed!");
                    }
                }

                teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                if (mapScreen.targetedPlayer == null)
                {
                    // This is likely not a player - should switch to a different user.
                    mapScreen.SwitchRadarTargetForward(true);
                }
                return "Teleporting player to ship.\n\n";
            }
        }
    }
}

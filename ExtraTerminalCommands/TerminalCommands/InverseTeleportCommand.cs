using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
using System;
using System.Reflection;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class InverseTeleportCommand
    {
        public static string description = "Activates the Inverse Teleporter.";
        public static void inverseTeleportCommand()
        {
            Commands.AddCommandWithAliases("itp", new CommandInfo
            {
                Category = "Extra",
                Description = description,
                DisplayTextSupplier = OnInverseTeleportCommand
            }, Config.inverseTeleportCommandAliases.Value, null, ETCNetworkHandler.Instance?.itpCmdDisabled ?? Config.configInverseTeleportCommand.Value);
        }

        private static string OnInverseTeleportCommand()
        {
            if (ETCNetworkHandler.Instance.itpCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }

            if (!StartOfRound.Instance.shipDoorsEnabled || !StartOfRound.Instance.currentLevel.planetHasTime)
            {
                return "You are currently not on a moon.\n\n";
            }

            if (GameObject.Find("InverseTeleporter(Clone)") == null)
            {
                return "You do not own the inverse teleporter.\n\n";
            }

            ShipTeleporter iTeleporter = GameObject.Find("InverseTeleporter(Clone)").GetComponent<ShipTeleporter>();
            FieldInfo cooldownField = iTeleporter.GetType().GetField("cooldownTime", BindingFlags.NonPublic | BindingFlags.Instance);
            float cooldownTimef = (float)cooldownField.GetValue(iTeleporter);


            if (cooldownField == null)
            {
                return "Your inverse teleporter does not have a cooldown.\n\n";
            }

            if (iTeleporter == null)
            {
                return "You do not own the inverse teleporter. If you do, try restarting the game.\n\n";
            }

            if (!iTeleporter.buttonTrigger.interactable)
            {
                //bug, shows 10s default cooldown
                return "Inverse teleporter is on cooldown, " + Math.Round(cooldownTimef, 1) + " seconds remain.\n\n";
            }
            else
            {
                iTeleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                return "Teleporting player(s) into the facility.\n\n";
            }
        }
    }
}

using System;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using UnityEngine;
using System.Reflection;
using ExtraTerminalCommands.Networking;

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


        private static string OnTeleportCommand()
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
                teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                return "Teleporting player to ship.\n";
            }
        }
    }
}

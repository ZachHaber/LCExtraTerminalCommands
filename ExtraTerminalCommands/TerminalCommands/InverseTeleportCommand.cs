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
        public static string description = "Inverse teleports everyone on the inverse teleporter.";
        public static void inverseTeleportCommand()
        {
            //AddCommand("iteleport", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = OnInverseTeleportCommand });
            AddCommand("itp", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = OnInverseTeleportCommand });
            //AddCommand("inverseteleport", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = OnInverseTeleportCommand });
            //AddCommand("inverse-teleport", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = OnInverseTeleportCommand });
            //AddCommand("inverse teleport", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = OnInverseTeleportCommand });
        }

        private static string OnInverseTeleportCommand()
        {
            if (ETCNetworkHandler.Instance.itpCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }

            if (!StartOfRound.Instance.shipDoorsEnabled || !StartOfRound.Instance.currentLevel.planetHasTime)
            {
                return "You are currently not on a moon.\n";
            }

            if (GameObject.Find("InverseTeleporter(Clone)") == null)
            {
                return "You do not own the inverse teleporter.\n";
            }

            ShipTeleporter iTeleporter = GameObject.Find("InverseTeleporter(Clone)").GetComponent<ShipTeleporter>();
            FieldInfo cooldownField = iTeleporter.GetType().GetField("cooldownTime", BindingFlags.NonPublic | BindingFlags.Instance);
            float cooldownTimef = (float)cooldownField.GetValue(iTeleporter);


            if (cooldownField == null)
            {
                return "Your inverse steleporter does not have a cooldown.\n";
            }

            if (iTeleporter == null)
            {
                return "You do not own the inverse teleporter. If you do, try restarting the game.\n";
            }

            if (!iTeleporter.buttonTrigger.interactable)
            {
                //bug, shows 10s default cooldown
                return "Inverse teleporter is on cooldown, " + Math.Round(cooldownTimef, 1) + " seconds remain.\n";
            }
            else
            {
                iTeleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                return "Teleporting player to facility.\n";
            }
        }
    }
}

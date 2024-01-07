using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class InverseTeleportCommand
    {
        public static void inverseTeleportCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "other",
                Description = "Inverse teleports everyone on the inverse teleporter.",
                DisplayTextSupplier = OnInverseTeleportCommand
            };
            AddCommand("iteleport", cmdInfo);
            AddCommand("itp", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = OnInverseTeleportCommand });
            AddCommand("inverseteleport", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = OnInverseTeleportCommand });
            AddCommand("inverse-teleport", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = OnInverseTeleportCommand });
            AddCommand("inverse teleport", new CommandInfo { Category = "none", Description = "Inverse teleports everyone on the inverse teleporter.", DisplayTextSupplier = OnInverseTeleportCommand });
        }


        private static string OnInverseTeleportCommand()
        {

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class LightsCommand
    {
        public static void lightsCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "other",
                Description = "Toggles the lightswitch.",
                DisplayTextSupplier = onLightCommand
            };
            CommandInfo cmdInfo2 = new CommandInfo
            {
                Category = "none",
                Description = "Toggles the lightswitch.",
                DisplayTextSupplier = onLightCommand
            };
            AddCommand("lights", cmdInfo);
            AddCommand("light", cmdInfo2);

        }

        private static string onLightCommand()
        {
            GameObject.Find("LightSwitch").GetComponent<InteractTrigger>().onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            return "Toggeled the lights.\n";
        }
    }
}

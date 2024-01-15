using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class LightsCommand
    {
        public static string description = "Toggles the lightswitch.";
        public static void lightsCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "none",
                Description = description,
                DisplayTextSupplier = onLightCommand
            };

            AddCommand("lights", cmdInfo);
            AddCommand("light", new CommandInfo{ Category = "none", Description = description, DisplayTextSupplier = onLightCommand });
            AddCommand("l", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = onLightCommand });

        }

        private static string onLightCommand()
        {
            GameObject.Find("LightSwitch").GetComponent<InteractTrigger>().onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            return "Toggeled the lights.\n";
        }
    }
}

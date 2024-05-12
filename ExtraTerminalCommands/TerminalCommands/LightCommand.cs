using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class LightsCommand
    {
        public static string description = "Switches the ship lights on and off.";
        public static void lightsCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "Extra",
                Description = description,
                DisplayTextSupplier = onLightCommand
            };

            Commands.AddCommandWithAliases("lights", cmdInfo, ["light"]);

        }

        private static string onLightCommand()
        {
            if (ETCNetworkHandler.Instance.lightCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }
            GameObject.Find("LightSwitch").GetComponent<InteractTrigger>().onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            return "Toggled the lights.\n\n";
        }
    }
}

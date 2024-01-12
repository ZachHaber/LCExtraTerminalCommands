using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class LaunchCommand
    {
        public static string description = "Launches or Lands the ship.";
        public static void launchCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "none",
                Description = description,
                DisplayTextSupplier = OnLaunchCommand
            };

            AddCommand("launch", cmdInfo);
            AddCommand("go", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = OnLaunchCommand });
            AddCommand("start", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = OnLaunchCommand });
        }


        private static string OnLaunchCommand()
        {
            if (GameObject.Find("StartGameLever") == null)
            {
                return "Can not find start lever.\n";
            }

            StartMatchLever lever = GameObject.Find("StartGameLever").GetComponent<StartMatchLever>();
            if (lever == null)
            {
                return "Can not find start lever.\n";
            }

            if ((StartOfRound.Instance.shipDoorsEnabled && !StartOfRound.Instance.shipHasLanded && !StartOfRound.Instance.shipIsLeaving) || (!StartOfRound.Instance.shipDoorsEnabled && StartOfRound.Instance.travellingToNewLevel))
            {
                return "Unable to complete action. The ship has already been launched.\n";
            }

            if(!ExtraTerminalCommandsBase.configAllowLaunchOnMoon.Value && !lever.leverHasBeenPulled)
            {
                return "Could not launch to space, you can only launch to a moon. This is due to config settings.\n";
            }

            lever.PullLever();
            lever.LeverAnimation();
            if (lever.leverHasBeenPulled)
            {
                lever.StartGame();
                return "Ship landing\n";
            }
            else
            {
                lever.EndGame();
                return "Ship launched\n";
            }
        }
    }
}
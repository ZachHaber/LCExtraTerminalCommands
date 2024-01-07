using BepInEx;
using HarmonyLib;
using System.Data.SqlClient;
using System.Reflection;
using TerminalApi;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class TimeCommand
    {
        public static void timeCommand()
        {
            CommandInfo commandInfo = new CommandInfo
            {
                Category = "other",
                Description = "Displays the current time on the planet.",
                DisplayTextSupplier = OnTimeCommand
            };

            AddCommand("time", commandInfo);
        }

        private static string OnTimeCommand()
        {
            return !StartOfRound.Instance.currentLevel.planetHasTime || !StartOfRound.Instance.shipDoorsEnabled ? "You are currently not on a moon, please try again once you are on a moon.\n" : $"The time is {HUDManager.Instance.clockNumber.text.Replace('\n', ' ')}.\n";
        }
    }
}

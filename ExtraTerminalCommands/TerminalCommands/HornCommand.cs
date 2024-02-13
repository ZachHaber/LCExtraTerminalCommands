using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
using System;
using System.Threading.Tasks;
using System.Timers;
using TerminalApi.Classes;
using UnityEngine;
using static TerminalApi.Events.Events;
using static TerminalApi.TerminalApi;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class HornCommand
    {
        public static bool isBlaring = false;
        public static string description = $"Holds the horn down for you for the next X-Amount of seconds (editable in config)";

        public static void hornCommand()
        {
            AddCommand("horn", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = returnText });
            TerminalParsedSentence += onParsedPlayerSentance;
        }
        public static string returnText()
        {
            if(isBlaring)
            {
                return "The horn is already making sound!\n";
            }
            if (ETCNetworkHandler.Instance.hornCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }
            if (GameObject.FindAnyObjectByType<ShipAlarmCord>() == null)
            {
                return "You have not yet purchased the horn.\n";
            }
            return $"The horn will continue to sound.";
        }

        private static void onParsedPlayerSentance(object sender, TerminalParseSentenceEventArgs e)
        {

            if (ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                return;
            }
            ParsedPlayerSentanceHandler.onParsedPlayerSentance(sender, e);
        }

        public static async Task onHornStandard()
        {
            if (isBlaring)
            {
                return;
            }
            if (ETCNetworkHandler.Instance.hornCmdDisabled) { return; }
                ShipAlarmCord horn = GameObject.FindAnyObjectByType<ShipAlarmCord>();
            if (horn == null) { return; }
            isBlaring = true;
            Timer timer = new Timer(ETCNetworkHandler.Instance.hornSeconds * 1000);
            timer.Elapsed += (sender, e) => OnTimerElapsedAsync(sender, e, horn);
            timer.Start();

            while (timer.Enabled)
            {
                await Task.Delay(200);
                horn.HoldCordDown();
            }
            isBlaring = false;
        }

        public static async Task onHornTimed(int seconds)
        {
            if (isBlaring)
            {
                return;
            }
            if (ETCNetworkHandler.Instance.hornCmdDisabled) { return; }
            ShipAlarmCord horn = GameObject.FindAnyObjectByType<ShipAlarmCord>();
            if (horn == null) { return; }
            isBlaring = true;
            Timer timer = new Timer(seconds * 1000);
            timer.Elapsed += (sender, e) => OnTimerElapsedAsync(sender, e, horn);
            timer.Start();

            while (timer.Enabled)
            {
                await Task.Delay(200);
                horn.HoldCordDown();
            }
            isBlaring = false;
        }

        private static async void OnTimerElapsedAsync(object sender, ElapsedEventArgs e, ShipAlarmCord horn)
        {
            ((Timer)sender).Stop();
            horn.StopHorn();
        }
    }
}

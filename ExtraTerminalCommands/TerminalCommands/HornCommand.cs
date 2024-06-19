using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
using System;
using System.Threading.Tasks;
using System.Timers;
using TerminalApi.Classes;
using UnityEngine;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class HornCommand
    {
        private const string hornSoundingText = $"The horn will continue to sound.\n\n";
        private const string hornStoppingText = $"Stopping the horn!\n\n";
        public static Timer blaringTimer = null;
        public static string description = $"Holds the horn down for you for the next X-Amount of seconds or a pre-specified time if left unset.";

        public static void hornCommand()
        {
            var commandKey = "horn";
            Commands.Add(commandKey, (string input) =>
            {
                var response = returnText();
                if (response != hornSoundingText && response != hornStoppingText)
                {
                    return response;
                }
                // input = input.Substring(commandKey.Length).Trim();
                if (input.Length == 0)
                {
                    _ = onHornTimed(0);
                }
                else
                {
                    try
                    {
                        double sec = double.Parse(input);
                        sec = Math.Min(sec, ETCNetworkHandler.Instance.hornMaxSeconds);
                        _ = onHornTimed(sec);
                    }
                    catch (Exception e)
                    {
                        ExtraTerminalCommandsBase.mls.LogError($"{e.Message}\n\nInputString: '{input}'");
                        return $"{e.Message}\n\nInputString: '{input}'\n\n";
                    }

                }
                return response;

                //return input;
            }, new CommandInfo()
            {
                Title = "Horn [X seconds]?",
                Description = description,
                Category = "Extra"
            }, Config.hornCommandAliases.Value);
        }
        public static string returnText()
        {

            if (ETCNetworkHandler.Instance.hornCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }
            ShipAlarmCord horn = getHorn();
            if (horn == null)
            {
                return "You have not yet purchased the horn.\n\n";
            }
            if (horn.otherClientHoldingCord)
            {
                return "The horn is already making sound!\n\n";
            }
            if (blaringTimer != null)
            {
                return hornStoppingText;
            }
            return hornSoundingText;
        }

        public static ShipAlarmCord getHorn()
        {
            return GameObject.FindAnyObjectByType<ShipAlarmCord>();
        }

        public static async Task onHornTimed(double seconds = 0)
        {
            if (blaringTimer != null)
            {
                StopHorn();
                return;
            }
            if (ETCNetworkHandler.Instance.hornCmdDisabled) { return; }
            seconds = seconds == 0 ? ETCNetworkHandler.Instance.hornSeconds : seconds;
            ExtraTerminalCommandsBase.mls.LogInfo($"Blaring horn for {seconds} seconds");
            ShipAlarmCord horn = getHorn();
            if (horn == null) { return; }

            blaringTimer = new Timer(seconds * 1000);
            blaringTimer.Elapsed += (sender, e) => OnTimerElapsedAsync();
            blaringTimer.Start();

            while (blaringTimer.Enabled)
            {
                await Task.Delay(200);
                horn.HoldCordDown();
            }
        }
        private static void StopHorn()
        {
            if (blaringTimer == null) return;
            blaringTimer.Stop();
            blaringTimer = null;
            ShipAlarmCord horn = getHorn();
            horn?.StopHorn();
        }

        private static void OnTimerElapsedAsync()
        {
            StopHorn();
        }
    }
}

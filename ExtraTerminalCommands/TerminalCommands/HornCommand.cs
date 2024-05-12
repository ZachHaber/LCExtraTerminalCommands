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
        private const string hornSoundingText = $"The horn will continue to sound.\n\n";
        public static bool isBlaring = false;
        public static string description = $"Holds the horn down for you for the next X-Amount of seconds or a pre-specified time if left unset.";

        public static void hornCommand()
        {
            var commandKey = "horn";
            Commands.Add(commandKey, (string input) =>
            {
                var response = returnText();
                if (response != hornSoundingText)
                {
                    return response;
                }
                input = input.Substring(commandKey.Length).Trim();
                if (input.Length == 0)
                {
                    _ = onHornStandard();
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
            });
        }
        public static string returnText()
        {
            if (isBlaring)
            {
                return "The horn is already making sound!\n\n";
            }
            if (ETCNetworkHandler.Instance.hornCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }
            if (GameObject.FindAnyObjectByType<ShipAlarmCord>() == null)
            {
                return "You have not yet purchased the horn.\n\n";
            }
            return hornSoundingText;
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

        public static async Task onHornTimed(double seconds)
        {
            if (isBlaring)
            {
                return;
            }
            if (ETCNetworkHandler.Instance.hornCmdDisabled) { return; }
            ExtraTerminalCommandsBase.mls.LogInfo($"Blaring horn for {seconds} seconds");
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

        private static void OnTimerElapsedAsync(object sender, ElapsedEventArgs e, ShipAlarmCord horn)
        {
            ((Timer)sender).Stop();
            horn.StopHorn();
        }
    }
}

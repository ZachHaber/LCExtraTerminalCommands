using System;
using System.Collections.Generic;
using System.Text;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class RadarBoosterCommands
    {
        public static string pingDescription = "Pings the current radar booster";
        public static string flashDescription = "Flashes the current radar booster";

        public static void PingCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "None",
                Description = pingDescription,
                DisplayTextSupplier = OnPingCommand
            };

            AddCommand("ping", cmdInfo);
        }
        public static void FlashCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "None",
                Description = flashDescription,
                DisplayTextSupplier = OnFlashCommand
            };

            AddCommand("flash", cmdInfo);
        }

        private static string OnPingCommand()
        {
            var targetIndex = GetCurrentRadarBooster();
            ExtraTerminalCommandsBase.mls.LogInfo($"Ping on {targetIndex}");

            if (targetIndex == -1) { return "Invalid target"; }

            StartOfRound.Instance.mapScreen.PingRadarBooster(targetIndex);

            return "Pinging\n";
        }
        private static string OnFlashCommand()
        {
            var targetIndex = GetCurrentRadarBooster();
            ExtraTerminalCommandsBase.mls.LogInfo($"Flash/Flash on {targetIndex}");
            if (targetIndex == -1) { return "Invalid target"; }

            StartOfRound.Instance.mapScreen.FlashRadarBooster(targetIndex);
            return "Flashing\n";
        }

        private static int GetCurrentRadarBooster()
        {
            int targetIndex = StartOfRound.Instance.mapScreen.targetTransformIndex;
            if (targetIndex == 0)
            {
                return -1;
            }
            return targetIndex;
        }
    }
}

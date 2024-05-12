using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class RadarBoosterCommands
    {
        public static string pingDescription = "Pings the current radar booster.";
        public static string flashDescription = "Flashes the current radar booster.";

        public static void PingCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "Extra",
                Description = pingDescription,
                DisplayTextSupplier = OnPingCommand
            };

            AddCommand("ping", cmdInfo);
        }
        public static void FlashCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "Extra",
                Description = flashDescription,
                DisplayTextSupplier = OnFlashCommand
            };

            AddCommand("flash", cmdInfo);
        }

        private static string OnPingCommand()
        {
            var targetIndex = GetCurrentRadarBooster();
            if (targetIndex <= 0) { return "Invalid target.\n\n"; }

            ExtraTerminalCommandsBase.mls.LogInfo($"Ping on {targetIndex}");
            //ExtraTerminalCommandsBase.mls.LogInfo(StartOfRound.Instance.mapScreen.radarTargets.Select((target,index) => $"{target.name} ({index})").Join(delimiter: ", "));


            StartOfRound.Instance.mapScreen.PingRadarBooster(targetIndex);
            return "Pinged radar booster.\n";
        }
        private static string OnFlashCommand()
        {
            var targetIndex = GetCurrentRadarBooster();
            if (targetIndex <= 0) { return "Invalid target.\n\n"; }
            ExtraTerminalCommandsBase.mls.LogInfo($"Flash on {targetIndex}");

            //ExtraTerminalCommandsBase.mls.LogInfo(StartOfRound.Instance.mapScreen.radarTargets.Select((target,index) => $"{target.name} ({index})").Join(delimiter: ", "));
            StartOfRound.Instance.mapScreen.FlashRadarBooster(targetIndex);
            return "Flashed radar booster.\n\n";
        }

        private static int GetCurrentRadarBooster()
        {
            int targetIndex = StartOfRound.Instance.mapScreen.targetTransformIndex;
            var targetedPlayer = StartOfRound.Instance.mapScreen.targetedPlayer;
            if (targetedPlayer!=null)
            {
                // this is likely a player - don't try and ping them
                ExtraTerminalCommandsBase.mls.LogInfo($"Ping/Flash called on Player {targetedPlayer.playerUsername} at targetTransformIndex {targetIndex}");
                return -1;
            }
            if(targetIndex < 0) {
                ExtraTerminalCommandsBase.mls.LogInfo($"Ping/Flash called with invalid targetTransformIndex: {targetIndex}");
                return -1;
            }
            return targetIndex;
        }
    }
}

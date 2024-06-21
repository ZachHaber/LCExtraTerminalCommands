using ExtraTerminalCommands.Handlers;
using ExtraTerminalCommands.Networking;
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

            Commands.AddCommandWithAliases("ping", cmdInfo, Config.pingCommandAliases.Value, null, ETCNetworkHandler.Instance?.pingCmdDisabled ?? Config.configPingCommand.Value);
        }
        public static void FlashCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "Extra",
                Description = flashDescription,
                DisplayTextSupplier = OnFlashCommand
            };

            Commands.AddCommandWithAliases("flash", cmdInfo, Config.flashCommandAliases.Value, null, ETCNetworkHandler.Instance?.flashCmdDisabled ?? Config.configFlashCommand.Value);
        }

        private static string OnPingCommand()
        {
            if (ETCNetworkHandler.Instance.pingCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }
            var targetIndex = GetCurrentRadarBooster();
            if (targetIndex <= 0) { return "Invalid target.\n\n"; }

            ExtraTerminalCommandsBase.mls.LogInfo($"Ping on {targetIndex}");
            //ExtraTerminalCommandsBase.mls.LogInfo(StartOfRound.Instance.mapScreen.radarTargets.Select((target,index) => $"{target.name} ({index})").Join(delimiter: ", "));


            StartOfRound.Instance.mapScreen.PingRadarBooster(targetIndex);
            return "Pinged radar booster.\n\n";
        }
        private static string OnFlashCommand()
        {
            if (ETCNetworkHandler.Instance.flashCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }
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
            if (targetedPlayer != null)
            {
                // this is likely a player - don't try and ping them
                ExtraTerminalCommandsBase.mls.LogInfo($"Ping/Flash called on Player {targetedPlayer.playerUsername} at targetTransformIndex {targetIndex}");
                return -1;
            }
            if (targetIndex < 0)
            {
                ExtraTerminalCommandsBase.mls.LogInfo($"Ping/Flash called with invalid targetTransformIndex: {targetIndex}");
                return -1;
            }
            return targetIndex;
        }
    }
}

using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using UnityEngine;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using BepInEx.Logging;
using JetBrains.Annotations;
using ExtraTerminalCommands.Networking;
using ExtraTerminalCommands.Handlers;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class IntroSongCommand : NetworkBehaviour
    {
        public static string description = "Plays a song that reminds you what you are working towards!";
        public static void introSongCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "Extra",
                Description = description,
                DisplayTextSupplier = PlayIntro
            };

            Commands.AddCommandWithAliases("intro", cmdInfo, Config.introSongCommandAliases.Value, null, ETCNetworkHandler.Instance?.introCmdDisabled ?? Config.configIntroSongCommand.Value);
        }

        public static string PlayIntro()
        {
            if (ETCNetworkHandler.Instance.introCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }

            if (ETCNetworkHandler.Instance.introPlaying) { return "Song is already playing, please try again once it has stopped playing\n\n"; }

            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                ETCNetworkHandler.Instance.PlayIntroSongClientRpc();
            }
            else
            {
                ETCNetworkHandler.Instance.PlayIntroSongServerRpc();
            }
            return "Now playing a banger song!\n\n";
        }
    }
}

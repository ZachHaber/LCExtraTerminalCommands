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

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class IntroSongCommand : NetworkBehaviour
    {
        public static string description = "Plays the intro song.";
        public void introSongCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "none",
                Description = description,
                DisplayTextSupplier = PlayIntro
            };

            AddCommand("intro", cmdInfo);
            AddCommand("song", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = PlayIntro });
            AddCommand("introsong", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = PlayIntro });
            AddCommand("intro-song", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = PlayIntro });
            AddCommand("intro song", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = PlayIntro });
            AddCommand("great asset", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = PlayIntro });
            AddCommand("greatasset", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = PlayIntro });
            AddCommand("ga", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = PlayIntro });
        }

        public string PlayIntro()
        {
            if (ETCNetworkHandler.Instance.introCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }

            if (ETCNetworkHandler.Instance.introPlaying) { return "Song is already playing, please try again once it has stopped playing\n"; }

            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                ETCNetworkHandler.Instance.PlayIntroSongClientRpc();
            }
            else
            {
                ETCNetworkHandler.Instance.PlayIntroSongServerRpc();
            }
            return "Now playing a banger song!\n";
        }
    }
}

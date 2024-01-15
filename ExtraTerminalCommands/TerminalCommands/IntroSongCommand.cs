using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using UnityEngine;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using BepInEx.Logging;
using JetBrains.Annotations;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class IntroSongCommand : NetworkBehaviour
    {
        ManualLogSource mls;
        public static string description = "Plays the intro song.";
        public void introSongCommand()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource("Beauver.ExtraTerminalCommands");

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
        /*
         * Changes that I want to be made to this:
         * - Everyone on the server will hear the intro song play, not only the person who wrote the command
         * - Can only run once
         
        [ServerRpc(RequireOwnership = false)]
        private void PlayIntroServerRpc()
        {
            mls.LogInfo("Called intro play ServerRPC");
            PlayIntroClientRpc();
        }

        [ClientRpc]
        private void PlayIntroClientRpc()
        {
            mls.LogInfo("Called intro play ClientRPC");
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            GameObject speakerAudio = GameObject.Find("SpeakerAudio");
            if (startOfRound == null || speakerAudio == null)
            {
                mls.LogError("Could not find speakers");
                return;
            }
            speakerAudio.GetComponent<AudioSource>().PlayOneShot(startOfRound.shipIntroSpeechSFX);
        }

        public string PlayIntro()
        {
            mls.LogInfo("Ran play intro");
            if (IsServer || IsHost) { PlayIntroClientRpc(); } else { PlayIntroServerRpc(); }
            return "Now playing a banger song!\n";
        }
        */

        public string PlayIntro()
        {
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            GameObject speakerAudio = GameObject.Find("SpeakerAudio");
            if (startOfRound == null || speakerAudio == null)
            {
                mls.LogError("Could not find speakers");
                return "Could not find speakers";
            }
            speakerAudio.GetComponent<AudioSource>().PlayOneShot(startOfRound.shipIntroSpeechSFX);
            return "Now playing a banger song!";
        }
    }
}

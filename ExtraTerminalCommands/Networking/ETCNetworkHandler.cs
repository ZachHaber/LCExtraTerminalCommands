using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

namespace ExtraTerminalCommands.Networking
{
    public class ETCNetworkHandler : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            LevelEvent = null;

            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
                Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
            Instance = this;

            base.OnNetworkSpawn();
        }

        [ClientRpc]
        public void EventClientRpc(string eventName)
        {
            // If the event has subscribers (does not equal null), invoke the event
            if (LevelEvent != null)
            {
                LevelEvent(eventName);
            }
        }

        public static event Action<String> LevelEvent;

        public static ETCNetworkHandler Instance { get; private set; }


        //Networking Variables:
        public bool introPlaying = false;
        public bool randomMoonCommandRan = false;

        //Networking Functions:

        //IntroSongCommand
        [ServerRpc(RequireOwnership = false)]
        public void PlayIntroSongServerRpc()
        {
            ExtraTerminalCommandsBase.mls.LogInfo("Called play intro ServerRPC");
            PlayIntroSongClientRpc();
        }

        [ClientRpc]
        public void PlayIntroSongClientRpc()
        {
            ExtraTerminalCommandsBase.mls.LogInfo("Called intro play ClientRPC");
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            GameObject speakerAudio = GameObject.Find("SpeakerAudio");
            if (startOfRound == null || speakerAudio == null)
            {
                ExtraTerminalCommandsBase.mls.LogError("Could not find speakers");
                return;
            }
            speakerAudio.GetComponent<AudioSource>().PlayOneShot(startOfRound.shipIntroSpeechSFX);
            introPlaying = true;
            startIntroTimer();
            return;
        }
        private async Task startIntroTimer()
        {
            await Task.Delay(38200);
            introPlaying = false;
        }

        //RandomMoonCommand
        [ServerRpc(RequireOwnership =false)]
        public void unknownPlanetServerRpc()
        {
            unknownPlanetClientRpc();
        }
        [ClientRpc]
        public void unknownPlanetClientRpc()
        {
            randomMoonCommandRan = true;
            undoRandomMoonCmd();
        }
        private async Task undoRandomMoonCmd()
        {
            await Task.Delay(6000);
            randomMoonCommandRan = false;
        }
    }
}


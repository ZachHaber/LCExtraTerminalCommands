using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace ExtraTerminalCommands.Networking
{
    public class ETCNetworkHandler : NetworkBehaviour
    {

        public override void OnNetworkSpawn()
        {
            LevelEvent = null;

            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                if (Instance != null)
                {
                    Instance.gameObject.GetComponent<NetworkObject>().Despawn();
                    ExtraTerminalCommandsBase.mls.LogWarning("Despawned network object. Don't fear, if you just started a level, this may happen.");
                }
            }
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

        public bool extraCmdDisabled = Config.configExtraCommandsList.Value;
        public bool timeCmdDisabled = Config.configTimeCommand.Value;
        public bool launchCmdDisabled = Config.configLaunchCommand.Value;
        public bool tpCmdDisabled = Config.configTeleportCommand.Value;
        public bool tpPlayerCmdDisabled = Config.configTeleportPlayerCommand.Value;
        public bool itpCmdDisabled = Config.configInverseTeleportCommand.Value;
        public bool flashCmdDisabled = Config.configFlashCommand.Value;
        public bool pingCmdDisabled = Config.configPingCommand.Value;
        public bool lightCmdDisabled = Config.configLightsCommand.Value;
        public bool doorCmdDisabled = Config.configDoorsCommand.Value;
        public bool introCmdDisabled = Config.configIntroSongCommand.Value;
        public bool randomCmdDisabled = Config.configRandomMoonCommand.Value;
        public bool clearCmdDisabled = Config.configClearCommand.Value;
        public bool switchCmdDisabled = Config.configSwitchCommand.Value;
        public bool hornCmdDisabled = Config.configHornCommand.Value;

        public bool allowWeatherFilter = Config.configAllowRandomWeatherFilter.Value;
        public bool allowHidePlanet = Config.configHidePlanet.Value;
        public int randomMoonPrice = Config.configRandomCommandPrice.Value;
        public bool allowLaunchOnMoon = Config.configAllowLaunchOnMoon.Value;
        public int hornSeconds = Config.configHornDefaultseconds.Value;
        public int hornMaxSeconds = Config.configHornMaxSeconds.Value;

        //Networking Functions:

        //Sync important things on join:
        [ServerRpc(RequireOwnership = false)]
        public void syncVariablesServerRpc()
        {
            syncVariablesClientRpc(extraCmdDisabled, timeCmdDisabled, launchCmdDisabled, tpCmdDisabled, itpCmdDisabled,
                lightCmdDisabled, doorCmdDisabled, introCmdDisabled, randomCmdDisabled, clearCmdDisabled, switchCmdDisabled,
                hornCmdDisabled,
                allowWeatherFilter, allowHidePlanet, randomMoonPrice, allowLaunchOnMoon, hornSeconds, hornMaxSeconds,
                tpPlayerCmdDisabled, flashCmdDisabled, pingCmdDisabled);
        }

        [ClientRpc]
        public void syncVariablesClientRpc(bool extraCmd, bool timeCmd, bool launchCmd, bool tpCmd, bool itpCmd, bool lightCmd,
            bool doorCmd, bool introCmd, bool randomCmd, bool clearCmd, bool switchCmd, bool hornCmd,
            bool weatherFilter, bool hidePlanet, int moonPrice, bool launchOnMoon, int hornSec, int hornMaxSec,
            bool tpPlayerCmd, bool flashCmd, bool pingCmd)
        {
            randomMoonPrice = moonPrice;
            extraCmdDisabled = extraCmd;
            timeCmdDisabled = timeCmd;
            launchCmdDisabled = launchCmd;
            tpCmdDisabled = tpCmd;
            itpCmdDisabled = itpCmd;
            tpPlayerCmdDisabled = tpPlayerCmd;
            flashCmdDisabled = flashCmd;
            pingCmdDisabled = pingCmd;
            lightCmdDisabled = lightCmd;
            doorCmdDisabled = doorCmd;
            introCmdDisabled = introCmd;
            randomCmdDisabled = randomCmd;
            clearCmdDisabled = clearCmd;
            switchCmdDisabled = switchCmd;
            hornCmdDisabled = hornCmd;

            allowWeatherFilter = weatherFilter;
            allowHidePlanet = hidePlanet;
            randomMoonPrice = moonPrice;
            allowLaunchOnMoon = launchOnMoon;
            hornSeconds = hornSec;
            hornMaxSeconds = hornMaxSec;
            ExtraTerminalCommandsBase.mls.LogInfo($"Synced variables with host.");
            ExtraTerminalCommandsBase.RegisterCommands();
        }

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

        [ClientRpc]
        public void canceledSongClientRpc()
        {
            introPlaying = false;
            introTimerCancellation?.Cancel();
        }

        [ServerRpc(RequireOwnership = false)]
        public void canceledSongServerRpc()
        {
            canceledSongClientRpc();
        }
        private CancellationTokenSource introTimerCancellation;
        public async Task startIntroTimer()
        {
            try
            {
                await Task.Delay(38200, introTimerCancellation.Token);
                introPlaying = false;
            }
            catch (TaskCanceledException)
            {
                introPlaying = false;
            }
        }


        //RandomMoonCommand
        [ServerRpc(RequireOwnership = false)]
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


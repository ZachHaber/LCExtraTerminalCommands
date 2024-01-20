using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

namespace ExtraTerminalCommands.Networking
{
    public class ETCNetworkHandler : NetworkBehaviour
    {
        public static event Action<String> LevelEvent;

        public static ETCNetworkHandler Instance { get; private set; }


        //Networking Variables:
        public bool introPlaying = false;
        public bool randomMoonCommandRan = false;

        public bool extraCmdDisabled = ExtraTerminalCommandsBase.configExtraCommandsList.Value;
        public bool timeCmdDisabled = ExtraTerminalCommandsBase.configTimeCommand.Value;
        public bool launchCmdDisabled = ExtraTerminalCommandsBase.configLaunchCommand.Value;
        public bool tpCmdDisabled = ExtraTerminalCommandsBase.configTeleportCommand.Value;
        public bool itpCmdDisabled = ExtraTerminalCommandsBase.configInverseTeleportCommand.Value;
        public bool lightCmdDisabled = ExtraTerminalCommandsBase.configLightsCommand.Value;
        public bool doorCmdDisabled = ExtraTerminalCommandsBase.configDoorsCommand.Value;
        public bool introCmdDisabled = ExtraTerminalCommandsBase.configIntroSongCommand.Value;
        public bool randomCmdDisabled = ExtraTerminalCommandsBase.configRandomMoonCommand.Value;
        public bool clearCmdDisabled = ExtraTerminalCommandsBase.configClearCommand.Value;

        public bool allowWeatherFilter = ExtraTerminalCommandsBase.configAllowRandomWeatherFilter.Value;
        public bool allowHidePlanet = ExtraTerminalCommandsBase.configHidePlanet.Value;
        public int randomMoonPrice = ExtraTerminalCommandsBase.configRandomCommandPrice.Value;
        public bool allowLaunchOnMoon = ExtraTerminalCommandsBase.configAllowLaunchOnMoon.Value;


        //Networking Functions:

        //Sync important things on join:
        [ServerRpc(RequireOwnership = true)]
        public void syncVariablesServerRpc()
        {
            syncVariablesClientRpc(extraCmdDisabled, timeCmdDisabled, launchCmdDisabled, tpCmdDisabled, itpCmdDisabled,
                lightCmdDisabled, doorCmdDisabled, introCmdDisabled, randomCmdDisabled, clearCmdDisabled, allowWeatherFilter, allowHidePlanet,
                randomMoonPrice, allowLaunchOnMoon);
        }

        [ClientRpc]
        public void syncVariablesClientRpc(bool extraCmd, bool timeCmd, bool launchCmd, bool tpCmd, bool itpCmd, bool lightCmd,
            bool doorCmd, bool introCmd, bool randomCmd, bool clearCmd, bool weatherFilter, bool hidePlanet, int moonPrice, bool launchOnMoon)
        {
            randomMoonPrice = moonPrice;
            extraCmdDisabled = extraCmd;
            timeCmdDisabled = timeCmd;
            launchCmdDisabled = launchCmd;
            tpCmdDisabled = tpCmd;
            itpCmdDisabled = itpCmd;
            lightCmdDisabled = lightCmd;
            doorCmdDisabled = doorCmd;
            introCmdDisabled = introCmd;
            randomCmdDisabled = randomCmd;
            clearCmdDisabled = clearCmd;

            allowWeatherFilter = weatherFilter;
            allowHidePlanet = hidePlanet;
            randomMoonPrice = moonPrice;
            allowLaunchOnMoon = launchOnMoon;
            ExtraTerminalCommandsBase.mls.LogInfo($"Synced variables with host.");
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


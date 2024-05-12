using ExtraTerminalCommands.Networking;
using ExtraTerminalCommands.TerminalCommands;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Linq;
using TerminalApi.Classes;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

namespace ExtraTerminalCommands.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("SetMapScreenInfoToCurrentLevel")]
        public static void HideMapScreenInfo(VideoPlayer ___screenLevelVideoReel, TextMeshProUGUI ___screenLevelDescription)
        {
            if (ETCNetworkHandler.Instance.allowHidePlanet && ETCNetworkHandler.Instance.randomMoonCommandRan)
            {
                ___screenLevelDescription.text = "Orbiting: Unkown\nPopulation: Unknown\nConditions: Unknown\nFauna: Unknown\nWeather: Unknown";
                ___screenLevelVideoReel.enabled = false;
                ___screenLevelVideoReel.clip = null;
                ___screenLevelVideoReel.gameObject.SetActive(value: false);
                ___screenLevelVideoReel.Stop();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("DisableShipSpeaker")]
        public static void DisableShipSpeaker()
        {
            if(!ETCNetworkHandler.Instance.introPlaying)
            {
                return;
            }

            if(NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                ETCNetworkHandler.Instance.canceledSongClientRpc();
            }
            else
            {
                ETCNetworkHandler.Instance.canceledSongServerRpc();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnClientConnect")]
        public static void onClientConnectPatch()
        {

            ETCNetworkHandler NH = ETCNetworkHandler.Instance;
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                ETCNetworkHandler.Instance.syncVariablesClientRpc(NH.extraCmdDisabled, NH.timeCmdDisabled, NH.launchCmdDisabled,
                    NH.tpCmdDisabled, NH.itpCmdDisabled, NH.lightCmdDisabled, NH.doorCmdDisabled, NH.introCmdDisabled,
                    NH.randomCmdDisabled, NH.clearCmdDisabled, NH.switchCmdDisabled, NH.hornCmdDisabled,
                    NH.allowWeatherFilter, NH.allowHidePlanet, NH.randomMoonPrice, NH.allowLaunchOnMoon, NH.hornSeconds, NH.hornMaxSeconds,
                    NH.tpPlayerCmdDisabled, NH.flashCmdDisabled, NH.pingCmdDisabled);
            }
            else
            {
                ETCNetworkHandler.Instance.syncVariablesServerRpc();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnLocalDisconnect")]
        public static void onLocalDisconnectPatch()
        {
            ExtraTerminalCommandsBase.daysJoined = 0;
        }

        [HarmonyPostfix]
        [HarmonyPatch("openingDoorsSequence")]
        public static void openingDoorsSequencePatch()
        {
            ExtraTerminalCommandsBase.daysJoined++;
        }
    }
}

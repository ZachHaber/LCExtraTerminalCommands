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

                    NH.allowWeatherFilter, NH.allowHidePlanet, NH.randomMoonPrice, NH.allowLaunchOnMoon, NH.hornSeconds, NH.hornMaxSeconds);
            }
            else
            {
                ETCNetworkHandler.Instance.syncVariablesServerRpc();
            }
            addSwitchCmd();
            addTeleportCmd();
        }
        [HarmonyPostfix, HarmonyPatch("Start")]
        public static void onStartup()
        {
            addSwitchCmd();
            addTeleportCmd();
            addHornCmd();
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

        public static void addSwitchCmd()
        {
            if (!ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                GameNetworkManager[] gameNetworkManagers = GameObject.FindObjectsOfType<GameNetworkManager>();

                foreach (GameNetworkManager playerNetwork in gameNetworkManagers)
                {
                    TerminalApi.TerminalApi.AddCommand("sw " + playerNetwork.username, new CommandInfo {Category = "none", Description = SwitchCommand.description, DisplayTextSupplier = SwitchCommand.returnText });
                    TerminalApi.TerminalApi.AddCommand("s " + playerNetwork.username, new CommandInfo { Category = "none", Description = SwitchCommand.description, DisplayTextSupplier = SwitchCommand.returnText });
                    ExtraTerminalCommandsBase.mls.LogInfo("Added command: s/sw " + playerNetwork.username);
                }
            }
        }

        public static void addTeleportCmd()
        {
            if (!ETCNetworkHandler.Instance.switchCmdDisabled)
            {
                GameNetworkManager[] gameNetworkManagers = GameObject.FindObjectsOfType<GameNetworkManager>();

                foreach (GameNetworkManager playerNetwork in gameNetworkManagers)
                {
                    TerminalApi.TerminalApi.AddCommand("tp " + playerNetwork.username, new CommandInfo { Category = "none", Description = TeleportCommand.description, DisplayTextSupplier = TeleportCommand.OnTeleportCommand});
                    //TerminalApi.TerminalApi.AddCommand("tp " + playerNetwork., new CommandInfo { Category = "none", Description = SwitchCommand.description, DisplayTextSupplier = SwitchCommand.returnText });
                    ExtraTerminalCommandsBase.mls.LogInfo("Added command: tp " + playerNetwork.username);
                }
            }
        }

        public static void addHornCmd()
        {
            if (!ETCNetworkHandler.Instance.hornCmdDisabled)
            {
                for (int i = 0; i <= ETCNetworkHandler.Instance.hornMaxSeconds; i++)
                {
                    TerminalApi.TerminalApi.AddCommand("horn " + i, new CommandInfo { Category = "none", Description = HornCommand.description, DisplayTextSupplier = HornCommand.returnText });
                }
            }
        }
    }
}

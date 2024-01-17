using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace ExtraTerminalCommands.Networking
{
    [HarmonyPatch]
    public class NetworkObjectManager
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Start))]
        public static void Init()
        {
            if (networkPrefab != null)
                return;

            networkPrefab = (GameObject) ExtraTerminalCommandsBase.MainAssetBundle.LoadAsset("Assets/AssetsBundlesWanted/NetworkHandler.prefab");
            networkPrefab.AddComponent<ETCNetworkHandler>();

            NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
            ExtraTerminalCommandsBase.mls.LogInfo("Loaded NetworkHandler");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake))]
        static void SpawnNetworkHandler()
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                var networkHandlerHost = Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
                networkHandlerHost.GetComponent<NetworkObject>().Spawn();
                ExtraTerminalCommandsBase.mls.LogInfo("Spawned NetworkHandler");
            }
        }

        static GameObject networkPrefab;
    }
}

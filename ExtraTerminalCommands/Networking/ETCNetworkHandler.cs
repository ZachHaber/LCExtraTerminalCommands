using System;
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
                Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
            Instance = this;

            base.OnNetworkSpawn();
        }

        [ClientRpc]
        public void EventClientRpc(string eventName)
        {
            LevelEvent?.Invoke(eventName); // If the event has subscribers (does not equal null), invoke the event
        }

        public static event Action<String> LevelEvent;

        public static ETCNetworkHandler Instance { get; private set; }
    }
}


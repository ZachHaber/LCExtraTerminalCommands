using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalApi.Classes;
using Unity.Netcode;
using UnityEngine;
using static TerminalApi.TerminalApi;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class TestRPC : NetworkBehaviour
    {
        public static string description = "Test commands";
        public void testRpc()
        {
            AddCommand("testrpc", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = testCmd });
        }

        private string testCmd()
        {
            if (!NetworkManager.Singleton.IsListening && NetworkManager.Singleton == null)
            {
                return "Network manager doesn't exist or is not listening!";
            }
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                TestClientRpc();
                return "Ran Client\n";
            }
            else
            {
                TestServerRpc();
                return "Ran Server\n";
            }
            
        }

        [ServerRpc(RequireOwnership = false)]
        private void TestServerRpc()
        {
            ExtraTerminalCommandsBase.mls.LogInfo("Test ServerRpc ran");
            Debug.Log("Test ServerRpc ran");
        }

        [ClientRpc]
        private void TestClientRpc()
        {
            ExtraTerminalCommandsBase.mls.LogInfo("Test ClientRpc ran");
            Debug.Log("Test ClientRpc ran");
        }
    }
}

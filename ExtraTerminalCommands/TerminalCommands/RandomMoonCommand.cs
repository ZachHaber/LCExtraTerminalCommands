using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalApi.Classes;
using UnityEngine;
using Unity.Netcode;
using static TerminalApi.TerminalApi;
using UnityEngine.Video;
using TMPro;
using ExtraTerminalCommands.Networking;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class RandomMoonCommand
    {
        public static string description = "Sends you to a random moon. Use [Weather/Money/Both] to blacklist moons that contain this.";
        public static void randomMoonCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "none",
                Description = description,
                DisplayTextSupplier = onRandomMoonNoFilter
            };

            if (ETCNetworkHandler.Instance.allowWeatherFilter)
            {
                AddCommand("r w", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = onRandomMoonWeather });
                AddCommand("r weather", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = onRandomMoonWeather });
                AddCommand("random weather", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = onRandomMoonWeather });
            }

            AddCommand("random", cmdInfo);
            AddCommand("r", new CommandInfo { Category = "none", Description = description, DisplayTextSupplier = onRandomMoonNoFilter });
        }

        private static string onRandomMoonNoFilter()
        {
            if (ETCNetworkHandler.Instance.randomCmdDisabled)
            {
                return "This command is disabled by the host.\n";
            }

            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            if (startOfRound.shipDoorsEnabled)
            {
                return "You are currently on a moon. Can not travel to a random moon.\n";
            }
            
            List<SelectableLevel> moons = new List<SelectableLevel>();
            foreach (SelectableLevel moon in terminal.moonsCatalogueList)
            {
                moons.Add(moon);
            }
            return goToRandomPlanet(moons);
        }

        private static string onRandomMoonWeather()
        {
            if (!ETCNetworkHandler.Instance.allowWeatherFilter)
            {
                return "This command is disabled by the host.\n";
            }

            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            if (startOfRound.shipDoorsEnabled)
            {
                return "You are currently on a moon. Can not travel to a random moon.\n";
            }

            List<SelectableLevel> moons = new List<SelectableLevel>();
            foreach (SelectableLevel moon in terminal.moonsCatalogueList)
            {
                if(moon.currentWeather != LevelWeatherType.None) { continue; }
                moons.Add(moon);
            }
            return goToRandomPlanet(moons);
        }

        private static string goToRandomPlanet(List<SelectableLevel> moons)
        {
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            System.Random rnd = new System.Random();

            int travelPrice = ETCNetworkHandler.Instance.randomMoonPrice;
            if(travelPrice < 0) {  travelPrice = 0; }
            if (terminal.groupCredits - travelPrice < 0)
            {
                return $"Could not go to a random moon, you have too little money, you need atleast: {travelPrice} credits.\n";
            }
                
            int randomMoonNum = rnd.Next(0, moons.Count - 1);

            if (startOfRound.IsHost || startOfRound.IsServer)
            {
                startOfRound.ChangeLevelClientRpc(moons[randomMoonNum].levelID, terminal.groupCredits - travelPrice);
                ETCNetworkHandler.Instance.unknownPlanetClientRpc();
            }
            else
            {
                startOfRound.ChangeLevelServerRpc(moons[randomMoonNum].levelID, terminal.groupCredits - travelPrice);
                ETCNetworkHandler.Instance.unknownPlanetServerRpc();
            }
            
            return "Traveled to ???\n";
        }
    }
}

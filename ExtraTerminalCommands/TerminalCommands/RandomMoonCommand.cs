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
        public static string description = "Sends you to a random moon. Use [Weather] to blacklist moons that contain this.";
        public static void randomMoonCommand()
        {
            var command = "random";
            CommandInfo cmdInfo = new CommandInfo
            {
                Title = $"{command.ToUpper()} [WEATHER]?",
                Category = "Extra",
                Description = description,
                DisplayTextSupplier = onRandomMoonNoFilter
            };

            if (Config.configAllowRandomWeatherFilter.Value)
            {
                AddCommand("random weather", new CommandInfo { Category = "None", Description = description, DisplayTextSupplier = onRandomMoonWeather });
            }

            AddCommand(command, cmdInfo);
        }

        private static string onRandomMoonNoFilter()
        {
            if(ETCNetworkHandler.Instance.randomCmdDisabled)
            {
                return "This command is disabled by the host.\n\n";
            }

            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            if (startOfRound.shipDoorsEnabled)
            {
                return "You are currently on a moon. Can not travel to a random moon\n\n";
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
                return "This filter is disabled by the host.\n\n";
            }

            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            if (startOfRound.shipDoorsEnabled)
            {
                return "You are currently on a moon. Can not travel to a random moon.\n\n";
            }

            List<SelectableLevel> moons = new List<SelectableLevel>();
            foreach (SelectableLevel moon in terminal.moonsCatalogueList)
            {
                if (moon.currentWeather != LevelWeatherType.None) { continue; }
                moons.Add(moon);
            }
            return goToRandomPlanet(moons);
        }
        /*
        private static string onRandomMoonMoney()
        {
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            if (startOfRound.shipDoorsEnabled || !startOfRound.currentLevel.planetHasTime)
            {
                return "You are currently on a moon. Can not travel to a random moon.\n\n";
            }

            List<SelectableLevel> moons = new List<SelectableLevel>();
            foreach (SelectableLevel moon in terminal.moonsCatalogueList)
            {
                //change to price
                if (moon.currentWeather != LevelWeatherType.None) { continue; }
                moons.Add(moon);
            }
            return goToRandomPlanet(moons);
        }

        private static string onRandomMoonBoth()
        {
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            if (startOfRound.shipDoorsEnabled || !startOfRound.currentLevel.planetHasTime)
            {
                return "You are currently on a moon. Can not travel to a random moon.\n\n";
            }

            List<SelectableLevel> moons = new List<SelectableLevel>();
            foreach (SelectableLevel moon in terminal.moonsCatalogueList)
            {
                //add price cancelation
                if (moon.currentWeather != LevelWeatherType.None) { continue; }
                moons.Add(moon);
            }
            return goToRandomPlanet(moons);
        }
        */

        private static string goToRandomPlanet(List<SelectableLevel> moons)
        {
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            System.Random rnd = new System.Random();

            int travelPrice = Config.configRandomCommandPrice.Value;
            if (travelPrice < 0) { travelPrice = 0; }
            if (terminal.groupCredits - travelPrice < 0)
            {
                return $"Could not go to a random moon, you have too little money, you need atleast: {travelPrice} credits.\n\n";
            }

            int randomMoonNum = rnd.Next(0, moons.Count);

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

            return "Traveled to ???\n\n";
        }
    }
}

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
using ExtraTerminalCommands.Handlers;


namespace ExtraTerminalCommands.TerminalCommands
{
    internal class RandomMoonCommand
    {
        public static string description = "Sends you to a random moon. Use `random Weather` to blacklist moons with weather.";
        public static void randomMoonCommand()
        {
            var command = "random";
            Commands.Add(command, (input) =>
            {
                if (ETCNetworkHandler.Instance.randomCmdDisabled)
                {
                    return "This command is disabled by the host.\n\n";
                }
                if (input == "weather")
                {
                    return onRandomMoonWeather();
                }
                else if (input != "")
                {
                    return $"Invalid option: '{input}'. option can only be 'weather'\n\n";
                }
                return onRandomMoonNoFilter();
            }, new CommandInfo()
            {
                Title = "random weather?",
                Category = "Extra",
                Description = description,
            }, Config.randomCommandAliases.Value, ETCNetworkHandler.Instance?.randomCmdDisabled ?? Config.configRandomMoonCommand.Value);
        }

        private static string getInvalidTravelText(StartOfRound startOfRound)
        {
            if (startOfRound.shipDoorsEnabled)
            {
                return "You are currently on a moon. Can not travel to a random moon\n\n";
            }
            if (startOfRound.travellingToNewLevel)
            {
                return "You are currently traveling to a moon\n\n";
            }
            return null;
        }

        private static string onRandomMoonNoFilter()
        {
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            var invalid = getInvalidTravelText(startOfRound);
            if (invalid != null)
            {
                return invalid;
            }

            List<SelectableLevel> moons = [.. terminal.moonsCatalogueList];
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
            var invalid = getInvalidTravelText(startOfRound);
            if (invalid != null)
            {
                return invalid;
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

            SelectableLevel moon = moons[randomMoonNum];

            ExtraTerminalCommandsBase.mls.LogDebug($"Traveling to Random Moon: {moon.PlanetName} - Weather {moon.currentWeather}");

            if (startOfRound.IsHost || startOfRound.IsServer)
            {
                startOfRound.ChangeLevelClientRpc(moon.levelID, terminal.groupCredits - travelPrice);
                ETCNetworkHandler.Instance.unknownPlanetClientRpc();
            }
            else
            {
                startOfRound.ChangeLevelServerRpc(moon.levelID, terminal.groupCredits - travelPrice);
                ETCNetworkHandler.Instance.unknownPlanetServerRpc();
            }


            return "Traveled to ???\n\n";
        }
    }
}

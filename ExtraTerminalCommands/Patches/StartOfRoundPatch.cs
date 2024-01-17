using ExtraTerminalCommands.Networking;
using HarmonyLib;
using TMPro;
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
            if (ExtraTerminalCommandsBase.configHidePlanet.Value && ETCNetworkHandler.Instance.randomMoonCommandRan)
            {
                ___screenLevelDescription.text = "Orbiting: Unkown\nPopulation: Unknown\nConditions: Unknown\nFauna: Unknown\nWeather: Unknown";
                ___screenLevelVideoReel.enabled = false;
                ___screenLevelVideoReel.clip = null;
                ___screenLevelVideoReel.gameObject.SetActive(value: false);
                ___screenLevelVideoReel.Stop();
            }
        }
    }
}

using ExtraTerminalCommands.Networking;
using ExtraTerminalCommands.TerminalCommands;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using static TerminalApi.Events.Events;
using static TerminalApi.TerminalApi;

namespace ExtraTerminalCommands.Handlers
{
    internal class ParsedPlayerSentanceHandler
    {

        public static void onParsedPlayerSentance(object sender, TerminalParseSentenceEventArgs e)
        {
            string userInput = GetTerminalInput();
            string[] userInputParts = userInput.Split(' ');

            if (userInputParts.Length == 1)
            {
                if (userInputParts[0] == "s" || userInputParts[0] == "sw")
                {
                    SwitchCommand.switchNormal();
                    return;
                }
                else if (userInputParts[0] == "horn")
                {
                    _ = HornCommand.onHornStandard();
                    return;
                }
            }
            else if (userInputParts.Length == 2)
            {
                if (userInputParts[0] == "s" || userInputParts[0] == "sw")
                {
                    SwitchCommand.switchInput(userInputParts);
                    return;
                }
                else if (userInputParts[0] == "horn")
                {
                    int sec;
                    if(!Int32.TryParse(userInputParts[1], out sec)) { return; }
                    if (sec > ETCNetworkHandler.Instance.hornMaxSeconds) { return; }
                    _ = HornCommand.onHornTimed(sec);
                    return;
                }
            }
        }
    }
}

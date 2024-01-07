using System;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using UnityEngine;

namespace ExtraTerminalCommands.TerminalCommands
{
    internal class IntroSongCommand
    {
        public static void introSongCommand()
        {
            CommandInfo cmdInfo = new CommandInfo
            {
                Category = "other",
                Description = "Plays the intro song",
                DisplayTextSupplier = onIntroSong
            };

            AddCommand("intro", cmdInfo);
            AddCommand("song", new CommandInfo { Category = "none", Description = "Plays the intro song", DisplayTextSupplier = onIntroSong });
            AddCommand("introsong", new CommandInfo { Category = "none", Description = "Plays the intro song", DisplayTextSupplier = onIntroSong });
            AddCommand("intro-song", new CommandInfo { Category = "none", Description = "Plays the intro song", DisplayTextSupplier = onIntroSong });
            AddCommand("intro song", new CommandInfo { Category = "none", Description = "Plays the intro song", DisplayTextSupplier = onIntroSong });
            AddCommand("great asset", new CommandInfo { Category = "none", Description = "Plays the intro song", DisplayTextSupplier = onIntroSong });
            AddCommand("greatasset", new CommandInfo { Category = "none", Description = "Plays the intro song", DisplayTextSupplier = onIntroSong });
        }

        private static string onIntroSong()
        {
            StartOfRound startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            Terminal terminal = GameObject.FindObjectOfType<Terminal>();
            if (terminal == null || startOfRound == null)
            {
                return "Could not find terminal or current round... But that doesn't make sense does it?\n";
            }
            terminal.terminalAudio.PlayOneShot(startOfRound.shipIntroSpeechSFX);
            return "Now playing a banger song!\n";
        }
    }
}

using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using Keys;

namespace MidiPlayground
{
    class Program
    {
        private static Keyboard keyboard = new Keyboard();

        private static List<IKeyAction> KeyActions = new List<IKeyAction>();

        static void Main(string[] args)
        {
            var chords = new Dictionary<string, List<Keys.Key>>();
            Chords.GetChordForEveryChromaticNote((noteName, i) =>
            {
                chords.Add(noteName + " Root Position", Chords.GenerateChord(i, Constants.TriadMajorRootPos));
                chords.Add(noteName + " Minor Root Position", Chords.GenerateChord(i, Constants.TriadMinorRootPos));

                //chords.Add(noteName + " 1st Inversion", Chords.GenerateChord(i, Constants.TriadMajor1stInv));
                //chords.Add(noteName + " Minor 1st Inversion", Chords.GenerateChord(i, Constants.TriadMinor1stInv));

                //chords.Add(noteName + " 2nd Inversion", Chords.GenerateChord(i, Constants.TriadMajor2ndInv));
                //chords.Add(noteName + " Minor 2nd Inversion", Chords.GenerateChord(i, Constants.TriadMinor2ndInv));

                //chords.Add(noteName + "Seventh Root Position", Chords.GenerateChord(i, Constants.SeventhMajorRootPos));
                //chords.Add(noteName + "Minor Seventh Root Position", Chords.GenerateChord(i, Constants.SeventhMinorRootPos));

                //chords.Add(noteName + "Seventh 1st Inversion", Chords.GenerateChord(i, Constants.SeventhMajor1stInv));
                //chords.Add(noteName + "Minor Seventh 1st Inversion", Chords.GenerateChord(i, Constants.SeventhMinor1stInv));

                //chords.Add(noteName + "Seventh Root 2nd Inversion", Chords.GenerateChord(i, Constants.SeventhMajor2ndInv));
                //chords.Add(noteName + "Minor Seventh 2nd Inversion", Chords.GenerateChord(i, Constants.SeventhMinor2ndInv));

                //chords.Add(noteName + "Seventh 3rd Inversion", Chords.GenerateChord(i, Constants.SeventhMajor3rdInv));
                //chords.Add(noteName + "Minor Seventh 3rd Inversion", Chords.GenerateChord(i, Constants.SeventhMinor3rdInv));
            });

            if (InputDevice.DeviceCount < 1)
            {
                Console.WriteLine("No MIDI device found. Connect device and restart.");
                return;
            }

            int deviceId = 0;
            Console.WriteLine("Using Device: " + InputDevice.GetDeviceCapabilities(deviceId).name);
            using (InputDevice inDevice = new InputDevice(deviceId))
            {
                keyboard = new Keyboard(inDevice);
                keyboard.StartRecording(inDevice, KeyActions);

                var rand = new Random();

                while (true)
                {
                    int chordIdx = rand.Next(0, chords.Count);
                    var chord = chords.ElementAt(chordIdx);

                    Console.WriteLine($"Play {chord.Key}");
                    var playAction = new PlayChord(chord.Value);
                    do
                    {
                        playAction.WaitForKeyInput(KeyActions);
                        Console.WriteLine(playAction.Played ? "You did it!" : "Nope...");
                    }
                    while (!playAction.Played);
                }
            }
        }

        private static void OutputCurrentPlayedNotes()
        {
            Console.WriteLine(string.Join(" ", keyboard.OnKeys.Select(k => $"{k.Name}{k.Register}")));
        }
    }
}

using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Keys;
using Sanford.Multimedia;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MidiPlayground
{
    class Program
    {
        private static Keyboard keyboard = new Keyboard();

        private static List<IKeyAction> KeyActions = new List<IKeyAction>();

        static void Main(string[] args)
        {
            if(InputDevice.DeviceCount < 1)
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
                    int chordIdx = rand.Next(0, Chords.ChordMaps.Count);
                    var chord = Chords.ChordMaps.ElementAt(chordIdx);

                    Console.WriteLine($"Play {chord.Key}");
                    var playAction = new PlayChordAction(chord.Value);
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

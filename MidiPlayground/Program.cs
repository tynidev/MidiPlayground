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

namespace MidiPlayground
{
    class Program
    {
        private static Keyboard keyboard = new Keyboard();

        private static List<IKeyAction> KeyActions = new List<IKeyAction>();

        static void Main(string[] args)
        {
            Console.WriteLine("Select Input device");
            for(int i = 0; i < InputDevice.DeviceCount; i++)
            {
                var caps = InputDevice.GetDeviceCapabilities(i);
                Console.WriteLine($"{i + 1}: {caps.name}");
            }

            var device = (int)Console.ReadKey().KeyChar - 49;

            if(device < 0 || device >= InputDevice.DeviceCount)
            {
                throw new ArgumentOutOfRangeException(nameof(device));
            }

            using (InputDevice inDevice = new InputDevice(0))
            {
                keyboard = new Keyboard(inDevice);

                inDevice.ChannelMessageReceived += delegate (object sender, ChannelMessageEventArgs e)
                {
                    var command = e.Message.Command;
                    var key = e.Message.Data1;

                    switch (command)
                    {
                        case ChannelCommand.NoteOn:
                            keyboard[key].On = true;
                            break;
                        case ChannelCommand.NoteOff:
                            keyboard[key].On = false;
                            break;
                    }

                    foreach (var foo in KeyActions)
                        foo.Action(keyboard, keyboard[key], command);
                };

                var rand = new Random();
                inDevice.StartRecording();

                while (true)
                {
                    int chordIdx = rand.Next(0, Chords.ChordMaps.Count);
                    var chord = Chords.ChordMaps.ElementAt(chordIdx);

                    Console.WriteLine($"Play {chord.Key}");

                    var playAction = new PlayChordAction(chord.Value);
                    KeyActions.Add(playAction);
                    while (!playAction.ready) { Thread.Sleep(10); };
                    KeyActions.Clear();

                    Console.WriteLine(playAction.played ? "You did it!" : "Nope...");
                }
            }
        }

        private static void OutputCurrentPlayedNotes()
        {
            Console.WriteLine(string.Join(" ", keyboard.OnKeys.Select(k => $"{k.Name}{k.Register}")));
        }
    }
}

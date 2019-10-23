using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using Keys;
using System.Diagnostics;
using System.IO;
using MidiKeyboard;

namespace MidiPlayground
{
    class Card
    {
        public string Prompt = "";
        public int Weight = 1500;
        public int Picked = 0;
    }

    /*
verovio.exe test.xml -r ../../data -o test.svg --adjust-page-height --page-width=500
inkscape.exe --without-gui --file test.svg --export-png=test.png
     * */

    class Program
    {
        private static Keyboard keyboard = new Keyboard();

        private static List<IKeyAction> KeyActions = new List<IKeyAction>();

        static void Main(string[] args)
        {
            List<ScaleNote> group = new List<ScaleNote>()
            {
                new ScaleNote(){interval = 0, register = 4},
                new ScaleNote(){interval = 3, register = 4},
                new ScaleNote(){interval = 5, register = 5},
            };

            var cMajor = new Scale(Note.c);
            var gMinor = new Scale(Note.g, mode: Mode.Minor);

            var notes = "";
            foreach(var ns in cMajor.SelectNotes(group))
            {
                var a = Accidental.n;
                var n = cMajor.GetNoteName(ns.interval, out a);
                notes += a != Accidental.n ? n.ToString() + a.ToString() : n.ToString();
                notes += " ";
            }
            Console.WriteLine(notes);
            Console.WriteLine();

            notes = "";
            foreach (var ns in gMinor.SelectNotes(cMajor.SelectNotes(group)))
            {
                var a = Accidental.n;
                var n = gMinor.GetNoteName(ns.interval, out a);
                notes += a != Accidental.n ? n.ToString() + a.ToString() : n.ToString();
                notes += " ";
            }
            Console.WriteLine(notes);
            Console.WriteLine();

            Console.ReadKey();

            foreach (var key in new List<Scale>() { new Scale(Note.b, Accidental.s, Mode.Major) } )
            {
                for(int i = 0; i < 8; i++)
                {
                    var a = Accidental.n;
                    var n = key.GetNoteName(i, out a);

                    notes += a != Accidental.n ? n.ToString() + a.ToString() : n.ToString();
                    notes += " ";
                }
                Console.WriteLine(notes);
                Console.WriteLine();
            }
            Console.ReadKey();

            var cMajorTrebble = new List<int>()
            {
                Key.ToPitch((int)Note.c, 4),
                Key.ToPitch((int)Note.e, 4),
                Key.ToPitch((int)Note.g, 4),
            };
            var cMajorClef = new List<int>()
            {
                Key.ToPitch((int)Note.c, 3),
                Key.ToPitch((int)Note.e, 3),
                Key.ToPitch((int)Note.g, 3),
            };

            var xml = OutputMei.Song("", 4, 4, "0", "major", 
                OutputMei.Measure(1, 
                    OutputMei.Chord(cMajorTrebble, OutputMei.NoteValue.Quarter), 
                    OutputMei.Chord(cMajorClef, OutputMei.NoteValue.Quarter)));

            File.WriteAllText(@"C:\Users\tyni\Desktop\test.xml", xml);

            var chords = new Dictionary<string, List<Key>>();

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

                List<Card> cards = new List<Card>();
                foreach(var c in chords)
                {
                    cards.Add(new Card() { Prompt = c.Key });
                }

                while (true)
                {
                    Card card = pick(cards);
                    card.Picked++;

                    Console.WriteLine($"Play {card.Prompt}");
                    var playAction = new PlayChord(chords[card.Prompt]);
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    do
                    {
                        playAction.WaitForKeyInput(KeyActions);
                        Console.WriteLine(playAction.Played ? "You did it!" : "Nope...");
                    }
                    while (!playAction.Played);
                    watch.Stop();
                    card.Weight = (((card.Picked - 1) * card.Weight) + (int)watch.ElapsedMilliseconds) / (card.Picked);
                }
            }
        }

        private static Card pick(List<Card> cards)
        {
            Shuffle(cards);

            int sumOfWeights = 0;
            foreach (var c in cards)
                sumOfWeights += c.Weight;

            int randomWeight = rng.Next(1, sumOfWeights);

            foreach (var c in cards)
            {
                randomWeight -= c.Weight;
                if (randomWeight <= 0)
                    return c;
            }

            throw new Exception("Should not get here");
        }

        private static Random rng = new Random((int)DateTime.Now.Ticks);

        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static void OutputCurrentPlayedNotes()
        {
            Console.WriteLine(string.Join(" ", keyboard.OnKeys.Select(k => $"{k.Name}{k.Register}")));
        }
    }
}

using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using Keys;
using System.Diagnostics;
using MidiKeyboard;
using FlashCards;

namespace MidiPlayground
{
    /*
verovio.exe test.xml -r ../../data -o test.svg --adjust-page-height --page-width=500
inkscape.exe --without-gui --file test.svg --export-png=test.png
     * */

    class Program
    {
        private static MidiKeyboard.MidiKeyboard keyboard = new MidiKeyboard.MidiKeyboard();

        private static List<IKeyAction> KeyActions = new List<IKeyAction>();

        private static string GetNoteName(Keys.Key key, int i)
        {
            var a = Accidental.n;
            var n = key.GetSheetNoteName(i, out a);

            var mod = "";
            switch (a)
            {
                case Accidental.s:
                    mod = "#";
                    break;
                case Accidental.x:
                    mod = "x";
                    break;
                case Accidental.f:
                    mod = "-";
                    break;
                case Accidental.ff:
                    mod = "--";
                    break;
            }
            return n.ToString() + mod;
        }

        private static string GetNoteNames(Keys.Key key, List<int> notes)
        {
            string name = "";
            for(int i = 0; i < notes.Count(); i++)
            {
                name += GetNoteName(key, notes[i]) + " ";
            }
            return name;
        }

        static void Main(string[] args)
        {
            var circle = new CircleOf5ths();
            //var cMajor = new Scale(Note.c);
            //var gMinor = new Scale(Note.g, mode: Mode.Minor);

            //var notes = "";
            //foreach(var ns in cMajor.SelectNotes(group))
            //{
            //    var a = Accidental.n;
            //    var n = cMajor.GetNoteName(ns.interval, out a);
            //    notes += a != Accidental.n ? n.ToString() + a.ToString() : n.ToString();
            //    notes += " ";
            //}
            //Console.WriteLine(notes);
            //Console.WriteLine();

            //notes = "";
            //foreach (var ns in gMinor.SelectNotes(cMajor.SelectNotes(group)))
            //{
            //    var a = Accidental.n;
            //    var n = gMinor.GetNoteName(ns.interval, out a);
            //    notes += a != Accidental.n ? n.ToString() + a.ToString() : n.ToString();
            //    notes += " ";
            //}
            //Console.WriteLine(notes);
            //Console.WriteLine();

            //Console.ReadKey();
            //var k = circle[0];
            //k.Mode = Mode.Minor;

            //var aChord = k.SelectNotes(new List<KeyNote>()
            //{
            //    new KeyNote(){ interval = 1 },
            //    new KeyNote(){ interval = 3 },
            //    new KeyNote(){ interval = 5 },
            //});

            //Console.WriteLine(GetNoteNames(k, aChord.Select(o => o.interval).ToList()));

            //foreach (var key in circle)
            //{
            //    key.Mode = Mode.Ionian;
            //    Console.WriteLine(GetNoteNames(key, key.Select(o => o.Interval).ToList()));
            //    key.Mode = Mode.Dorian;
            //    Console.WriteLine(GetNoteNames(key, key.Select(o => o.Interval).ToList()));
            //    key.Mode = Mode.Phrygian;
            //    Console.WriteLine(GetNoteNames(key, key.Select(o => o.Interval).ToList()));
            //    key.Mode = Mode.Lydian;
            //    Console.WriteLine(GetNoteNames(key, key.Select(o => o.Interval).ToList()));
            //    key.Mode = Mode.Mixolydian;
            //    Console.WriteLine(GetNoteNames(key, key.Select(o => o.Interval).ToList()));
            //    key.Mode = Mode.Aeolian;
            //    Console.WriteLine(GetNoteNames(key, key.Select(o => o.Interval).ToList()));
            //    key.Mode = Mode.Locrian;
            //    Console.WriteLine(GetNoteNames(key, key.Select(o => o.Interval).ToList()));

            //    Console.WriteLine();
            //}
            //Console.ReadKey();

            //var cMajorTrebble = new List<int>()
            //{
            //    MidiKeyboard.Key.ToPitch((int)Note.c, 4),
            //    MidiKeyboard.Key.ToPitch((int)Note.e, 4),
            //    MidiKeyboard.Key.ToPitch((int)Note.g, 4),
            //};
            //var cMajorClef = new List<int>()
            //{
            //    MidiKeyboard.Key.ToPitch((int)Note.c, 3),
            //    MidiKeyboard.Key.ToPitch((int)Note.e, 3),
            //    MidiKeyboard.Key.ToPitch((int)Note.g, 3),
            //};

            //var xml = OutputMei.Song("", 4, 4, "0", "major", 
            //    OutputMei.Measure(1, 
            //        OutputMei.Chord(cMajorTrebble, OutputMei.NoteValue.Quarter), 
            //        OutputMei.Chord(cMajorClef, OutputMei.NoteValue.Quarter)));

            //File.WriteAllText(@"C:\Users\tyni\Desktop\test.xml", xml);


            List<Note> triad = new List<Note>()
            {
                new Note(){Interval = 1},
                new Note(){Interval = 3},
                new Note(){Interval = 5},
            };

            Deck deck = new Deck();
            foreach (var scale in circle)
            {
                scale.Mode = Mode.Major;
                deck.Add(new Card(
                    new KeyValuePair<string, List<int>>
                    (
                        GetNoteName(scale, 1).ToUpper(),
                        scale.Transpose(triad)
                         .Select(n => (int)n.RelativePitch)
                         .ToList()
                    )));
                
                scale.Mode = Mode.Minor;
                deck.Add(new Card(
                    new KeyValuePair<string, List<int>>
                    (
                        GetNoteName(scale, 1).ToUpper() + $"m",
                        scale.Transpose(triad)
                             .Select(n => (int)n.RelativePitch)
                             .ToList()
                    )));
            }

            if (InputDevice.DeviceCount < 1)
            {
                Console.WriteLine("No MIDI device found. Connect device and restart.");
                return;
            }
            for(int i = 0; i < InputDevice.DeviceCount; i++)
            {
                Console.WriteLine($"{i + 1}: {InputDevice.GetDeviceCapabilities(i).name}");
            }
            var key = Console.ReadKey();

            int deviceId = 0;
            // We check input for a Digit
            if (char.IsDigit(key.KeyChar))
            {
                deviceId = int.Parse(key.KeyChar.ToString()) -1; // use Parse if it's a Digit
            }

            Console.WriteLine("Using Device: " + InputDevice.GetDeviceCapabilities(deviceId).name);
            using (InputDevice inDevice = new InputDevice(deviceId))
            {
                keyboard = new MidiKeyboard.MidiKeyboard(inDevice);
                keyboard.StartRecording(inDevice, KeyActions);

                while (true)
                {
                    Card card = deck.Pick();
                    var kvp = (KeyValuePair<string,List<int>>)card.Get();

                    Console.WriteLine($"Play {kvp.Key}");
                    var playAction = new PlayChord(kvp.Value);
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    do
                    {
                        playAction.WaitForKeyInput(KeyActions);
                        Console.WriteLine(playAction.Played ? "You did it!" : "Nope...");
                    }
                    while (!playAction.Played);
                    watch.Stop();
                }
            }
        }
    }
}

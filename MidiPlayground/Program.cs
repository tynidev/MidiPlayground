﻿using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using Keys;
using System.Diagnostics;
using Sanford.Multimedia;
using System.IO;

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
            var k = new Scale(1, 0, Scales.Major); // C Flat
            k.GetNoteName(6);

            foreach (var key in Scales.Keys)
            {
                string notes = "";
                for(int i = 0; i < 8; i++)
                {
                    notes += key.GetNoteName(i) + " ";
                }
                Console.WriteLine(notes);
                Console.WriteLine();
            }
            Console.ReadKey();

            var cMajorTrebble = new List<Keys.Key>()
            {
                new Keys.Key(Note.C, 4),
                new Keys.Key(Note.E, 4),
                new Keys.Key(Note.G, 4),
            };
            var cMajorClef = new List<Keys.Key>()
            {
                new Keys.Key(Note.C, 3),
                new Keys.Key(Note.E, 3),
                new Keys.Key(Note.G, 3),
            };

            var xml = OutputMei.Song("", 4, 4, "0", "major", 
                OutputMei.Measure(1, 
                    OutputMei.Chord(cMajorTrebble, OutputMei.NoteValue.Quarter), 
                    OutputMei.Chord(cMajorClef, OutputMei.NoteValue.Quarter)));

            File.WriteAllText(@"C:\Users\tyni\Desktop\test.xml", xml);

            var chords = new Dictionary<string, List<Keys.Key>>();
            Generator.ForEveryChromaticNote((string noteName, int midiNoteValue) =>
            {
                chords.Add(noteName + "", Generator.GenerateChord(midiNoteValue, Constants.TriadMajorRootPos));
                chords.Add(noteName + " Minor", Generator.GenerateChord(midiNoteValue, Constants.TriadMinorRootPos));

                //chords.Add(noteName + " 1st Inversion", Generator.GenerateChord(midiNoteValue, Constants.TriadMajor1stInv));
                //chords.Add(noteName + " Minor 1st Inversion", Generator.GenerateChord(midiNoteValue, Constants.TriadMinor1stInv));

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

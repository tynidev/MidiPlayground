using Sanford.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Keys
{
    public class Chords
    {
        public static Dictionary<string, List<Key>> GetChordForEveryChromaticNote(Action<string, int> group, int register = 4)
        {
            var chords = new Dictionary<string, List<Key>>();

            // Generate all major chords
            int startNote = register * 12;
            int endNote = startNote + 12;
            for(int i = startNote; i < endNote; i++)
            {
                var note = Key.ToNote(i);

                string noteName = note.ToString();

                if (note == Note.DFlat) noteName = "CSharp";
                if (note == Note.DSharp) noteName = "EFlat";
                if (note == Note.GFlat) noteName = "FSharp";
                if (note == Note.GSharp) noteName = "AFlat";
                if (note == Note.ASharp) noteName = "BFlat";

                group(noteName, i);
            }

            return chords;
        }

        public static List<Key> GenerateChord(int rootAbsolute, List<int> semitones)
        {
            semitones.Sort();

            var list = new SortedList<int, Key>();

            for(int i = 0; i < semitones.Count; i++)
            {
                list.Add(rootAbsolute + semitones[i], new Key(rootAbsolute + semitones[i]));
            }

            return list.Values.ToList();
        }
    }
}

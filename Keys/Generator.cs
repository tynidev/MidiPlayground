using System;
using System.Collections.Generic;
using System.Linq;

namespace Keys
{
    public class Generator
    {
        public static void ForEveryChromaticNote(Action<string, int> GenerateSequence, int register = 4)
        {
            int startNote = register * 12;
            int endNote = startNote + 12;
            for(int i = startNote; i < endNote; i++)
            {
                var note = Key.ToNote(i);

                string noteName = note.ToString();

                if (note == Note.dFlat) noteName = "CSharp";
                if (note == Note.dSharp) noteName = "EFlat";
                if (note == Note.ff) noteName = "FSharp";
                if (note == Note.gs) noteName = "AFlat";
                if (note == Note.@as) noteName = "BFlat";

                GenerateSequence(noteName, i);
            }
        }

        public static List<Key> GenerateScale(int rootAbsolute, List<int> semitones)
        {
            semitones.Sort();

            var list = new SortedList<int, Key>();

            for (int i = 0; i < semitones.Count; i++)
            {
                list.Add(rootAbsolute + semitones[i], new Key(rootAbsolute + semitones[i]));
            }

            return list.Values.ToList();
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

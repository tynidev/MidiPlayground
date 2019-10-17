using Sanford.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public class Chords
    {
        public static Dictionary<string, SortedList<int, Key>> ChordMaps
        {
            get
            {
                var chords = new Dictionary<string, SortedList<int, Key>>();

                // Generate all major chords
                for(int i = 60; i < 60 + 12; i++)
                {
                    var note = Key.ToNote(i);

                    string noteName = note.ToString();

                    if (note == Note.DFlat) noteName = "CSharp";
                    if (note == Note.DSharp) noteName = "EFlat";
                    if (note == Note.GFlat) noteName = "FSharp";
                    if (note == Note.GSharp) noteName = "AFlat";
                    if (note == Note.ASharp) noteName = "BFlat";

                    chords.Add(noteName + " Major", GenerateMajorTriadChordRootPosition(i));

                    chords.Add(noteName + " Minor", GenerateMinorTriadChordRootPosition(i));
                }

                return chords;
            }
        }

        public static SortedList<int, Key> GenerateMajorTriadChordRootPosition(int i)
        {
            var list = new SortedList<int, Key>();

            // add root
            list.Add(i, new Key(i));

            // add 3rd
            list.Add(i + 4, new Key(i + 4));

            // add 5th
            list.Add(i + 7, new Key(i + 7));

            return list;
        }

        public static SortedList<int, Key> GenerateMinorTriadChordRootPosition(int i)
        {
            var list = new SortedList<int, Key>();

            // add root
            list.Add(i, new Key(i));

            // add 3rd
            list.Add(i + 3, new Key(i + 3));

            // add 5th
            list.Add(i + 7, new Key(i + 7));

            return list;
        }
    }
}

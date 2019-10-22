using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public enum Accidental
    {
        s =  1,
        f = -1,
        n =  0,
    }

    public class ScaleNote
    {
        public int register = 4;
        public char name;
        public int SemitonesFromRoot;
        public Accidental accidental = Accidental.n;
    }

    public class Scale
    {
        public Scale(int offset, int noteOffset, int[] key, int register = 4)
        {
            this.RootOffset = offset;
            this.NoteOffset = noteOffset;
            Notes = new ScaleNote[7];
            var names = new char[] { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };
            int pitch = 0;
            for(int i = 0; i < 7; i++)
            {
                pitch += key[i];
                var noteNameIdx = (noteOffset + i) % 7;
                Notes[i] = new ScaleNote()
                {
                    name = names[noteNameIdx],
                    SemitonesFromRoot = pitch,
                    register = register
                };
            }
        }

        public readonly int NoteOffset;
        public readonly int RootOffset;
        public readonly ScaleNote[] Notes;

        private ScaleNote GetNote(int interval, Accidental accidental, int register = -1)
        {
            int length = Notes.Length;
            var note = Notes[interval % length];

            return new ScaleNote()
            {
                register = register >= 0 ? register : note.register + interval / length,
                name = note.name,
                SemitonesFromRoot = note.SemitonesFromRoot + (int)accidental,
                accidental = accidental
            };
        }

        public string GetNoteName(int interval)
        {
            interval = interval % 7;
            int[] CMajor = new int[] { 0, 2, 4, 5, 7, 9, 11, 12};
            int semitonesFromC = (Notes[interval].SemitonesFromRoot + RootOffset) % 12;

            int intervalInC = (interval + NoteOffset) % 7;
            if (semitonesFromC == 11 && intervalInC == 0)
                intervalInC = 7;

            string sharpFlat = string.Empty;
            if (semitonesFromC > CMajor[intervalInC])
                sharpFlat = "+";
            else if (semitonesFromC < CMajor[intervalInC])
                sharpFlat = "-";

            if (RootOffset == 1 && NoteOffset == 0 && interval == 6 && Notes[interval].SemitonesFromRoot == 11)
                sharpFlat = "+";

            return $"{Notes[interval].name}{sharpFlat}";
        }
    }

    public class Scales
    {
        public static int[] Major = new int[] { 0, 2, 2, 1, 2, 2, 2 };
        public static int[] Minor = new int[] { 0, 2, 1, 2, 2, 1, 2 };

        public static Scale[] MajorKeys = new Scale[] {
            new Scale ( 0, 0, Major), // C
            new Scale ( 7, 4, Major), // G
            new Scale ( 2, 1, Major), // D
            new Scale ( 9, 5, Major), // A
            new Scale ( 4, 2, Major), // E
            new Scale (11, 6, Major), // B
            new Scale ( 5, 3, Major), // F
            new Scale (10, 6, Major), // B Flat
            new Scale ( 3, 2, Major), // E Flat
            new Scale ( 8, 5, Major), // A Flat
            new Scale ( 1, 1, Major), // D Flat
            new Scale ( 6, 4, Major), // G Flat
            new Scale ( 6, 3, Major), // F Sharp
            new Scale ( 1, 0, Major), // C Sharp
            new Scale (-1, 0, Major), // C Flat
        };

        public static Scale[] MinorKeys = new Scale[] {
            new Scale ( 9, 5, Minor), // a
            new Scale ( 4, 2, Minor), // e
            new Scale (11, 6, Minor), // b
            new Scale ( 6, 3, Minor), // f#
            new Scale ( 1, 0, Minor), // c#
            new Scale ( 8, 4, Minor), // g#
            new Scale ( 2, 1, Minor), // d
            new Scale ( 7, 4, Minor), // g
            new Scale ( 0, 0, Minor), // c
            new Scale ( 5, 3, Minor), // f
            new Scale (10, 6, Minor), // b Flat
            new Scale ( 3, 2, Minor), // e flat
            new Scale ( 3, 1, Minor), // d#
        };

        public static Scale[] Keys = new Scale[] {
            new Scale ( 0, 0, Major), // C
            new Scale ( 9, 5, Minor), // a

            new Scale ( 7, 4, Major), // G
            new Scale ( 4, 2, Minor), // e

            new Scale ( 2, 1, Major), // D
            new Scale (11, 6, Minor), // b

            new Scale ( 9, 5, Major), // A
            new Scale ( 6, 3, Minor), // f #

            new Scale ( 4, 2, Major), // E
            new Scale ( 1, 0, Minor), // c #

            new Scale (11, 6, Major), // B
            new Scale ( 8, 4, Minor), // g #

            new Scale ( 5, 3, Major), // F
            new Scale ( 2, 1, Minor), // d

            new Scale (10, 6, Major), // B Flat
            new Scale ( 7, 4, Minor), // g

            new Scale ( 3, 2, Major), // E Flat
            new Scale ( 0, 0, Minor), // c

            new Scale ( 8, 5, Major), // A Flat
            new Scale ( 5, 3, Minor), // f

            new Scale ( 1, 1, Major), // D Flat
            new Scale (10, 6, Minor), // b Flat

            new Scale ( 6, 4, Major), // G Flat
            new Scale ( 3, 2, Minor), // e flat

            new Scale ( 6, 3, Major), // F Sharp
            new Scale ( 3, 1, Minor), // d #

            new Scale ( 1, 0, Major), // C #
            new Scale (-1, 0, Major), // C Flat
        };
    }
}

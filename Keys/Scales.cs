using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public enum Accidental
    {
        s =   1,
        x =   2,
        f =  -1,
        ff = -2,
        n =   0,
    }

    public enum Note
    {
        C = 0,
        CSharp = 1,
        DFlat = 1,
        D = 2,
        DSharp = 3,
        EFlat = 3, 
        E = 3,
        F = 5,
        FSharp = 6,
        GFlat = 6,
        G = 7,
        GSharp = 8,
        AFlat = 8,
        A = 9,
        ASharp = 10,
        BFlat = 10,
        B = 11
    }

    public enum Mode
    {
        Major,
        Minor
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
        private static int[] Major = new int[] { 0, 2, 2, 1, 2, 2, 2, 1 };
        private static int[] Minor = new int[] { 0, 2, 1, 2, 2, 1, 2, 2 };

        public Scale(Note note, Accidental accidental, Mode mode, int register = 4)
        {
            int offset = (int)note + (int)accidental;
            var noteOffsets = new Dictionary<Note, int>()
            {
                { Note.C, 0 },
                { Note.D, 1 },
                { Note.E, 2 },
                { Note.F, 3 },
                { Note.G, 4 },
                { Note.A, 5 },
                { Note.B, 6 },
            };

            this.init(offset, noteOffsets[note], mode == Mode.Major ? Major : Minor, register);
        }

        private void init(int offset, int noteOffset, int[] key, int register = 4)
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

        public int NoteOffset;
        public int RootOffset;
        public ScaleNote[] Notes;

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

        public char GetNoteName(int intervalFromRoot, out Accidental accidental)
        {
            int[] CMajor = new int[] { 0, 2, 4, 5, 7, 9, 11, 12 };

            intervalFromRoot = intervalFromRoot % 7;
            int semitonesFromRoot = Notes[intervalFromRoot].SemitonesFromRoot;

            int intervalInC = (intervalFromRoot + NoteOffset) % 7;
            int semitonesFromC = (semitonesFromRoot + RootOffset) % 12;

            // Handle the two wrap around cases where we need to compare against top C
            if (intervalInC == 0 && semitonesFromC == 11)
                intervalInC = 7;
            if (intervalInC == 6 && semitonesFromC == 0)
                semitonesFromC = 12;

            accidental = (Accidental)(semitonesFromC - CMajor[intervalInC]);
            return Notes[intervalFromRoot].name;
        }
    }

    public class Scales
    {
        public static Scale[] MajorKeys = new Scale[] {
            new Scale (Note.C, Accidental.n, Mode.Major),
            new Scale (Note.G, Accidental.n, Mode.Major),
            new Scale (Note.D, Accidental.n, Mode.Major),
            new Scale (Note.A, Accidental.n, Mode.Major),
            new Scale (Note.E, Accidental.n, Mode.Major),
            new Scale (Note.B, Accidental.n, Mode.Major),
            new Scale (Note.F, Accidental.n, Mode.Major),
            new Scale (Note.B, Accidental.f, Mode.Major),
            new Scale (Note.E, Accidental.f, Mode.Major),
            new Scale (Note.A, Accidental.f, Mode.Major),
            new Scale (Note.D, Accidental.f, Mode.Major),
            new Scale (Note.G, Accidental.f, Mode.Major),
            new Scale (Note.F, Accidental.s, Mode.Major),
            new Scale (Note.C, Accidental.s, Mode.Major),
            new Scale (Note.C, Accidental.f, Mode.Major),
        };

        public static Scale[] MinorKeys = new Scale[] {
            new Scale (Note.A, Accidental.n, Mode.Minor),
            new Scale (Note.E, Accidental.n, Mode.Minor),
            new Scale (Note.B, Accidental.n, Mode.Minor),
            new Scale (Note.F, Accidental.s, Mode.Minor),
            new Scale (Note.C, Accidental.s, Mode.Minor),
            new Scale (Note.G, Accidental.s, Mode.Minor),
            new Scale (Note.D, Accidental.n, Mode.Minor),
            new Scale (Note.G, Accidental.n, Mode.Minor),
            new Scale (Note.C, Accidental.n, Mode.Minor),
            new Scale (Note.F, Accidental.n, Mode.Minor),
            new Scale (Note.B, Accidental.f, Mode.Minor),
            new Scale (Note.E, Accidental.f, Mode.Minor),
            new Scale (Note.D, Accidental.s, Mode.Minor),
        };
    }
}

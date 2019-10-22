using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

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
        c = 0,
        cSharp = 1,
        dFlat = 1,
        d = 2,
        dSharp = 3,
        eFlat = 3, 
        e = 3,
        f = 5,
        fSharp = 6,
        ff = 6,
        g = 7,
        gs = 8,
        af = 8,
        a = 9,
        @as = 10,
        bf = 10,
        b = 11
    }

    public enum Mode
    {
        Major,
        Minor
    }

    public class ScaleNote
    {
        public char name;
        public int SemitonesFromRoot;

        public int interval = 0;
        public int register = 4;
        public Accidental accidental = Accidental.n;
    }

    public class Scale
    {
        private static int[] Major = new int[] { 0, 2, 2, 1, 2, 2, 2, 1 };
        private static int[] Minor = new int[] { 0, 2, 1, 2, 2, 1, 2, 2 };

        public Scale(Note note, Accidental accidental, Mode mode)
        {
            int offset = (int)note + (int)accidental;
            var noteOffsets = new Dictionary<Note, int>()
            {
                { Note.c, 0 },
                { Note.d, 1 },
                { Note.e, 2 },
                { Note.f, 3 },
                { Note.g, 4 },
                { Note.a, 5 },
                { Note.b, 6 },
            };

            this.init(offset, noteOffsets[note], mode == Mode.Major ? Major : Minor);
        }

        private void init(int offset, int noteOffset, int[] key)
        {
            this.RootOffset = offset;
            this.NoteOffset = noteOffset;
            Notes = new ScaleNote[7];
            var names = new char[7] { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };
            int pitch = 0;
            for(int i = 0; i < 7; i++)
            {
                pitch += key[i];
                var noteNameIdx = (noteOffset + i) % 7;
                Notes[i] = new ScaleNote()
                {
                    name = names[noteNameIdx],
                    SemitonesFromRoot = pitch,
                    interval = i
                };
            }
        }

        private int NoteOffset;
        private int RootOffset;
        private ScaleNote[] Notes;

        public ScaleNote SelectNote(ScaleNote noteToSelect)
        {
            int length = Notes.Length;
            var note = Notes[noteToSelect.interval % length];

            return new ScaleNote()
            {
                name = note.name,
                SemitonesFromRoot = note.SemitonesFromRoot + (int)noteToSelect.accidental,

                register = noteToSelect.register + noteToSelect.interval / length,
                interval = noteToSelect.interval,
                accidental = noteToSelect.accidental
            };
        }

        public List<ScaleNote> SelectNotes(List<ScaleNote> notesToSelect)
        {
            return notesToSelect.Select(n => SelectNote(n)).ToList();
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
            new Scale (Note.c, Accidental.n, Mode.Major),
            new Scale (Note.g, Accidental.n, Mode.Major),
            new Scale (Note.d, Accidental.n, Mode.Major),
            new Scale (Note.a, Accidental.n, Mode.Major),
            new Scale (Note.e, Accidental.n, Mode.Major),
            new Scale (Note.b, Accidental.n, Mode.Major),
            new Scale (Note.f, Accidental.n, Mode.Major),
            new Scale (Note.b, Accidental.f, Mode.Major),
            new Scale (Note.e, Accidental.f, Mode.Major),
            new Scale (Note.a, Accidental.f, Mode.Major),
            new Scale (Note.d, Accidental.f, Mode.Major),
            new Scale (Note.g, Accidental.f, Mode.Major),
            new Scale (Note.f, Accidental.s, Mode.Major),
            new Scale (Note.c, Accidental.s, Mode.Major),
            new Scale (Note.c, Accidental.f, Mode.Major),
        };

        public static Scale[] MinorKeys = new Scale[] {
            new Scale (Note.a, Accidental.n, Mode.Minor),
            new Scale (Note.e, Accidental.n, Mode.Minor),
            new Scale (Note.b, Accidental.n, Mode.Minor),
            new Scale (Note.f, Accidental.s, Mode.Minor),
            new Scale (Note.c, Accidental.s, Mode.Minor),
            new Scale (Note.g, Accidental.s, Mode.Minor),
            new Scale (Note.d, Accidental.n, Mode.Minor),
            new Scale (Note.g, Accidental.n, Mode.Minor),
            new Scale (Note.c, Accidental.n, Mode.Minor),
            new Scale (Note.f, Accidental.n, Mode.Minor),
            new Scale (Note.b, Accidental.f, Mode.Minor),
            new Scale (Note.e, Accidental.f, Mode.Minor),
            new Scale (Note.d, Accidental.s, Mode.Minor),
        };
    }
}

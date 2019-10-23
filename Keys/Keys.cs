using System.Collections.Generic;
using System.Linq;

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
        cs = 1,
        dFlat = 1,
        d = 2,
        ds = 3,
        eFlat = 3, 
        e = 4,
        f = 5,
        fs = 6,
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
        Major = 0,
        Minor = 5,
    }

    public class KeyNote
    {
        public char name;
        public int SemitonesFromRoot;

        public int interval = 0;
        public int register = 4;
        public Accidental accidental = Accidental.n;
    }

    public class Key : CircularList<KeyNote>
    {
        public Mode mode = Mode.Major;
        public Mode Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
                this.position = 0;
                this.Rotate((int)this.mode);
            }
        }

        private int NoteOffset;
        private int RootOffset;

        private static int[] Major = new int[] { 0, 2, 2, 1, 2, 2, 2, 1 };

        public List<KeyNote> Notes
        {
            get { return this.list;  }
            private set { this.list = value; }
        }

        public Key(Note note, Accidental accidental = Accidental.n)
        {
            this.mode = Mode.Major;

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

            this.init(offset, noteOffsets[note], Major);
        }

        private void init(int offset, int noteOffset, int[] key)
        {
            this.RootOffset = offset;
            this.NoteOffset = noteOffset;
            Notes = new List<KeyNote>(7);
            var names = new char[7] { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };
            int pitch = 0;
            for(int i = 0; i < 7; i++)
            {
                pitch += key[i];
                var noteNameIdx = (noteOffset + i) % 7;
                Notes.Add(new KeyNote()
                {
                    name = names[noteNameIdx],
                    SemitonesFromRoot = pitch,
                    interval = i
                });
            }
        }

        public KeyNote SelectNote(KeyNote noteToSelect)
        {
            int length = Notes.Count();
            var note = Notes[(this.Position + noteToSelect.interval) % length];

            return new KeyNote()
            {
                name = note.name,
                SemitonesFromRoot = note.SemitonesFromRoot + (int)noteToSelect.accidental,

                register = noteToSelect.register + noteToSelect.interval / length,
                interval = noteToSelect.interval,
                accidental = noteToSelect.accidental
            };
        }

        public List<KeyNote> SelectNotes(List<KeyNote> notesToSelect)
        {
            return notesToSelect.Select(n => SelectNote(n)).ToList();
        }

        public char GetNoteName(int intervalFromRoot, out Accidental accidental)
        {
            int[] CMajor = new int[] { 0, 2, 4, 5, 7, 9, 11, 12 };

            intervalFromRoot = (intervalFromRoot + this.Position) % 7;
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

    public class Keys
    {
        public static CircularList<Key> CircleOf5ths = new CircularList<Key>()
        {
            new Key(Note.c),
            new Key(Note.g),
            new Key(Note.d),
            new Key(Note.a),
            new Key(Note.e),
            new Key(Note.b),
            new Key(Note.f, Accidental.s),
            new Key(Note.g, Accidental.f),
            new Key(Note.d, Accidental.f),
            new Key(Note.a, Accidental.f),
            new Key(Note.e, Accidental.f),
            new Key(Note.b, Accidental.f),
            new Key(Note.f),
        };
    }
}

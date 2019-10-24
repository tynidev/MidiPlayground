using System;
using System.Collections.Generic;
using System.Linq;

namespace Keys
{
    public enum Accidental
    {
        s =   1,
        x =   2,
        ts =  3,
        f =  -1,
        ff = -2,
        tf = -3,
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

        Ionian = 0,
        Dorian = 1,
        Phrygian = 2,
        Lydian = 3,
        Mixolydian = 4,
        Aeolian = 5,
        Locrian = 6,
    }

    public class KeyNote
    {
        public char Name;
        public int SemitonesFromRoot;
        public int RootSemitone;

        public int Interval = 0;
        public int Register = 4;

        private Accidental a = Accidental.n;
        public Accidental Accidental
        {
            get
            {
                return a;
            }
            set
            {
                this.SemitonesFromRoot += (int)value;
                this.a = value;
            }
        }

        public int AbsolutePitch
        {
            get => RootSemitone + SemitonesFromRoot + (12 * Register);
        }

        public int RelativePitch
        {
            get => (RootSemitone + SemitonesFromRoot) % 12;
        }
    }

    public class Key : CircularList<KeyNote>
    {
        private Mode mode = Mode.Major;
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

        public int NoteOffset;
        public int RootOffset;

        private static int[] Major = new int[] { 0, 2, 2, 1, 2, 2, 2, 1 };

        public override KeyNote this[int index]
        {
            get
            {
                return base[index - 1];
            }
            set
            {
                base[index - 1] = value;
            }
        }

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
                    Name = names[noteNameIdx],
                    SemitonesFromRoot = pitch,
                    RootSemitone = offset,
                    Interval = i + 1 // offset so we can have human readable intervals
                });
            }
        }

        public KeyNote Project(KeyNote project)
        {
            var note = Notes[0];
            foreach(var n in Notes)
            {
                if(Math.Abs(n.RelativePitch - project.RelativePitch) <
                   Math.Abs(note.RelativePitch - project.RelativePitch))
                {
                    note = n;
                }
            }

            var ret = new KeyNote()
            {
                Name = note.Name,
                Interval = note.Interval,
                Accidental = (Accidental)(project.RelativePitch - note.RelativePitch),

                Register = project.Register,
                RootSemitone = note.RootSemitone,
                SemitonesFromRoot = 12 - Math.Abs(note.RootSemitone - project.RelativePitch),
            };
            return ret;
        }

        public KeyNote Transpose(KeyNote noteToSelect)
        {
            int length = Notes.Count();
            var note = Notes[(this.Position + noteToSelect.Interval - 1) % length];

            int addRegister = (this.NoteOffset + noteToSelect.Interval - 1) / length;

            return new KeyNote()
            {
                Name = note.Name,
                SemitonesFromRoot = note.SemitonesFromRoot,
                RootSemitone = this.RootOffset,
                Register = noteToSelect.Register + addRegister,
                Interval = noteToSelect.Interval,
                Accidental = noteToSelect.Accidental,
            };
        }

        public List<KeyNote> Project(List<KeyNote> notesToSelect)
        {
            return notesToSelect.Select(n => Project(n)).ToList();
        }

        public List<KeyNote> Transpose(List<KeyNote> notesToSelect)
        {
            return notesToSelect.Select(n => Transpose(n)).ToList();
        }

        public char GetNoteName(int interval, out Accidental accidental)
        {
            interval -= 1; // offset so we can use human readable intervals

            int[] CMajor = new int[] { 0, 2, 4, 5, 7, 9, 11, 12 };

            interval = (interval + this.Position) % 7;
            int semitonesFromRoot = Notes[interval].SemitonesFromRoot;

            int intervalInC = (interval + NoteOffset) % 7;
            int semitonesFromC = (semitonesFromRoot + RootOffset) % 12;

            // Handle the two wrap around cases where we need to compare against top C
            if (intervalInC == 0 && semitonesFromC == 11)
                intervalInC = 7;
            if (intervalInC == 6 && semitonesFromC == 0)
                semitonesFromC = 12;

            accidental = (Accidental)(semitonesFromC - CMajor[intervalInC]);
            return Notes[interval].Name;
        }
    }

    public class CircleOf5ths : CircularList<Key>
    {
        public CircleOf5ths()
        {
            this.list = (new CircularList<Key>()
            {
                new Key(Note.c),
                
                new Key(Note.g),
                new Key(Note.d),
                new Key(Note.a),
                new Key(Note.e),
                new Key(Note.b),
                
                new Key(Note.f, Accidental.s), new Key(Note.g, Accidental.f),

                new Key(Note.d, Accidental.f),
                new Key(Note.a, Accidental.f),
                new Key(Note.e, Accidental.f),
                new Key(Note.b, Accidental.f),
                new Key(Note.f),
            }).list;
        }
    }
}

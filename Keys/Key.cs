using System;
using System.Collections.Generic;
using System.Linq;

namespace Keys
{
    public class Key : CircularList<Note>
    {
        public string KeyAccidentals = "0";

        public Semitone RootSemitone { get; private set; }

        private int noteOffset = 0;
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

        public override Note this[int index]
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

        public Key(NoteLetters rootName, Accidental rootAccidental = Accidental.n)
        {
            this.mode = Mode.Major;

            this.RootSemitone = (Semitone)(((int)rootName + (int)rootAccidental) % 12);
            var major = new CircularList<Semitone> { Semitone.c, Semitone.d, Semitone.e, Semitone.f, Semitone.g, Semitone.a, Semitone.b };

            this.noteOffset = 0;
            foreach (var name in major)
            {
                if (name == (Semitone)rootName)
                {
                    break;
                }
                this.noteOffset++;
            }

            for (int interval = 0; interval < 7; interval++)
            {
                this.Add(new Note()
                {
                    Name = (NoteLetters)major[this.noteOffset + interval],
                    SemitonesFromRoot = major[interval],
                    RootSemitone = this.RootSemitone,
                    Interval = interval + 1 // offset so we can have human readable intervals
                });
            }
        }

        /// <summary>
        /// Returns the note as it would be written in the current key.
        /// </summary>
        /// <param name="project">Note to project</param>
        /// <returns></returns>
        public Note Project(Note project)
        {
            var note = this[0];
            foreach (var n in this)
            {
                if (Math.Abs(n.RelativePitch - project.RelativePitch) <
                   Math.Abs(note.RelativePitch - project.RelativePitch))
                {
                    note = n;
                }
            }

            int newTone = (int)(project.RootSemitone - note.RootSemitone + project.SemitonesFromRoot);
            var ret = new Note()
            {
                Name = note.Name,
                Interval = note.Interval,
                Accidental = (Accidental)(project.RelativePitch - note.RelativePitch),

                Register = project.Register + (newTone / 12),
                RootSemitone = note.RootSemitone,
                SemitonesFromRoot = (Semitone)(newTone % 12),
            };

            return ret;
        }

        /// <summary>
        /// Returns the notes as they would be written in the current key
        /// </summary>
        /// <param name="project">Notes to project</param>
        /// <returns></returns>
        public List<Note> Project(List<Note> project)
        {
            return project.Select(n => Project(n)).ToList();
        }

        /// <summary>
        /// Returns the equivalent interval in the current key.
        /// </summary>
        /// <param name="transpose">Note to transpose</param>
        /// <returns></returns>
        public Note Transpose(Note transpose)
        {
            int length = this.Count();
            var note = this[transpose.Interval];

            int addRegister = (this.noteOffset + transpose.Interval - 1) / length;

            return new Note()
            {
                Name = note.Name,
                SemitonesFromRoot = note.SemitonesFromRoot,
                RootSemitone = this.RootSemitone,
                Register = transpose.Register + addRegister,
                Interval = transpose.Interval,
                Accidental = transpose.Accidental,
            };
        }

        /// <summary>
        /// Returns the equivalent intervals in the current key.
        /// </summary>
        /// <param name="transpose">Notes to transpose</param>
        /// <returns></returns>
        public List<Note> Transpose(List<Note> transpose)
        {
            return transpose.Select(n => Transpose(n)).ToList();
        }

        /// <summary>
        /// Returns the specified interval as it would be notated in sheet music.
        /// </summary>
        /// <param name="interval">Interval of note name to retrieve</param>
        /// <param name="accidental">Accidental of note returned</param>
        /// <returns></returns>
        public NoteLetters GetSheetNoteName(int interval, out Accidental accidental)
        {
            interval -= 1; // offset so we can use human readable intervals

            int[] CMajor = new int[] { 0, 2, 4, 5, 7, 9, 11, 12 };

            interval = (interval + this.Position) % 7;
            int semitonesFromRoot = (int)this.list[interval].SemitonesFromRoot;

            int intervalInC = (interval + noteOffset) % 7;
            int semitonesFromC = (semitonesFromRoot + (int)RootSemitone) % 12;

            // Handle the two wrap around cases where we need to compare against top C
            if (intervalInC == 0 && semitonesFromC == 11)
                intervalInC = 7;
            if (intervalInC == 6 && semitonesFromC == 0)
                semitonesFromC = 12;

            accidental = (Accidental)(semitonesFromC - CMajor[intervalInC]);
            return this.list[interval].Name;
        }
    }
}

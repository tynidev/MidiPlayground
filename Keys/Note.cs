using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public class Note
    {
        /// <summary>
        /// Note name
        /// </summary>
        public NoteLetters Name;

        /// <summary>
        /// Interval of scale
        /// </summary>
        public int Interval = 0;

        /// <summary>
        /// Semitone of scale root
        /// </summary>
        public Semitone RootSemitone;

        /// <summary>
        /// Semitone from scale root
        /// </summary>
        public Semitone SemitonesFromRoot;

        /// <summary>
        /// Register this note is in
        /// </summary>
        public int Register = 4;

        /// <summary>
        /// Accidental
        /// </summary>
        public Accidental Accidental
        {
            get
            {
                return a;
            }
            set
            {
                this.SemitonesFromRoot = (Semitone)((int)this.SemitonesFromRoot + (int)value);
                this.a = value;
            }
        }
        private Accidental a = Accidental.n;

        /// <summary>
        /// Absolute MIDI note value
        /// </summary>
        public Semitone AbsolutePitch
        {
            get => (Semitone)((int)RootSemitone + (int)SemitonesFromRoot + (12 * Register));
        }

        public Semitone RelativePitch
        {
            get => (Semitone)(((int)RootSemitone + (int)SemitonesFromRoot) % 12);
        }
    }
}

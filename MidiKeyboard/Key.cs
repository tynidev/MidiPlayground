
using Sanford.Multimedia;

namespace MidiKeyboard
{
    public class Key
    {
        public Key(int pitch)
        {
            this.AbsolutePitch = pitch;
        }

        public Key(Note name, int register)
        {
            this.AbsolutePitch = ToPitch(name, register);
        }

        public static int ToPitch(Note name, int register)
        {
            return (int)name + (register * 12);
        }

        public static Note ToNote(int pitch)
        {
            return (Note)(pitch % 12);
        }

        public readonly int AbsolutePitch = 0;
        public Note Name => (Note)RelativePitch;
        public int RelativePitch => AbsolutePitch % 12;
        public int Register => (AbsolutePitch / 12);

        private bool on = false;
        public bool On 
        { 
            get => on;
            set 
            {
                lock (this)
                {
                    on = value;
                }
            }
        }

        public override string ToString()
        {
            return this.Name + this.Register.ToString();
        }
    }
}

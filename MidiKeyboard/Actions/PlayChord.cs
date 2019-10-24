using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sanford.Multimedia.Midi;

namespace MidiKeyboard
{
    public class PlayChord : IKeyAction
    {
        protected List<int> chord = null;
        protected bool absolute = false;
        protected bool ready = false;
        protected DateTime timeFirstNotePlayed = DateTime.MinValue;

        public bool Played = false;

        public PlayChord(List<int> chord, bool absolute=false)
        {
            this.chord = chord;
            this.absolute = absolute;
        }

        public void KeyPressEvent(MidiKeyboard keyboard, int key, ChannelCommand command)
        {
            if (command != ChannelCommand.NoteOn)
                return;

            lock (this)
            {
                if (ready)
                    return;

                var on = keyboard.OnKeys;

                if (on.Count() == 1)
                {
                    timeFirstNotePlayed = DateTime.Now;
                }

                if (on.Count() != chord.Count())
                {
                    return;
                }

                if ((DateTime.Now - timeFirstNotePlayed).TotalMilliseconds >= 500)
                {
                    return;
                }


                for (int i = 0; i < chord.Count(); i++)
                {
                    if ((absolute && (on.ElementAt(i).AbsolutePitch != chord.ElementAt(i))) ||
                       (!absolute && (on.ElementAt(i).RelativePitch != chord.ElementAt(i))))
                    {
                        Played = false;
                        ready = true;
                        return;
                    }
                }
                Played = true;
                ready = true;
                return;
            }
        }

        public void WaitForKeyInput(List<IKeyAction> KeyActions)
        {
            this.ready = false;
            KeyActions.Add(this);
            while (!this.ready) { Thread.Sleep(10); };
            KeyActions.Clear();
        }
    }
}

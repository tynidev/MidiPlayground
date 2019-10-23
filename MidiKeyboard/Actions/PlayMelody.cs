using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sanford.Multimedia.Midi;

namespace MidiKeyboard
{
    public class PlayMelody : IKeyAction
    {
        protected List<int> melody = null;
        protected bool absolute = false;
        protected bool breakOnError = false;
        protected bool ready = false;
        protected int idx = 0;
        protected DateTime lastKeyPress = DateTime.MinValue;

        public bool Played = false;

        public PlayMelody(List<int> melody, bool absolute=false, bool breakOnError=false)
        {
            this.melody = melody;
            this.absolute = absolute;
            this.idx = 0;
            this.breakOnError = breakOnError;
        }

        public void KeyPressEvent(Keyboard keyboard, int absolutePitch, ChannelCommand command)
        {
            if (command != ChannelCommand.NoteOn)
                return;

            lock (this)
            {
                if (ready)
                    return;

                if ((DateTime.Now - lastKeyPress).TotalMilliseconds <= 500)
                {   // Don't accept key press if it happens nearly simultaneously 
                    return;
                }
                int relativePitch = absolutePitch % 12;
                if ((absolute && (absolutePitch == melody.ElementAt(idx))) ||
                   (!absolute && (relativePitch == melody.ElementAt(idx))))
                {   // If we played the correct note in the sequence then move to next note
                    lastKeyPress = DateTime.Now;
                    idx++;
                    if (idx == melody.Count)
                    {   // If we've played all the notes in the sequence then end
                        Played = true;
                        ready = true;
                        return;
                    }
                }
                else if (breakOnError)
                {
                    // we played a wrong note and breakOnError == true
                    ready = true;
                }
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

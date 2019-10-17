﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sanford.Multimedia.Midi;

namespace Keys
{
    public class PlayMelody : IKeyAction
    {
        protected List<Key> melody = null;
        protected bool absolute = false;
        protected bool breakOnError = false;
        protected bool ready = false;
        protected int idx = 0;

        public bool Played = false;

        public PlayMelody(List<Key> melody, bool absolute=false, bool breakOnError=false)
        {
            this.melody = melody;
            this.absolute = absolute;
            this.idx = 0;
            this.breakOnError = breakOnError;
        }

        public void KeyPressEvent(Keyboard keyboard, Key key, ChannelCommand command)
        {
            if (command != ChannelCommand.NoteOn)
                return;

            lock (this)
            {
                if (ready)
                    return;

                if ((absolute && (key.AbsolutePitch == melody.ElementAt(idx).AbsolutePitch)) ||
                   (!absolute && (key.RelativePitch == melody.ElementAt(idx).RelativePitch)))
                {   // If we played the correct note in the sequence then move to next note
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

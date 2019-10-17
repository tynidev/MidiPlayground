using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace Keys
{
    public class PlayChordAction : IKeyAction
    {
        private SortedList<int, Key> chord = null;
        private readonly bool absolute = false;
        private bool ready = false;
        private DateTime timeFirstNotePlayed = DateTime.MinValue;

        public bool Played = false;

        public PlayChordAction(SortedList<int, Key> chord, bool absolute=false)
        {
            this.chord = chord;
            this.absolute = absolute;
        }

        public void KeyPressEvent(Keyboard keyboard, Key key, ChannelCommand command)
        {
            if (ready || 
                command != ChannelCommand.NoteOn) 
            return;

            var on = keyboard.OnKeys;

            if (on.Count() == 1)
            {
                timeFirstNotePlayed = DateTime.Now;
            }

            if(on.Count() != chord.Count())
            {
                return;
            }

            if((DateTime.Now - timeFirstNotePlayed).TotalMilliseconds >= 500)
            {
                return;
            }

            
            for(int i = 0; i < chord.Count(); i++)
            {
                if ((absolute && (on.ElementAt(i).AbsolutePitch != chord.ElementAt(i).Value.AbsolutePitch)) || 
                   (!absolute && (on.ElementAt(i).RelativePitch != chord.ElementAt(i).Value.RelativePitch)))
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

        public void WaitForKeyInput(List<IKeyAction> KeyActions)
        {
            this.ready = false;
            KeyActions.Add(this);
            while (!this.ready) { Thread.Sleep(10); };
            KeyActions.Clear();
        }
    }
}

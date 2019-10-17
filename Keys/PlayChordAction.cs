using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace Keys
{
    public class PlayChordAction : IKeyAction
    {
        private SortedList<int, Key> chord = null;
        public bool ready = false;
        public bool played = false;

        public PlayChordAction(SortedList<int, Key> chord)
        {
            this.chord = chord;
        }

        public void Action(Keyboard keyboard, Key key, ChannelCommand command)
        {
            if (ready || 
                command != ChannelCommand.NoteOn || 
                keyboard.OnKeys.Count() != chord.Count()) 
            return;

            var on = keyboard.OnKeys;

            for(int i = 0; i < chord.Count(); i++)
            {
                if (on.ElementAt(i).AbsolutePitch != chord[i].AbsolutePitch)
                {
                    played = false;
                    return;
                }
            }
            return;
        }
    }
}

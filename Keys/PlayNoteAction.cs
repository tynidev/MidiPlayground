using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace Keys
{
    public class PlayNoteAction : IKeyAction
    {
        private Key keyToPlay = null;
        public int called = 0;
        public bool played;

        public PlayNoteAction(Key key)
        {
            this.keyToPlay = key;
            this.called = 0;
        }

        public void Action(Keyboard keyboard, Key key, ChannelCommand command)
        {
            if(command != ChannelCommand.NoteOn) return;

            lock (this)
            {
                this.played = this.keyToPlay.AbsolutePitch == key.AbsolutePitch;
                this.called++;
            }
        }
    }
}

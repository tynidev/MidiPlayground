using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace Keys
{
    public class PlayNoteAction : IKeyAction
    {
        private Key keyToPlay = null;
        private bool ready = false;
        public bool Played;

        public PlayNoteAction(Key key)
        {
            this.keyToPlay = key;
        }

        public void KeyPressEvent(Keyboard keyboard, Key key, ChannelCommand command)
        {
            if(command != ChannelCommand.NoteOn) return;

            lock (this)
            {
                this.Played = this.keyToPlay.AbsolutePitch == key.AbsolutePitch;
                this.ready = true;
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

using System.Collections.Generic;
using System.Threading;
using Sanford.Multimedia.Midi;

namespace MidiKeyboard
{
    public class PlayNote : IKeyAction
    {
        private Key keyToPlay = null;
        private bool ready = false;
        public bool Played;

        public PlayNote(Key key)
        {
            this.keyToPlay = key;
        }

        public void KeyPressEvent(Keyboard keyboard, Key key, ChannelCommand command)
        {
            if(command != ChannelCommand.NoteOn) return;

            lock (this)
            {
                if (ready)
                    return;

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

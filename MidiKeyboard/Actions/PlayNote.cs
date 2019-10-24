using System.Collections.Generic;
using System.Threading;
using Sanford.Multimedia.Midi;

namespace MidiKeyboard
{
    public class PlayNote : IKeyAction
    {
        private MidiKey keyToPlay = null;
        private bool ready = false;
        public bool Played;

        public PlayNote(MidiKey key)
        {
            this.keyToPlay = key;
        }

        public void KeyPressEvent(MidiKeyboard keyboard, int absolutePitch, ChannelCommand command)
        {
            if(command != ChannelCommand.NoteOn) return;

            lock (this)
            {
                if (ready)
                    return;

                this.Played = this.keyToPlay.AbsolutePitch == absolutePitch;
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

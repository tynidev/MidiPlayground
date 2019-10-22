using Sanford.Multimedia.Midi;
using System.Collections.Generic;

namespace MidiKeyboard
{
    public interface IKeyAction
    {
        void KeyPressEvent(Keyboard keyboard, Key key, ChannelCommand command);

        void WaitForKeyInput(List<IKeyAction> KeyActions);
    }
}

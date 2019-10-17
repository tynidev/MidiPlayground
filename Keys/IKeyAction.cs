using Sanford.Multimedia.Midi;

namespace Keys
{
    public interface IKeyAction
    {
        void Action(Keyboard keyboard, Key key, ChannelCommand command);
    }
}

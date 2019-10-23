using System;
using System.Collections.Generic;

namespace MidiKeyboard
{
    public class PlayInterval : PlayChord
    {
        public PlayInterval(List<int> interval, bool absolute=false) : base(interval, absolute)
        {
            if (interval.Count > 2)
                throw new ArgumentOutOfRangeException(nameof(interval));
        }
    }
}

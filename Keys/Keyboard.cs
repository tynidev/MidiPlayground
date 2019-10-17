using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sanford.Multimedia.Midi;

namespace Keys
{
    public class Keyboard
    {
        public readonly int BottomKey = 0;
        public readonly int TopKey = 0;
        public readonly Dictionary<int, Key> Keys = new Dictionary<int, Key>();

        public Keyboard() : this(128) { }
        public Keyboard(int size) : this(0, size - 1) { }
        public Keyboard(int bottomKey, int topKey)
        {
            this.BottomKey = bottomKey;
            this.TopKey = topKey;

            for (int i = this.BottomKey; i <= this.TopKey; i++)
            {
                Keys.Add(i, new Key(i));
            }
        }

        public Keyboard(InputDevice inDevice)
        {
            inDevice.StartRecording();
            bool read = false;
            int pitch = 0;

            inDevice.ChannelMessageReceived += delegate (object sender, ChannelMessageEventArgs e)
            {
                switch (e.Message.Command)
                {
                    case ChannelCommand.NoteOn:
                        lock(this)
                        {
                            read = true;
                            pitch = e.Message.Data1;
                        }
                        break;
                }
            };

            Console.WriteLine("Play bottom note on keyboard...");
            read = false;
            while (!read) { Thread.Sleep(10); };

            this.BottomKey = pitch;

            Console.WriteLine("Play top note on keyboard...");
            read = false;
            while (!read) { Thread.Sleep(10); };

            this.TopKey = pitch;

            for (int i = this.BottomKey; i <= this.TopKey; i++)
            {
                Keys.Add(i, new Key(i));
            }

            inDevice.StopRecording();
            inDevice.Reset();
        }

        public void StartRecording(InputDevice inDevice, IEnumerable<IKeyAction> actions)
        {
            inDevice.ChannelMessageReceived += delegate (object sender, ChannelMessageEventArgs e)
            {
                var command = e.Message.Command;
                var key = e.Message.Data1;

                switch (command)
                {
                    case ChannelCommand.NoteOn:
                        this[key].On = true;
                        break;
                    case ChannelCommand.NoteOff:
                        this[key].On = false;
                        break;
                }

                foreach (var foo in actions)
                    foo.Action(this, this[key], command);
            };
            inDevice.StartRecording();
        }

        public IEnumerable<Key> OnKeys => Keys.Where(k => k.Value.On == true).Select(k => k.Value);

        public Key this[int index]
        {
            get
            {
                if (!Keys.ContainsKey(index)) throw new ArgumentOutOfRangeException(nameof(index), $"{index} exceeds range of keyboard [{BottomKey},{TopKey}]");
                return Keys[index];
            }
        }
    }
}

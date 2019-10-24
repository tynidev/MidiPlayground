using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sanford.Multimedia.Midi;

namespace MidiKeyboard
{
    public class MidiKeyboard
    {
        public readonly int BottomKey = 0;
        public readonly int TopKey = 0;
        public readonly Dictionary<int, MidiKey> Keys = new Dictionary<int, MidiKey>();

        public MidiKeyboard() : this(128) { }
        public MidiKeyboard(int size) : this(0, size - 1) { }
        public MidiKeyboard(int bottomKey, int topKey)
        {
            this.BottomKey = bottomKey;
            this.TopKey = topKey;

            for (int i = this.BottomKey; i <= this.TopKey; i++)
            {
                Keys.Add(i, new MidiKey(i));
            }
        }

        public MidiKeyboard(InputDevice inDevice)
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
                Keys.Add(i, new MidiKey(i));
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
                    foo.KeyPressEvent(this, key, command);
            };
            inDevice.StartRecording();
        }

        public IEnumerable<MidiKey> OnKeys => Keys.Where(k => k.Value.On == true).Select(k => k.Value);

        public MidiKey this[int index]
        {
            get
            {
                if (!Keys.ContainsKey(index)) throw new ArgumentOutOfRangeException(nameof(index), $"{index} exceeds range of keyboard [{BottomKey},{TopKey}]");
                return Keys[index];
            }
        }
    }
}

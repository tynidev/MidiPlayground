using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(InputDevice.DeviceCount);
            Console.WriteLine(InputDevice.GetDeviceCapabilities(0));

            using (InputDevice inDevice = new InputDevice(0))
            {
                ChannelStopper stopper = new ChannelStopper();

                inDevice.ChannelMessageReceived += delegate (object sender, ChannelMessageEventArgs e)
                {
                    OutputChannelMessage(e);
                };

                inDevice.StartRecording();

                Console.ReadKey();
            }
        }

        private static void OutputChannelMessage(ChannelMessageEventArgs e)
        {
            var note = new Pitch(e.Message.Data1);

            Console.WriteLine(
$@"Channel: {e.Message.MidiChannel}
Type: {e.Message.MessageType}
Command: {e.Message.Command}
Data1: {e.Message.Data1}
Data2: {e.Message.Data2}
Note: {note.Name} {note.Register}");

        }

        private class Pitch
        {
            public enum Note
            {
                C = 0,
                Csharp = 1,
                Dflat = 1,
                D = 2,
                Dsharp = 3,
                Eflat = 3,
                E = 4,
                F = 5,
                Fsharp = 6,
                Gflat = 6,
                G = 7,
                GSharp = 8,
                Aflat = 8,
                A = 9,
                Asharp = 10,
                Bflat = 10,
                B = 11
            }

            public Pitch(int pitch)
            {
                this.AbsolutePitch = pitch;
            }

            public Note Name => (Note)(AbsolutePitch % 12);

            public int AbsolutePitch = 0;
            public int RelativePitch => AbsolutePitch % 12;
            public int Register => (AbsolutePitch / 12) - 1;
        }
    }
}

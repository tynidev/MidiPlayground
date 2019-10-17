using Sanford.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public class Chords
    {
        public static readonly Dictionary<string, SortedList<int, Key>> ChordMaps = new Dictionary<string, SortedList<int, Key>>()
        {
            {
                "C Major Triad Root Position", 
                new SortedList<int, Key>()
                {
                    {60, new Key(Note.C, 4)},
                    {64, new Key(Note.E, 4)},
                    {67, new Key(Note.G, 4)},
                }
            }
        };
    }
}

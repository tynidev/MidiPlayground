using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public class CircleOf5ths : CircularList<Key>
    {
        public CircleOf5ths()
        {
            this.list = (new CircularList<Key>()
            {
                new Key(NoteLetters.c),

                new Key(NoteLetters.g),
                new Key(NoteLetters.d),
                new Key(NoteLetters.a),
                new Key(NoteLetters.e),
                new Key(NoteLetters.b),

                new Key(NoteLetters.f, Accidental.s), new Key(NoteLetters.g, Accidental.f),

                new Key(NoteLetters.d, Accidental.f),
                new Key(NoteLetters.a, Accidental.f),
                new Key(NoteLetters.e, Accidental.f),
                new Key(NoteLetters.b, Accidental.f),
                new Key(NoteLetters.f),
            }).list;
        }
    }
}

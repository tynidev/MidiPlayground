using System.Collections.Generic;
using System.Linq;

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

            foreach(var key in this)
            {
                int flats = key.Where(n =>
                {
                    var a = Accidental.n;
                    key.GetSheetNoteName(n.Interval, out a);

                    return a == Accidental.f;
                }).Count();
                int sharps = key.Where(n => {
                    var a = Accidental.n;
                    key.GetSheetNoteName(n.Interval, out a);

                    return a == Accidental.s;
                }).Count();

                string keyStr = "0";
                if(flats > 0 && sharps == 0)
                {
                    keyStr = $"{flats}f";
                }
                else if (sharps > 0 && flats == 0)
                {
                    keyStr = $"{sharps}s";
                }
                key.KeyAccidentals = keyStr;
                KeyMap.Add(keyStr, key);
            }
        }

        private Dictionary<string, Key> KeyMap = new Dictionary<string, Key>();

        public Key this[string index]
        {
            get
            {
                return this.Where(k => k.KeyAccidentals == index).FirstOrDefault();
            }
        }
    }
}

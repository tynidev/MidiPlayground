using System;
using System.Collections.Generic;
using System.Linq;
using Keys;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeysUnitTests
{
    [TestClass]
    public class KeyTests
    {
        [TestMethod]
        public void TestCircleOf5ths()
        {
            var circle = new CircleOf5ths();
            Assert.AreEqual(13, circle.Count);

            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, -6, -5, -4, -3, -2, -1 };
            for (int i = 0; i < circle.Count; i++)
            {
                int count = 0;
                foreach (var n in circle[i])
                {
                    var a = Accidental.n;
                    var name = circle[i].GetSheetNoteName(n.Interval, out a);

                    if (expected[i] > 0)
                    {
                        Assert.IsTrue((int)a >= 0 && (int)a < 2);
                    }
                    else
                    {
                        Assert.IsTrue((int)a <= 0 && (int)a > -2);
                    }

                    count += (int)a;

                    if (i != 6)
                    {
                        // Assert Tranpose while we are at it
                        var noteToChange = circle[i][1];
                        var transposedNote = circle[i + 1].Transpose(noteToChange);
                        var projectNote = circle[i + 1].Project(noteToChange);

                        Assert.AreEqual(noteToChange.AbsolutePitch, projectNote.AbsolutePitch);
                        Assert.AreEqual(circle[i + 1][1].SemitonesFromRoot, transposedNote.SemitonesFromRoot);
                        Assert.AreEqual(circle[i + 1][1].RootSemitone, transposedNote.RootSemitone);
                        Assert.AreEqual(circle[i + 1][1].Interval, transposedNote.Interval);
                        Assert.AreEqual(circle[i][5].RelativePitch, transposedNote.RelativePitch);
                    }
                }

                Assert.AreEqual(expected[i], count);
            }
        }

        [TestMethod]
        public void TestProject()
        {
            var circle = new CircleOf5ths();
            Assert.AreEqual(13, circle.Count);

            var c = circle[0][1];
            var gScale = circle[1];

            var note = gScale.Project(c);
            Assert.AreEqual(c.AbsolutePitch, note.AbsolutePitch);
        }

        [TestMethod]
        public void TestSheetMusicNoteNames()
        {
            var circle = new CircleOf5ths();
            Assert.AreEqual(13, circle.Count);

            var expected = new Dictionary<int, string>()
            {
                {0,"cdefgab"},
                {1,"gabcdef#" },
                {2,"def#gabc#"},
                {3,"abc#def#g#"},
                {4,"ef#g#abc#d#"},
                {5,"bc#d#ef#g#a#"},
                {6,"f#g#a#bc#d#e#"},
                {-1,"fgab-cde"},
                {-2,"b-cde-fga"},
                {-3,"e-fga-b-cd"},
                {-4,"a-b-cd-e-fg"},
                {-5,"d-e-fg-a-b-c"},
                {-6,"g-a-b-c-d-e-f"},
            };

            foreach (var kvp in expected)
            {
                var names = GetNoteNames(circle[kvp.Key], circle[kvp.Key].Select(n => n.Interval).ToList());
                Assert.AreEqual(kvp.Value, names, $"The key on the circle of 5ths with index {kvp.Key} failed to generate correct note names.  Expected {kvp.Value} != Actual {names}");
            }

            var cf = new Key(NoteLetters.c, Accidental.f);
            var cfNames = GetNoteNames(cf, cf.Select(n => n.Interval).ToList());
            Assert.AreEqual("c-d-e-f-g-a-b-", cfNames, $"C Flat key names were not correct Expected c-d-e-f-g-a-b- != Actual {cfNames}");

            var cs = new Key(NoteLetters.c, Accidental.s);
            var csNames = GetNoteNames(cs, cs.Select(n => n.Interval).ToList());
            Assert.AreEqual("c#d#e#f#g#a#b#", csNames, $"C Sharp key names were not correct Expected c#d#e#f#g#a#b# != Actual {csNames}");

            var gs = new Key(NoteLetters.g, Accidental.s);
            var gsNames = GetNoteNames(gs, gs.Select(n => n.Interval).ToList());
            Assert.AreEqual("g#a#b#c#d#e#fx", gsNames, $"G Sharp key names were not correct Expected g#a#b#c#d#e#fx != Actual {gsNames}");

            var ff = new Key(NoteLetters.f, Accidental.f);
            var ffNames = GetNoteNames(ff, ff.Select(n => n.Interval).ToList());
            Assert.AreEqual("f-g-a-b--c-d-e-", ffNames, $"F Flat key names were not correct Expected f-g-a-b--c-d-e- != Actual {ffNames}");
        }

        private static string GetNoteName(Keys.Key key, int i)
        {
            var a = Accidental.n;
            var n = key.GetSheetNoteName(i, out a);

            var mod = "";
            switch (a)
            {
                case Accidental.s:
                    mod = "#";
                    break;
                case Accidental.x:
                    mod = "x";
                    break;
                case Accidental.f:
                    mod = "-";
                    break;
                case Accidental.ff:
                    mod = "--";
                    break;
            }
            return n.ToString() + mod;
        }

        private static string GetNoteNames(Keys.Key key, List<int> notes)
        {
            string name = "";
            for (int i = 0; i < notes.Count(); i++)
            {
                name += GetNoteName(key, notes[i]);
            }
            return name;
        }
    }
}

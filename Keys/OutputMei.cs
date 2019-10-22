using Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public class OutputMei
    {
        public enum NoteValue
        {
            Sixteenth = 16,
            Eighth = 8,
            Quarter = 4,
            Half = 2,
            Whole = 1,

        }

        public static string Chord(List<Key> chord, NoteValue length, bool dotted = false, string color = "#000000")
        {
            var notes = string.Join("\r\n", chord.Select(c => $"<note pnum='{c.AbsolutePitch}' />"));
            return
$@"<chord dur='{(int)length}' color='{color}'>
{notes}
</chord>";
        }

        public static string Measure(int n, string trebble, string clef)
        {
            return $@"<measure n='{n}'>
						<staff n='1'>
							<layer>
                                {trebble}
							</layer>
						</staff>
						<staff n='2'>
							<layer>
								{clef}
							</layer>
						</staff>
					</measure>";
        }

        public static string Song(string title, int beatsPerMeasure, int beatUnit, string keySig, string majorMinor, string section)
        {
            return $@"<?xml version='1.0' encoding='UTF-8' standalone='no'?>
<?xml-model href='http://music-encoding.org/schema/4.0.0/mei-all.rng' type='application/xml' schematypens='http://relaxng.org/ns/structure/1.0'?>
<?xml-model href='http://music-encoding.org/schema/4.0.0/mei-all.rng' type='application/xml' schematypens='http://purl.oclc.org/dsdl/schematron'?>
<mei xmlns:xlink='http://www.w3.org/1999/xlink' xmlns='http://www.music-encoding.org/ns/mei'
  meiversion='4.0.0'>
<meiHead>
	<fileDesc>
		{title}
		<pubStmt>
		  <unpub></unpub>
	   </pubStmt>
	</fileDesc>
</meiHead>
<music>
	<body>
		<mdiv>
			<score>
				<scoreDef meter.count='{beatsPerMeasure}' meter.unit='{beatUnit}' key.sig='{keySig}' key.mode='{majorMinor}'>
					<staffGrp>
						<staffDef n='1' clef.line='2' clef.shape='G' lines='5'/>
						<staffDef n='2' clef.line='4' clef.shape='F' clef.dis.place='below' lines='5'/>
					</staffGrp>
				</scoreDef>
				<section>
{section}
				</section>
			</score>
		</mdiv>
	</body>
</music>
</mei>";
        }
        public static string Title(string title)
        {
            return $@"<titleStmt>
			<title>{title}</title>
		</titleStmt>
";
        }
    }
}

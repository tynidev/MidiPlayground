using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Keys;
using Microsoft.Toolkit.Wpf.UI.Controls;

namespace FlashCards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var circle = new CircleOf5ths();

            var triad = new List<Note>()
            {
                new Note(){Interval = 1, Register = 2},
                new Note(){Interval = 3, Register = 2},
                new Note(){Interval = 5, Register = 2},
            };

            var note = circle[-1].Project(circle[-1][4]);
            var chord = circle[-1].Project(circle[3].Transpose(triad)).ToList();

            //var eMajor = circle[4].Transpose(triad).ToList();

            var xml = OutputMei.Song("", 4, 4, "1f", "major",
                OutputMei.Measure(1,
                    OutputMei.Note(note, OutputMei.NoteValue.Quarter),
                    OutputMei.Chord(chord, OutputMei.NoteValue.Quarter)));

            DisplayMei(xml);
        }

        public void DisplayHtml(string html)
        {
            webView.NavigateToString(html);
        }

        public void DisplayMei(string mei)
        {
            DisplayHtml($@"<html>
    <head>
        <script src='https://www.verovio.org/javascript/app/verovio-app.js'></script>
        <script>
        function myFunction()
        " + "{" + $@"
            var text = '{Regex.Replace(mei.Replace("'", "\\'"), @"\t|\n|\r", "")}';

            // Create the app - here with an empty option object
            const app = new Verovio.App(document.getElementById('app'), " + "{}" + @");

            // Load a file (MEI or MusicXML)
            app.loadData(text);
        " + "}" + @"
        </script>
    </head>
    <body onload='myFunction()'>
        <div class='panel-body'>
            <div id='app' class='panel' style='border: 1px solid lightgray; min-height: 450px;'></div>
        </div>
    </body>
</html>");
        }
    }
}

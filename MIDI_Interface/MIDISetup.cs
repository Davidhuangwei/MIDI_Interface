using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Sanford.Collections;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia;
using Sanford.Threading;
using System.Windows.Forms;


namespace MIDI_Interface
{
    class MIDISetup
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InterfaceForm());
        }
    }
}

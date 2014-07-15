using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;         // Required for Synchronisation contexts
using Sanford.Collections;      // Required Sanford libraries for MIDI Toolbox
using Sanford.Multimedia.Midi;
using Sanford.Multimedia;
using Sanford.Threading;
using System.Text.RegularExpressions;


namespace MIDI_Interface
{
    internal struct outputMessages // Output messages struct containing the note and velocity
    {
        public int Note;        // MIDI Note Value
        public int Mess;        // Velocity of MIDI Note
        public bool isCC;       // True for Control Change or false for NoteOn message
    }

    public partial class InterfaceForm : Form
    {
        private static NotifyIcon notifyIcon = null;   // System tray icon
        private HardwareSetup FormSetup; // Instance of hardware setup class

/*       public static void returnMessages(out int chan, out int mess)
        {
            chan = outmess.Note;
            mess = outmess.Mess;
        }
*/

        public InterfaceForm() 
        {
            InitializeComponent(); // Initialise windows form
        }

        protected override void OnLoad(EventArgs e) // On form load check for input devices
        {
            SetUpTrayIcon();
            FormSetup = new HardwareSetup(this);
            FormSetup.initialise();
            return;
        }

        protected override void OnClosed(EventArgs e) // When the form is closed, ensure device is stopped and close stream
        {
            FormSetup.release();
            base.OnClosed(e); // repeat as necessary
        }

/*        private void sortOutmess(outputMessages mess) // Will set variables for TFM to be read outside
        {                                             // of program whenever new message is received
            if (mess.isCC)
            {
                float velocity = mess.Mess;
                switch (mess.Note)
                {
                    case 0:                 // Fader 1
                        parameters.x1 = velocity;
                        this.Fader1.Value = (decimal) velocity;
                        break;
                    case 1:                 // Fader 2
                        parameters.x2 = velocity;
                        this.Fader2.Value = (decimal) velocity;
                        break;
                    case 2:
                        parameters.x3 = velocity;
                        this.Fader3.Value = (decimal) velocity;
                        break;
                    case 3:
                        parameters.x4 = velocity;
                        this.Fader4.Value = (decimal) velocity;
                        break;
                    case 4:
                        parameters.x5 = velocity;
                        this.Fader5.Value = (decimal) velocity;
                        break;
                    case 5:
                        parameters.x6 = velocity;
                        this.Fader6.Value = (decimal) velocity;
                        break;
                    case 6:
                        parameters.x7 = velocity;
                        this.Fader7.Value = (decimal) velocity;
                        break;
                    case 7:                 // Fader 8
                        parameters.x8 = velocity;
                        this.Fader8.Value = (decimal) velocity;
                        break;
                    case 8:                 // Knob 1
                        parameters.x9 = velocity;
                        this.Knob1.Value = (decimal) velocity;
                        break;
                    case 9:
                        parameters.x10 = velocity;
                        this.Knob2.Value = (decimal) velocity;
                        break;
                    case 10:
                        parameters.x11 = velocity;
                        this.Knob3.Value = (decimal) velocity;
                        break;
                    case 11:
                        parameters.x12 = velocity;
                        this.Knob4.Value = (decimal) velocity;
                        break;
                    case 12:
                        parameters.x13 = velocity;
                        this.Knob5.Value = (decimal) velocity;
                        break;
                    case 13:
                        parameters.x14 = velocity;
                        this.Knob6.Value = (decimal) velocity;
                        break;
                    case 14:
                        parameters.x15 = velocity;
                        this.Knob7.Value = (decimal) velocity;
                        break;
                    case 15:                // Knob 8
                        parameters.x16 = velocity;
                        this.Knob8.Value = (decimal) velocity;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Invalid control change, please change note value");
                        break;
                }

            }
            else
            {
                bool onoff = mess.Mess > 0;
                switch (mess.Note)
                {
                    case 0:                  // Top row, far left button (Button 1)
                        parameters.y1 = onoff;
                        Button1_Click(this, EventArgs.Empty);
                        break;
                    case 1:                 // Button to the right of Button 1 (Button 2)
                        parameters.y2 = onoff;
                        Button2_Click(this, EventArgs.Empty);
                        break;
                    case 2:
                        parameters.y3 = onoff;
                        Button3_Click(this, EventArgs.Empty);
                        break;
                    case 3:
                        parameters.y4 = onoff;
                        Button4_Click(this, EventArgs.Empty);
                        break;
                    case 4:
                        parameters.y5 = onoff;
                        Button5_Click(this, EventArgs.Empty);
                        break;
                    case 5:
                        parameters.y6 = onoff;
                        Button6_Click(this, EventArgs.Empty);
                        break;
                    case 6:
                        parameters.y7 = onoff;  
                        Button7_Click(this, EventArgs.Empty);
                        break;
                    case 7:                      // Top row, far right button (Button 8)
                        parameters.y8 = onoff;
                        Button8_Click(this, EventArgs.Empty);
                        break;
                    case 8:                     // Bottom row, far left (Button 9)
                        parameters.y9 = onoff;
                        Button9_Click(this, EventArgs.Empty);
                        break;
                    case 9:
                        parameters.y10 = onoff;
                        Button10_Click(this, EventArgs.Empty);
                        break;
                    case 10:
                        parameters.y11 = onoff;
                        Button11_Click(this, EventArgs.Empty);
                        break;
                    case 11:
                        parameters.y12 = onoff;
                        Button12_Click(this, EventArgs.Empty);
                        break;
                    case 12:
                        parameters.y13 = onoff;
                        Button13_Click(this, EventArgs.Empty);
                        break;
                    case 13:
                        parameters.y14 = onoff;
                        Button14_Click(this, EventArgs.Empty);
                        break;
                    case 14:
                        parameters.y15 = onoff;
                        Button15_Click(this, EventArgs.Empty);
                        break;
                    case 15:                // Bottom row, far right (Button 16)
                        parameters.y16 = onoff;
                        Button16_Click(this, EventArgs.Empty);
                        break;
                    case 16:                // Bottom right of controller, top left (Button 17)
                        parameters.y17 = onoff;
                        Button17_Click(this, EventArgs.Empty);
                        break;
                    case 17:                // Bottom right of controller, top right (Button 18)
                        parameters.y18 = onoff;
                        Button18_Click(this, EventArgs.Empty);
                        break;
                    case 18:                // Bottom right of controller, bottom left (Button 19)
                        parameters.y19 = onoff;
                        Button19_Click(this, EventArgs.Empty);
                        break;
                    case 19:                // Bottom right of controller, bottom right (Button 20)
                        parameters.y20 = onoff;
                        Button20_Click(this, EventArgs.Empty);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Invalid button pressed, please change note value");
                        break;
                }
            }
            return;
        }
*/

        private void SetUpTrayIcon()  // sets up an icon for the program in the system tray
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon(); // create system tray icon
            notifyIcon.BalloonTipText = "MIDI Interface running in system tray"; // baloon popup text
            notifyIcon.BalloonTipTitle = "MIDI Interface"; // title 
            notifyIcon.Text = "MIDI Interface"; // icon hover text
            notifyIcon.Icon = Properties.Resources.MyIcon; // location of icon in resources
            notifyIcon.Click += new EventHandler(notifyIcon_Click); // on click event handler
        }

        private void notifyIcon_Click(object sender, EventArgs e) // when tray icon is clicked
        {
            this.Show(); // show form again
            this.WindowState = FormWindowState.Normal; // change the state of the window to normal
            notifyIcon.Visible = false; // hide tray icon
        }

        private void Form_Resize(object sender, EventArgs e) // When window is resized
        {
            if (WindowState == FormWindowState.Minimized) // if the window is minimised
            {
                if (notifyIcon != null)
                {
                    notifyIcon.Visible = true; // show icon
                    notifyIcon.ShowBalloonTip(2000); // show baloon tooltip for 2 seconds
                }
                this.Hide(); // hide form
            }
        }

        internal void setValue(int note, decimal value)
        {
            switch (note)
            {
                case 0:
                    this.Fader1.Value = value;
                    break;
                case 1:
                    this.Fader2.Value = value;
                    break;
                case 2:
                    this.Fader3.Value = value;
                    break;
                case 3:
                    this.Fader4.Value = value;
                    break;
                case 4:
                    this.Fader5.Value = value;
                    break;
                case 5:
                    this.Fader6.Value = value;
                    break;
                case 6:
                    this.Fader7.Value = value;
                    break;
                case 7:
                    this.Fader8.Value = value;
                    break;
                case 8:
                    this.Knob1.Value = value;
                    break;
                case 9:
                    this.Knob2.Value = value;
                    break;
                case 10:
                    this.Knob3.Value = value;
                    break;
                case 11:
                    this.Knob4.Value = value;
                    break;
                case 12:
                    this.Knob5.Value = value;
                    break;
                case 13:
                    this.Knob6.Value = value;
                    break;
                case 14:
                    this.Knob7.Value = value;
                    break;
                case 15:
                    this.Knob8.Value = value;
                    break;
                default:
                    break;
            }
        }

        internal void setButton(int note, bool onoff)
        {
            Color C;
            if (onoff) { C = Color.Green; }
            else{ C = Color.Red; }

            switch (note)
            {
                case 0:                  // Top row, far left button (Button 1)
                    this.Button1.BackColor = C;
                    break;
                case 1:                 // Button to the right of Button 1 (Button 2)
                    this.Button2.BackColor = C;
                    break;
                case 2:
                    this.Button3.BackColor = C;
                    break;
                case 3:
                    this.Button4.BackColor = C;
                    break;
                case 4:
                    this.Button5.BackColor = C;
                    break;
                case 5:
                    this.Button6.BackColor = C;
                    break;
                case 6:
                    this.Button7.BackColor = C;
                    break;
                case 7:                      // Top row, far right button (Button 8)
                    this.Button8.BackColor = C;
                    break;
                case 8:                     // Bottom row, far left (Button 9)
                    this.Button9.BackColor = C;
                    break;
                case 9:
                    this.Button10.BackColor = C;
                    break;
                case 10:
                    this.Button11.BackColor = C;
                    break;
                case 11:
                    this.Button12.BackColor = C;
                    break;
                case 12:
                    this.Button13.BackColor = C;
                    break;
                case 13:
                    this.Button14.BackColor = C;
                    break;
                case 14:
                    this.Button15.BackColor = C;
                    break;
                case 15:                // Bottom row, far right (Button 16)
                    this.Button16.BackColor = C;
                    break;
                case 16:                // Bottom right of controller, top left (Button 17)
                    this.Button17.BackColor = C;
                    break;
                case 17:                // Bottom right of controller, top right (Button 18)
                    this.Button18.BackColor = C;
                    break;
                case 18:                // Bottom right of controller, bottom left (Button 19)
                    this.Button19.BackColor = C;
                    break;
                case 19:                // Bottom right of controller, bottom right (Button 20)
                    this.Button20.BackColor = C;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Invalid button pressed, please change note value");
                    break;
            }
        }

        // After this point is mostly on-click event handlers for faders,
        // knobs and buttons of which there are a lot

        private void Button_Click(object sender, EventArgs e) // When button clicked on interface
        {
            Label L = (Label)sender;
            string tag = Regex.Match((string)L.Tag, @"\d+").Value; // extract integer value from tag
            int k = Int32.Parse(tag) - 1; // parse as integer and make zero indexed
            if (L.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                L.BackColor = System.Drawing.Color.Red;
                parameters.setY(k, false);
                HardwareSetup.noteMess(k, false);
            }
            else // if button is inactive
            {
                L.BackColor = System.Drawing.Color.Green;
                parameters.setY(k, true);
                HardwareSetup.noteMess(k, true);
            }
            
        }

        private void Fader_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            NumericUpDown N = (NumericUpDown)sender;
            int index = Int32.Parse(N.Tag.ToString().Last().ToString()) - 1;
            float value = (float)N.Value; // interface value
            if (value != parameters.x[index]) // if the current value on the interface differs from stored parameter
            {
                parameters.setX(index, value);
                HardwareSetup.controlMess(index, value);
            }
        }

        private void Knob_ValueChanged(object sender, EventArgs e) // If knob value is changed
        {
            NumericUpDown N = (NumericUpDown)sender;
            int index = 7 + Int32.Parse(N.Tag.ToString().Last().ToString());
            float value = (float)N.Value; // interface value
            if (value != parameters.x[index]) // if the current value on the interface differs from stored parameter
            {
                parameters.setX(index, value);
                HardwareSetup.controlMess(index, value);
            }
        }
    }
}

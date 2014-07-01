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


namespace MIDI_Interface
{
    public struct outputMessages // Output messages struct containing the note and velocity
    {
        public int Note;        // MIDI Note Value
        public int Mess;        // Velocity of MIDI Note
        public bool isCC;       // True for Control Change or false for NoteOn message
    }
    public partial class InterfaceForm : Form
    {
        private static ChannelCommand n = ChannelCommand.NoteOn;
        private static SynchronizationContext context; // Synchronisation context for syncing device
        private static int inDeviceID = 0;             // Input device ID, increments with further device additions
        public static InputDevice BCF2000_i = null;    // MIDI input device object
        public static OutputDevice BCF2000_o = null;   // MIDI output to send messages to BCF2000
        private static outputMessages outmess;         // output messages stored in this struct
        private static NotifyIcon notifyIcon = null;   // System tray icon
        public static void returnMessages(out int chan, out int mess)
        {
            chan = outmess.Note;
            mess = outmess.Mess;
        }

        public InterfaceForm() 
        {
            InitializeComponent(); // Initialise windows form
        }

        public static void start() // Initialises BCF2000 data stream
        {
            BCF2000_i.SysExBufferSize = 4096;
            BCF2000_i.StartRecording();
            return;
        }
        protected override void OnLoad(EventArgs e) // On form load check for input devices
        {
            SetUpTrayIcon();
            if (InputDevice.DeviceCount <= inDeviceID)
            {
                //System.Diagnostics.Debug.WriteLine("No Devices");
            }
            else
            {
                try
                {
                    context = SynchronizationContext.Current;

                    //System.Diagnostics.Debug.WriteLine("MIDI Devices detected!");
                    BCF2000_i = new InputDevice(inDeviceID++);                        // Set up input device by ID
                    BCF2000_i.ChannelMessageReceived += HandleChannelMessageReceived; // Handles all channel messages
                    BCF2000_o = new OutputDevice(1);
                    System.Diagnostics.Debug.WriteLine("MIDI Device " + BCF2000_i.DeviceID + " initialised");
                    //System.Diagnostics.Debug.WriteLine("Number of output devices: " + OutputDevice.DeviceCount);
                    //System.Diagnostics.Debug.WriteLine("Number of input devices: " + InputDevice.DeviceCount);

                }
                catch (Exception ex) // Device failure by any exceptions
                {
                    System.Diagnostics.Debug.WriteLine("Device failed to initialise, " + ex.Message);
                    return;
                }
                if (BCF2000_o != null)
                {
                    try
                    {
                        BCF2000_o.RunningStatusEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Device failed to initialise, " + ex.Message);
                        return;
                    }
                }

                if (BCF2000_i != null)
                {
                    try
                    {
                        start(); // Start recording if BCF2000 is a valid object
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Device failed to initialise, " + ex.Message);
                        return;
                    }
                }
            }
            return;
        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate(object dummy)
            {
                System.Diagnostics.Debug.WriteLine(
                    e.Message.Command.ToString() + '\t' + '\t' +
                    e.Message.MidiChannel.ToString() + '\t' +
                    e.Message.Data1.ToString() + '\t' +
                    e.Message.Data2.ToString());
            }, null);                                  // Writes all channel messages to output console
            outmess.Note = e.Message.Data1; // Insert channel messages to variable struct: Note
            outmess.Mess = e.Message.Data2; // Velocity
            if (e.Message.Command.Equals(n))
            {
                outmess.isCC = false; // set false if the output message is a NoteOn message
            }
            else
            {
                outmess.isCC = true; // True if output message is a Control Change (only two types of message possible)
            }
            sortOutmess(outmess);
        }


        protected override void OnClosed(EventArgs e) // When the form is closed, ensure device is stopped and close stream
        {
            if (BCF2000_i != null)
            {
                BCF2000_i.StopRecording();
                BCF2000_i.Reset();
                BCF2000_i.Close();

                System.Diagnostics.Debug.WriteLine("MIDI Device disposed");
            }

            if (BCF2000_o != null)
            {
                BCF2000_o.Reset();
                BCF2000_o.Close();

                System.Diagnostics.Debug.WriteLine("MIDI Device disposed");
            }

            base.OnClosed(e); // repeat as necessary
        }

        private void sortOutmess(outputMessages mess) // Will set variables for TFM to be read outside
        {                                             // of program whenever new message is received
            if (mess.isCC)
            {
                float velocity = mess.Mess;
                switch (mess.Note)
                {
                    case 0:                 // Fader 1
                        parameters.x1 = velocity;
                        break;
                    case 1:                 // Fader 2
                        parameters.x2 = velocity;
                        break;
                    case 2:
                        parameters.x3 = velocity;
                        break;
                    case 3:
                        parameters.x4 = velocity;
                        break;
                    case 4:
                        parameters.x5 = velocity;
                        break;
                    case 5:
                        parameters.x6 = velocity;
                        break;
                    case 6:
                        parameters.x7 = velocity;
                        break;
                    case 7:                 // Fader 8
                        parameters.x8 = velocity;
                        break;
                    case 8:                 // Knob 1
                        parameters.x9 = velocity;
                        break;
                    case 9:
                        parameters.x10 = velocity;
                        break;
                    case 10:
                        parameters.x11 = velocity;
                        break;
                    case 11:
                        parameters.x12 = velocity;
                        break;
                    case 12:
                        parameters.x13 = velocity;
                        break;
                    case 13:
                        parameters.x14 = velocity;
                        break;
                    case 14:
                        parameters.x15 = velocity;
                        break;
                    case 15:                // Knob 8
                        parameters.x16 = velocity;
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
                        break;
                    case 2:
                        parameters.y3 = onoff;
                        break;
                    case 3:
                        parameters.y4 = onoff;
                        break;
                    case 4:
                        parameters.y5 = onoff;
                        break;
                    case 5:
                        parameters.y6 = onoff;
                        break;
                    case 6:
                        parameters.y7 = onoff;  
                        break;
                    case 7:                      // Top row, far right button (Button 8)
                        parameters.y8 = onoff;
                        break;
                    case 8:                     // Bottom row, far left (Button 9)
                        parameters.y9 = onoff;
                        break;
                    case 9:
                        parameters.y10 = onoff;
                        break;
                    case 10:
                        parameters.y11 = onoff;
                        break;
                    case 11:
                        parameters.y12 = onoff;
                        break;
                    case 12:
                        parameters.y13 = onoff;
                        break;
                    case 13:
                        parameters.y14 = onoff;
                        break;
                    case 14:
                        parameters.y15 = onoff;
                        break;
                    case 15:                // Bottom row, far right (Button 16)
                        parameters.y16 = onoff;
                        break;
                    case 16:                // Bottom right of controller, top left (Button 17)
                        parameters.y17 = onoff;
                        break;
                    case 17:                // Bottom right of controller, top right (Button 18)
                        parameters.y18 = onoff;
                        break;
                    case 18:                // Bottom right of controller, bottom left (Button 19)
                        parameters.y19 = onoff;
                        break;
                    case 19:                // Bottom right of controller, bottom right (Button 20)
                        parameters.y20 = onoff;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Invalid button pressed, please change note value");
                        break;
                }
            }
            return;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();
            builder.MidiChannel = 0;
            builder.Command = ChannelCommand.NoteOn;
            builder.Data1 = 0;
            if (this.Button1.BackColor == System.Drawing.Color.Green)
            {
                if (parameters.y1 == false)
                {
                    this.Button1.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.Button1.BackColor = System.Drawing.Color.Red;
                    parameters.y1 = false;
                }
                builder.Data2 = 0;
            }
            else
            {
                if (parameters.y1 == true)
                {
                    this.Button1.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button1.BackColor = System.Drawing.Color.Green;
                    parameters.y1 = true;
                }
                builder.Data2 = 100;
            }
            builder.Build();
            BCF2000_o.Send(builder.Result);
        }

        private void SetUpTrayIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "MIDI Interface running in system tray";
            notifyIcon.BalloonTipTitle = "MIDI Interface";
            notifyIcon.Text = "MIDI Interface";
            notifyIcon.Icon = Properties.Resources.MyIcon;
            notifyIcon.Click += new EventHandler(notifyIcon_Click);
        }
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }
        private void Form_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (notifyIcon != null)
                {
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(2000);
                }
                this.Hide();
            }
        }
    }
}

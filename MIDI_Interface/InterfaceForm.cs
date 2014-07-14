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
        private static SynchronizationContext context; // Synchronisation context for syncing form
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
                    context = SynchronizationContext.Current; // update synchronisation context for threading in program

                    //System.Diagnostics.Debug.WriteLine("MIDI Devices detected!");
                    BCF2000_i = new InputDevice(inDeviceID++);                        // Input from controller
                    BCF2000_i.ChannelMessageReceived += HandleChannelMessageReceived; // Handles all channel messages
                    BCF2000_o = new OutputDevice(1);                                  // Output to controller
                    System.Diagnostics.Debug.WriteLine("MIDI Device " + BCF2000_i.DeviceID + " initialised");
                    //System.Diagnostics.Debug.WriteLine("Number of output devices: " + OutputDevice.DeviceCount);
                    //System.Diagnostics.Debug.WriteLine("Number of input devices: " + InputDevice.DeviceCount);

                }
                catch (Exception ex) // Device failure by any exceptions
                {
                    System.Diagnostics.Debug.WriteLine("Device failed to initialise, " + ex.Message);
                    return;
                }
                if (BCF2000_o != null) // if MIDI device has initialised
                {
                    try
                    {
                        BCF2000_o.RunningStatusEnabled = true;  // allow output messages
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Device failed to initialise, " + ex.Message);
                        return;
                    }
                }

                if (BCF2000_i != null) // if MIDI device has initialised
                {
                    try
                    {
                        start(); // Start recording if BCF2000_i is a valid object
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
               /* System.Diagnostics.Debug.WriteLine(
                    e.Message.Command.ToString() + '\t' + '\t' +
                    e.Message.MidiChannel.ToString() + '\t' +
                    e.Message.Data1.ToString() + '\t' +
                    e.Message.Data2.ToString());*/
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
            sortOutmess(outmess); // use messages to change parameters
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

        // After this point is mostly on-click event handlers of which there are a lot

        private void Button1_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 0;  // Note number
            if (this.Button1.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y1 == false) // if variable is already false then the command came from controller
                {
                    this.Button1.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button1.BackColor = System.Drawing.Color.Red;
                    parameters.y1 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y1 == true)  // as above
                {
                    this.Button1.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button1.BackColor = System.Drawing.Color.Green;
                    parameters.y1 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button2_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 1;  // Note number
            if (this.Button2.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y2 == false) // if variable is already false then the command came from controller
                {
                    this.Button2.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button2.BackColor = System.Drawing.Color.Red;
                    parameters.y2 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y2 == true)  // as above
                {
                    this.Button2.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button2.BackColor = System.Drawing.Color.Green;
                    parameters.y2 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button3_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 2;  // Note number
            if (this.Button3.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y3 == false) // if variable is already false then the command came from controller
                {
                    this.Button3.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button3.BackColor = System.Drawing.Color.Red;
                    parameters.y3 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y3 == true)  // as above
                {
                    this.Button3.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button3.BackColor = System.Drawing.Color.Green;
                    parameters.y3 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button4_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 3;  // Note number
            if (this.Button4.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y4 == false) // if variable is already false then the command came from controller
                {
                    this.Button4.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button4.BackColor = System.Drawing.Color.Red;
                    parameters.y4 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y4 == true)  // as above
                {
                    this.Button4.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button4.BackColor = System.Drawing.Color.Green;
                    parameters.y4 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button5_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 4;  // Note number
            if (this.Button5.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y5 == false) // if variable is already false then the command came from controller
                {
                    this.Button5.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button5.BackColor = System.Drawing.Color.Red;
                    parameters.y5 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y5 == true)  // as above
                {
                    this.Button5.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button5.BackColor = System.Drawing.Color.Green;
                    parameters.y5 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button6_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 5;  // Note number
            if (this.Button6.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y6 == false) // if variable is already false then the command came from controller
                {
                    this.Button6.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button6.BackColor = System.Drawing.Color.Red;
                    parameters.y6 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y6 == true)  // as above
                {
                    this.Button6.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button6.BackColor = System.Drawing.Color.Green;
                    parameters.y6 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button7_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 6;  // Note number
            if (this.Button7.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y7 == false) // if variable is already false then the command came from controller
                {
                    this.Button7.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button7.BackColor = System.Drawing.Color.Red;
                    parameters.y7 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y7 == true)  // as above
                {
                    this.Button7.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button7.BackColor = System.Drawing.Color.Green;
                    parameters.y7 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button8_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 7;  // Note number
            if (this.Button8.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y8 == false) // if variable is already false then the command came from controller
                {
                    this.Button8.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button8.BackColor = System.Drawing.Color.Red;
                    parameters.y8 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y8 == true)  // as above
                {
                    this.Button8.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button8.BackColor = System.Drawing.Color.Green;
                    parameters.y8 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button9_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 8;  // Note number
            if (this.Button9.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y9 == false) // if variable is already false then the command came from controller
                {
                    this.Button9.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button9.BackColor = System.Drawing.Color.Red;
                    parameters.y9 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y9 == true)  // as above
                {
                    this.Button9.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button9.BackColor = System.Drawing.Color.Green;
                    parameters.y9 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button10_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 9;  // Note number
            if (this.Button10.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y10 == false) // if variable is already false then the command came from controller
                {
                    this.Button10.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button10.BackColor = System.Drawing.Color.Red;
                    parameters.y10 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y10 == true)  // as above
                {
                    this.Button10.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button10.BackColor = System.Drawing.Color.Green;
                    parameters.y10 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button11_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 10;  // Note number
            if (this.Button11.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y11 == false) // if variable is already false then the command came from controller
                {
                    this.Button11.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button11.BackColor = System.Drawing.Color.Red;
                    parameters.y11 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y11 == true)  // as above
                {
                    this.Button11.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button11.BackColor = System.Drawing.Color.Green;
                    parameters.y11 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button12_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 11;  // Note number
            if (this.Button12.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y12 == false) // if variable is already false then the command came from controller
                {
                    this.Button12.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button12.BackColor = System.Drawing.Color.Red;
                    parameters.y12 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y12 == true)  // as above
                {
                    this.Button12.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button12.BackColor = System.Drawing.Color.Green;
                    parameters.y12 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button13_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 12;  // Note number
            if (this.Button13.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y13 == false) // if variable is already false then the command came from controller
                {
                    this.Button13.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button13.BackColor = System.Drawing.Color.Red;
                    parameters.y13 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y13 == true)  // as above
                {
                    this.Button13.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button13.BackColor = System.Drawing.Color.Green;
                    parameters.y13 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button14_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 13;  // Note number
            if (this.Button14.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y14 == false) // if variable is already false then the command came from controller
                {
                    this.Button14.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button14.BackColor = System.Drawing.Color.Red;
                    parameters.y14 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y14 == true)  // as above
                {
                    this.Button14.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button14.BackColor = System.Drawing.Color.Green;
                    parameters.y14 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button15_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 14;  // Note number
            if (this.Button15.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y15 == false) // if variable is already false then the command came from controller
                {
                    this.Button15.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button15.BackColor = System.Drawing.Color.Red;
                    parameters.y15 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y15 == true)  // as above
                {
                    this.Button15.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button15.BackColor = System.Drawing.Color.Green;
                    parameters.y15 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button16_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 15;  // Note number
            if (this.Button16.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y16 == false) // if variable is already false then the command came from controller
                {
                    this.Button16.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button16.BackColor = System.Drawing.Color.Red;
                    parameters.y16 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y16 == true)  // as above
                {
                    this.Button16.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button16.BackColor = System.Drawing.Color.Green;
                    parameters.y16 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button17_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 16;  // Note number
            if (this.Button17.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y17 == false) // if variable is already false then the command came from controller
                {
                    this.Button17.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button17.BackColor = System.Drawing.Color.Red;
                    parameters.y17 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y17 == true)  // as above
                {
                    this.Button17.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button17.BackColor = System.Drawing.Color.Green;
                    parameters.y17 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button18_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 17;  // Note number
            if (this.Button18.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y18 == false) // if variable is already false then the command came from controller
                {
                    this.Button18.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button18.BackColor = System.Drawing.Color.Red;
                    parameters.y18 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y18 == true)  // as above
                {
                    this.Button18.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button18.BackColor = System.Drawing.Color.Green;
                    parameters.y18 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button19_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 18;  // Note number
            if (this.Button19.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y19 == false) // if variable is already false then the command came from controller
                {
                    this.Button19.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button19.BackColor = System.Drawing.Color.Red;
                    parameters.y19 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y19 == true)  // as above
                {
                    this.Button19.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button19.BackColor = System.Drawing.Color.Green;
                    parameters.y19 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }
        private void Button20_Click(object sender, EventArgs e) // When button clicked on interface
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
            builder.MidiChannel = 0; // MIDI channel 0
            builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
            builder.Data1 = 19;  // Note number
            if (this.Button20.BackColor == System.Drawing.Color.Green) // if the button is active
            {
                if (parameters.y20 == false) // if variable is already false then the command came from controller
                {
                    this.Button20.BackColor = System.Drawing.Color.Red;
                }
                else // otherwise it came from the interface hence y1 needs to be changed
                {
                    this.Button20.BackColor = System.Drawing.Color.Red;
                    parameters.y20 = false;
                }
                builder.Data2 = 0; // Velocity 0 to turn off button on controller
            }
            else // if button is inactive
            {
                if (parameters.y20 == true)  // as above
                {
                    this.Button20.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.Button20.BackColor = System.Drawing.Color.Green;
                    parameters.y20 = true;
                }
                builder.Data2 = 100; // velocity 100 to turn on button on controller
            }
            builder.Build(); // build message
            BCF2000_o.Send(builder.Result); // send message to controller
        }

        private void Fader1_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader1.Value; // interface value
            if (value != parameters.x1) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 0;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x1 = value;
            }
        }
        private void Fader2_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader2.Value; // interface value
            if (value != parameters.x2) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 1;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x2 = value;
            }
        }
        private void Fader3_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader3.Value; // interface value
            if (value != parameters.x3) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 2;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x3 = value;
            }
        }
        private void Fader4_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader4.Value; // interface value
            if (value != parameters.x4) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 3;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x4 = value;
            }
        }
        private void Fader5_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader5.Value; // interface value
            if (value != parameters.x5) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 4;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x5 = value;
            }
        }
        private void Fader6_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader6.Value; // interface value
            if (value != parameters.x6) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 5;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x6 = value;
            }
        }
        private void Fader7_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader7.Value; // interface value
            if (value != parameters.x7) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 6;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x7 = value;
            }
        }
        private void Fader8_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Fader8.Value; // interface value
            if (value != parameters.x8) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 7;  // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x8 = value;
            }
        }

        private void Knob1_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob1.Value; // interface value
            if (value != parameters.x9) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 8; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x9 = value;
            }
        }
        private void Knob2_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob2.Value; // interface value
            if (value != parameters.x10) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 9; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x10 = value;
            }
        }
        private void Knob3_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob3.Value; // interface value
            if (value != parameters.x11) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 10; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x11 = value;
            }
        }
        private void Knob4_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob4.Value; // interface value
            if (value != parameters.x12) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 11; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x12 = value;
            }
        }
        private void Knob5_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob5.Value; // interface value
            if (value != parameters.x13) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 12; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x13 = value;
            }
        }
        private void Knob6_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob6.Value; // interface value
            if (value != parameters.x14) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 13; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x14 = value;
            }
        }
        private void Knob7_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob7.Value; // interface value
            if (value != parameters.x15) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 14; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x15 = value;
            }
        }
        private void Knob8_ValueChanged(object sender, EventArgs e) // If fader value is changed
        {
            float value = (float)this.Knob8.Value; // interface value
            if (value != parameters.x16) // if the current value on the interface differs from stored parameter
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                builder.MidiChannel = 0; // Always channel 0
                builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                builder.Data1 = 15; // Control number
                builder.Data2 = (int)value; // send the value from interface
                builder.Build();
                BCF2000_o.Send(builder.Result);
                parameters.x16 = value;
            }
        }
    }
}

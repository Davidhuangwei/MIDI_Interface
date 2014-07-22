using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Threading;

namespace MIDI_Interface
{
    public class HardwareSetup
    {
        private static ChannelCommand n = ChannelCommand.NoteOn;
        private static InterfaceForm inForm;
        private static SynchronizationContext context; // Synchronisation context for syncing form
        private static int inDeviceID = 0;             // Input device ID, increments with further device additions
        internal static InputDevice BCF2000_i = null;    // MIDI input device object
        internal static OutputDevice BCF2000_o = null;   // MIDI output to send messages to BCF2000
        private static outputMessages outmess;         // output messages stored in this struct
        internal static bool ControlSender = false;    // Set true when handling MIDI messages recieved from controller
        internal static bool FormSender = false;       // Set true when handling messages from Form

        internal HardwareSetup(InterfaceForm mainForm)
        {
            inForm = mainForm;
        }

        private static void start() // Initialises BCF2000 data stream
        {
            BCF2000_i.SysExBufferSize = 4096;
            BCF2000_i.StartRecording();
            return;
        }

        public void initialise()
        {
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
            ControlSender = true;
            sortOutmess(outmess); // use messages to change parameters
            ControlSender = false;
        }

        public void release() // Disposes of all devices and stops recording
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
            return;
        }

        internal static void controlMess(int note, float velocity)
        {
            if (ControlSender == false)
            {
                if (HardwareSetup.BCF2000_i != null)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                    builder.MidiChannel = 0; // Always channel 0
                    builder.Command = ChannelCommand.Controller; // Control message for knobs and faders
                    builder.Data1 = note;  // Control number
                    builder.Data2 = (int)velocity; // send the value from interface
                    builder.Build();
                    BCF2000_o.Send(builder.Result);
                    if (FormSender == false)
                    {
                        inForm.setValue(note, (decimal)velocity);
                    }
                }
            }
        }

        internal static void noteMess(int note, bool onoff)
        {
            if (ControlSender == false)
            {
                if (HardwareSetup.BCF2000_i != null)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder(); // build MIDI Message
                    builder.MidiChannel = 0; // Always channel 0
                    builder.Command = ChannelCommand.NoteOn; // NoteOn message for buttons
                    builder.Data1 = note;  // Note number
                    if (onoff)
                    {
                        builder.Data2 = 100; // send 100 if true
                    }
                    else
                    {
                        builder.Data2 = 0;
                    }
                    builder.Build();
                    BCF2000_o.Send(builder.Result);
                    if (FormSender == false)
                    {
                        inForm.setButton(note, onoff);
                    }
                }
            }
        }

        private void sortOutmess(outputMessages mess) // Will set variables for TFM to be read outside
        {                                             // of program whenever new message is received
            if (mess.isCC)
            {
                float velocity = mess.Mess;
                inForm.setValue(mess.Note, (decimal)velocity);
                if (mess.Note <= 15)
                {
                    parameters.setX(mess.Note, velocity);
                    controlMess(mess.Note, velocity);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Invalid control change, please change note value");
                }
            }
            else
            {
                if (mess.Note <= 20)
                {
                    bool onoff = mess.Mess > 0;
                    parameters.setY(mess.Note, onoff);
                    inForm.setButton(mess.Note, onoff);
                    noteMess(mess.Note, onoff);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Invalid button pressed, please change note value");
                }
            }
            return;
        }

    }
}

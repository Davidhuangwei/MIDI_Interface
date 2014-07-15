using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace MIDI_Interface
{
    public static class parameters
    {

        public static float[] x = new float[16];
        public static bool[] y = new bool[20];

/*        // Button variables
        public static bool _y1 = false;
        public static bool _y2 = false;
        public static bool _y3 = false;
        public static bool _y4 = false;
        public static bool _y5 = false;
        public static bool _y6 = false;
        public static bool _y7 = false;
        public static bool _y8 = false;
        public static bool _y9 = false;
        public static bool _y10 = false;
        public static bool _y11 = false;
        public static bool _y12 = false;
        public static bool _y13 = false;
        public static bool _y14 = false;
        public static bool _y15 = false;
        public static bool _y16 = false;
        public static bool _y17 = false;
        public static bool _y18 = false;
        public static bool _y19 = false;
        public static bool _y20 = false;
*/ 

        public static void setX(int index, float val)
        {
            x[index] = val;
        }

        public static void setY(int index, bool val)
        {
            y[index] = val;
        }



/*        // Accessors for faders and knobs
        public static float[] X
        {
            get { return x; }
            set
            {
                x = value;
                int index = Array.IndexOf(x,value);
                HardwareSetup.controlMess(index, value[index]);
            }
        }

*/

/*        // Accessors for Buttons
        public static bool y1
        {
            get { return _y1; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 0;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y1 = value;
            }
        }
        public static bool y2
        {
            get { return _y2; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 1;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y2 = value;
            }
        }
        public static bool y3
        {
            get { return _y3; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 2;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y3 = value;
            }
        }
        public static bool y4
        {
            get { return _y4; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 3;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y4 = value;
            }
        }
        public static bool y5
        {
            get { return _y5; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 4;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y5 = value;
            }
        }
        public static bool y6
        {
            get { return _y6; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 5;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y6 = value;
            }
        }
        public static bool y7
        {
            get { return _y7; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 6;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y7 = value;
            }
        }
        public static bool y8
        {
            get { return _y8; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 7;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y8 = value;
            }
        }
        public static bool y9
        {
            get { return _y9; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 8;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y9 = value;
            }
        }
        public static bool y10
        {
            get { return _y10; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 9;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y10 = value;
            }
        }
        public static bool y11
        {
            get { return _y11; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 10;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y11 = value;
            }
        }
        public static bool y12
        {
            get { return _y12; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 11;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y12 = value;
            }
        }
        public static bool y13
        {
            get { return _y13; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 12;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y13 = value;
            }
        }
        public static bool y14
        {
            get { return _y14; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 13;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y14 = value;
            }
        }
        public static bool y15
        {
            get { return _y15; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 14;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y15 = value;
            }
        }
        public static bool y16
        {
            get { return _y16; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 15;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y16 = value;
            }
        }
        public static bool y17
        {
            get { return _y17; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 16;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y17 = value;
            }
        }
        public static bool y18
        {
            get { return _y18; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 17;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y18 = value;
            }
        }
        public static bool y19
        {
            get { return _y19; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 18;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y19 = value;
            }
        }
        public static bool y20
        {
            get { return _y20; }
            set
            {
                if (HardwareSetup.ControlSender == false)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();  // relays messages to controller
                    builder.MidiChannel = 0; // MIDI channel 0
                    builder.Command = ChannelCommand.NoteOn; // buttons use note on commands
                    builder.Data1 = 19;  // Note number
                    if (value == true)
                    {
                        builder.Data2 = 100; // Turns on button on controller
                    }
                    else
                    {
                        builder.Data2 = 0;   // Turns off button on controller
                    }
                    builder.Build();
                    HardwareSetup.BCF2000_o.Send(builder.Result);
                }
                _y20 = value;
            }
        }
        */
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.IO;
using System.Configuration;

namespace MIDI_Interface
{
    public static class parameters
    {
        private static char[] delimiter = { ';' };
        private static char[] otherDelimiter = { '\t', ';' };
        private static string[] configLines = Properties.Resources.config.Split(delimiter, 4);
        private static string[] currLine;

        public static void printOut()
        {

            currLine = configLines[0].Split(otherDelimiter, 9);
            for (int i = 0; i < 8; i++)
                float.TryParse(currLine[i + 1], out faderScaleLowerBound[i]);

            currLine = configLines[1].Split(otherDelimiter, 9);
            for (int i = 0; i < 8; i++)
                float.TryParse(currLine[i + 1], out faderScaleUpperBound[i]);

            currLine = configLines[2].Split(otherDelimiter, 9);
            for (int i = 0; i < 8; i++)
                float.TryParse(currLine[i + 1], out knobScaleLowerBound[i]);

            currLine = configLines[3].Split(otherDelimiter, 10);
            for (int i = 0; i < 8; i++)
                float.TryParse(currLine[i + 1], out knobScaleUpperBound[i]);

        }


        public static float[] Control = new float[16];
        public static bool[] Button = new bool[20];

        public static float[] Faders = new float[8];
        public static float[] Knobs = new float[8];

        public static float[] faderScaleUpperBound = new float[8];
        public static float[] faderScaleLowerBound = new float[8];

        public static float[] knobScaleUpperBound = new float[8];
        public static float[] knobScaleLowerBound = new float[8];

        public static void setX(int index, float val)
        {
            Control[index] = val;
            if (!HardwareSetup.FormSender && !HardwareSetup.ControlSender && HardwareSetup.BCF2000_i != null)
            {
                HardwareSetup.controlMess(index, val);
            }

            if (index < 8)
            {
                float increment = (faderScaleUpperBound[index] - faderScaleLowerBound[index]) / (float)127;
                Faders[index] = faderScaleLowerBound[index] + (increment * Control[index]);
            }
            else
            {
                float increment = (knobScaleUpperBound[index - 8] - knobScaleLowerBound[index - 8]) / (float)127;
                Knobs[index - 8] = knobScaleLowerBound[index - 8] + (increment * Control[index]);
            }

        }

        public static void setY(int index, bool val)
        {
            Button[index] = val;

            if (!HardwareSetup.FormSender && !HardwareSetup.ControlSender && HardwareSetup.BCF2000_i != null)
            {
                HardwareSetup.noteMess(index, val);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.IO;
using System.Configuration;

namespace MIDI_Interface
{
    public static class parameters
    {
        private static Properties.Settings My = new Properties.Settings();
        private static InterfaceForm inForm;

        internal static void setForm(InterfaceForm mainForm)
        {
            inForm = mainForm;
        }

        public static void loadScale() // Loads settings for scaling from Properties.Settings
        {

            for (int i = 0; i < 8; i++)
                float.TryParse(My.scaling_hi[i], out knobScaleUpperBound[i]);

            for (int i = 0; i < 8; i++)
                float.TryParse(My.scaling_lo[i], out knobScaleLowerBound[i]);

            for (int i = 0; i < 8; i++)
                setScale(i, knobScaleLowerBound[i], knobScaleUpperBound[i]);

        }

        public static void saveScale()
        {

            StringCollection scale = new StringCollection();
            for (int i = 0; i < 8; i++)
                scale.Add(knobScaleLowerBound[i].ToString());
            My.scaling_lo = scale;
            scale = null;
            scale = new StringCollection();
            for (int i = 0; i < 8; i++)
                scale.Add(knobScaleUpperBound[i].ToString());
            My.scaling_hi = scale;

            My.Save();
        }

        public static float[] Control = new float[16];
        public static bool[] Button = new bool[20];

        public static float[] Faders = new float[8];
        public static float[] Knobs = new float[8];
        public static float[] Pairs = new float[8];

        internal static float[] knobScaleUpperBound = new float[8];
        internal static float[] knobScaleLowerBound = new float[8];

        public static void setScale(int pair, float low, float hi) // Set the scale of a fader/knob pair
        {
            knobScaleLowerBound[pair] = low;
            knobScaleUpperBound[pair] = hi;

            switch (pair)
            {
                case 0:
                    inForm.Pair1_lo.Value = (Decimal)low;
                    inForm.Pair1_hi.Value = (Decimal)hi;
                    break;
                case 1:
                    inForm.Pair2_lo.Value = (Decimal)low;
                    inForm.Pair2_hi.Value = (Decimal)hi;
                    break;
                case 2:
                    inForm.Pair3_lo.Value = (Decimal)low;
                    inForm.Pair3_hi.Value = (Decimal)hi;
                    break;
                case 3:
                    inForm.Pair4_lo.Value = (Decimal)low;
                    inForm.Pair4_hi.Value = (Decimal)hi;
                    break;
                case 4:
                    inForm.Pair5_lo.Value = (Decimal)low;
                    inForm.Pair5_hi.Value = (Decimal)hi;
                    break;
                case 5:
                    inForm.Pair6_lo.Value = (Decimal)low;
                    inForm.Pair6_hi.Value = (Decimal)hi;
                    break;
                case 6:
                    inForm.Pair7_lo.Value = (Decimal)low;
                    inForm.Pair7_hi.Value = (Decimal)hi;
                    break;
                case 7:
                    inForm.Pair8_lo.Value = (Decimal)low;
                    inForm.Pair8_hi.Value = (Decimal)hi;
                    break;
                default:
                    break;
            }

        }

        public static void setX(int index, float val) // Set the value of a fader/knob
        {
            Control[index] = val;
            if (!HardwareSetup.FormSender && !HardwareSetup.ControlSender && (HardwareSetup.BCF2000_i != null && HardwareSetup.BCF2000_o != null))
            {
                HardwareSetup.controlMess(index, val);
            }

            if (index < 8)
            {
                float increment = ((knobScaleUpperBound[index] - knobScaleLowerBound[index]) / (float)127.0) / (float)127.0;
                Faders[index] = Knobs[index] + (increment * val);
            }
            else
            {
                float increment = (knobScaleUpperBound[index - 8] - knobScaleLowerBound[index - 8]) / (float)127.0;
                Knobs[index - 8] = knobScaleLowerBound[index - 8] + (increment * val);
            }



        }

        public static void setY(int index, bool val) // Set the value of a button
        {
            Button[index] = val;

            if (!HardwareSetup.FormSender && !HardwareSetup.ControlSender && HardwareSetup.BCF2000_i != null)
            {
                HardwareSetup.noteMess(index, val);
            }
        }

    }
}

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

        // Contains all variables and methods for changing variable values:
        // -----------------------------------------
        // PUBLIC METHODS:
        //=================
        // loadScale() - load scaling parameters from Properties.Settings
        // saveScale() - save any changed scaling parameters to Properties.Settings
        // setScale(int pair, float low, float, hi) - sets the scaling parameters for a fader/knob pair
        // setSensitivity(int pair, float value) - Sets the sensitivity of the fader for a fader/knob pair
        // setX(int index, float val) - sets the value of Control[index] to val, also updates device and form if necessary,
        //                              changes in Control[] affect Faders[] and/or Knobs[] as well when performed with this
        //                              method
        // setY(int index, bool val) - Changes Button[index] to val, also updates form and device.
        // returnAllX() - returns the Control array
        // returnScaledX() - returns the Faders array
        // returnY() - returns the Button array

        // PUBLIC VARIABLES:
        //=================
        // float Control[16] - Contains unscaled control values, 0 to 7 correspond to the faders, 8 to 15 the knobs (left to right)
        // bool Button[20] - Contains button values, on or off. buttons go left to right then top to bottom so 0 to 7 = top row,
        //                   8 to 15 = second row, and 16 to 19 are the buttons at the bottom right of the controller
        // float Faders[8] - Contains weighted fader values, taking into account fader/knob pairs and scale adjustments
        // float Knobs[8] - Contains weighted knob values, taking into account knob value and scale adjustments

        // INTERNAL METHODS:
        //=================
        // setForm(InterfaceForm mainForm) - as in HardwareSetup

        private static Properties.Settings My = new Properties.Settings();
        private static InterfaceForm inForm;

        public static float[] Control = new float[16];
        public static bool[] Button = new bool[20];

        public static float[] Faders = new float[8];
        public static float[] Knobs = new float[8];

        internal static float[] ScaleUpperBound = new float[8];
        internal static float[] ScaleLowerBound = new float[8];
        internal static float[] FineScaleRatio = new float[8] { 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f };

        internal static void setForm(InterfaceForm mainForm) // sets inForm to the current form
        {
            inForm = mainForm;
        }

        public static void loadScale() // Loads settings for scaling from Properties.Settings
        {

            for (int i = 0; i < 8; i++)
                float.TryParse(My.scaling_hi[i], out ScaleUpperBound[i]);

            for (int i = 0; i < 8; i++)
                float.TryParse(My.scaling_lo[i], out ScaleLowerBound[i]);

            for (int i = 0; i < 8; i++)
                setScale(i, ScaleLowerBound[i], ScaleUpperBound[i]);

        }

        public static void saveScale() // Saves all scaling parameters
        {

            StringCollection scale = new StringCollection();
            for (int i = 0; i < 8; i++)
                scale.Add(ScaleLowerBound[i].ToString());
            My.scaling_lo = scale;
            scale = null;
            scale = new StringCollection();
            for (int i = 0; i < 8; i++)
                scale.Add(ScaleUpperBound[i].ToString());
            My.scaling_hi = scale;
            scale = null;
            scale = new StringCollection();
            for (int i = 0; i < 8; i++)
                scale.Add(FineScaleRatio[i].ToString());
            My.sensitivity = scale;
            My.Save();
        }

        public static void setScale(int pair, float low, float hi) // Set the scale of a fader/knob pair
        {
            ScaleLowerBound[pair] = low;
            ScaleUpperBound[pair] = hi;

            if (inForm != null)
            {
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

        }

        public static void setSensitivity(int pair, float value)
        {
            FineScaleRatio[pair] = value;
            if (HardwareSetup.FormSender == false)
                if (inForm != null)
                {
                    switch (pair)
                    {
                        case 0:
                            inForm.Sensitivity1.Value = (Decimal)value;
                            break;
                        case 1:
                            inForm.Sensitivity2.Value = (Decimal)value;
                            break;
                        case 2:
                            inForm.Sensitivity3.Value = (Decimal)value;
                            break;
                        case 3:
                            inForm.Sensitivity4.Value = (Decimal)value;
                            break;
                        case 4:
                            inForm.Sensitivity5.Value = (Decimal)value;
                            break;
                        case 5:
                            inForm.Sensitivity6.Value = (Decimal)value;
                            break;
                        case 6:
                            inForm.Sensitivity7.Value = (Decimal)value;
                            break;
                        case 7:
                            inForm.Sensitivity8.Value = (Decimal)value;
                            break;
                        default:
                            break;
                    }
                }
            setX(pair, Control[pair]);
        }

        public static void setX(int index, float val) // Set the value of a fader/knob
        {
            Control[index] = val;
            if (!HardwareSetup.FormSender && !HardwareSetup.ControlSender && (HardwareSetup.BCF2000_i != null && HardwareSetup.BCF2000_o != null))
            {
                HardwareSetup.controlMess(index, val);
            }
            // Old method of setting fader values:
            //if (index < 8)
            //{
            //    /*
            //    float increment = (ScaleUpperBound[index] - ScaleLowerBound[index]) / (float)127.0;
            //    Knobs[index] = ScaleLowerBound[index] + (increment * Control[index + 8]);
            //    increment /= (float)127.0;
            //     * Faders[index] = Knobs[index] + (increment * Control[index]);
            //     */
            //    float Range = ScaleUpperBound[index] - ScaleLowerBound[index];
            //    float StepCoarse = Range/127f;
            //    float ValueCoarse = ScaleLowerBound[index] + ((float)Control[index + 8] / 127f) * Range;
            //    float RangeFine = StepCoarse * FineScaleRatio[index];
            //    float ValueFine = (((float)Control[index] - 63f) / 127f) * RangeFine;

            //    Faders[index] = ValueCoarse + ValueFine;
            //}
            //else
            //{
            //    index -= 8;

            //    float Range = ScaleUpperBound[index] - ScaleLowerBound[index];
            //    float ValueCoarse = ScaleLowerBound[index] + ((float)Control[index + 8] / 127f) * Range;
            //    float RangeFine = Range * FineScaleRatio[index];
            //    float ValueFine = (((float)Control[index] - 63f) / 127f) * RangeFine;
            //    Faders[index] = ValueCoarse + ValueFine;

            //    /*               
            //    float increment = (ScaleUpperBound[index] - ScaleLowerBound[index]) / (float)127.0;
            //    Knobs[index] = ScaleLowerBound[index] + (increment * Control[index + 8]);
            //    increment /= (float)127.0;
            //    Faders[index] = Knobs[index] + (increment * Control[index]);
            //     */
            //}

            if (index > 7) // if index is greater than 7, it refers to a knob, hence reduce by 8 for associated pair
                index -= 8;

            float Range = ScaleUpperBound[index] - ScaleLowerBound[index]; // Total range of faders
            float StepCoarse = Range / 127f;
            float ValueCoarse = ScaleLowerBound[index] + (Control[index + 8] * StepCoarse); // Knob value
            float RangeFine = Range * (1f - FineScaleRatio[index]); // Range of fader within coarse range
            if (RangeFine == 0)
                RangeFine += 0.01f; // Make sure faders always do something at least
            float ValueFine = ((Control[index] - 63f) / 127f) * RangeFine;
            Faders[index] = ValueCoarse + ValueFine;

            if (inForm != null)
                inForm.ChangePair(index, Faders[index]);
        }

        public static void setY(int index, bool val) // Set the value of a button
        {
            Button[index] = val;

            if (!HardwareSetup.FormSender && !HardwareSetup.ControlSender && HardwareSetup.BCF2000_i != null)
            {
                HardwareSetup.noteMess(index, val);

                if (inForm != null)
                    inForm.setButton(index, val);
            }

        }

        public static float[] returnAllX() // Returns unscaled array of all controls
        {
            return Control;
        }

        public static float[] returnScaledX() // Returns the array of scaled faders
        {
            return Faders;
        }

        public static bool[] returnY() // Returns array of button values
        {
            return Button;
        }





    }
}

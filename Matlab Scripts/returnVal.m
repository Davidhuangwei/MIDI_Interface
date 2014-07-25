function [ Faders, Buttons ] = returnVal()
%RETURNVAL gets messages from C# MIDI Interface form
%   note = MIDI note
%   vel = velocity

a = MIDI_Interface.parameters.returnY;
b = MIDI_Interface.parameters.returnScaledX;
Faders = double(b);
Buttons = boolean(double(a));

end


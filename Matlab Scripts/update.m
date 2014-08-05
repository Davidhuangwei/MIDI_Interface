[Faders, Buttons] = returnVal();
if mod(count, 2) == 0
    clc
    fprintf('Buttons:\t')
    fprintf('[%d]\t', Buttons)
    fprintf('\nFaders: \t')
    fprintf('[%g]\t', Faders)
    fprintf('\n')
    count = 0;
end
count = count + 1;

if Buttons(19)
    close all;
    pause(1);
    MIDI_Interface.parameters.setY(18, boolean(0));
    FMC_Lardner;
    pause(1);
end

if Buttons(20)
    InterfaceClose; 
end
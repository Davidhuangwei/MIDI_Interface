MIDI = NET.addAssembly('C:\Users\pjb13148\Downloads\MIDI_Interface-master\MIDI_Interface-master\MIDI_Interface\bin\Debug\MIDI_Interface.exe');

Form = MIDI_Interface.InterfaceForm;
%Hardware = MIDI_Interface.HardwareSetup;
Form.Show;
%Hardware.Init();
MIDI_Interface.parameters.setScale(1, 5000, 7000);
MIDI_Interface.parameters.setScale(3, 2, 1024);
MIDI_Interface.parameters.setScale(4, 0, 1);
MIDI_Interface.parameters.setScale(2, 2, 1024);
MIDI_Interface.parameters.setScale(0, 5500000, 6500000);
MIDI_Interface.parameters.setScale(7, 3, 15);
MIDI_Interface.parameters.setScale(6, 3, 15);
count = 0;
pause(1);
A = timer('TimerFcn', 'update', 'Period', 0.25, 'ExecutionMode', 'fixedSpacing');
start(A);
NET.addAssembly('E:\Files\MIDI_Interface\MIDI_Interface\bin\Debug\MIDI_Interface.exe');

Form = MIDI_Interface.InterfaceForm;
Hardware = MIDI_Interface.HardwareSetup;
Form.Show;
pause(1);
A = timer('TimerFcn', 'update', 'Period', 0.25, 'ExecutionMode', 'fixedSpacing');
start(A);
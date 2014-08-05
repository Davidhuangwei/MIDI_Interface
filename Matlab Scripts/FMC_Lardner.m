%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%                                       %
%   Timothy Lardner, 2014               %
%   Centre for Ultrasonic Engineering   %
%   University of Strathclyde           %  
%                                       %
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%% Initialise variables

freq_dispersion = Buttons(17);         % Set to 1 to enable frequency dispersion

reflector_loc = [-5,   75;    % The points at which the transducer will focus
                 0,    75;     
                 2.5,  75;
                 0,    75;
                 -5,   25;
                 0,    25;
                 2.5,  25;
                 0,    25];

reflector_loc = reflector_loc+1;
reflector_loc = reflector_loc.*([Buttons(1:8); Buttons(1:8)]');
reflector_loc(reflector_loc==0) = [];
reflector_loc = reflector_loc-1;

% reflector_loc = [-10, 75];   
             
% reflector_loc = [0 10;
%                  0 100];

reflector_loc_scale = 1e-3;             
centre_freq = Faders(1);
n_cycles = Faders(7);
velocity_at_centre_freq = Faders(2);

save_on_completion = Buttons(18);

image_depth = Faders(5);
grid_points = round(Faders(4));
t_step = round(Faders(8)); % Number of time steps per period
n_transducers = round(Faders(3));
transducer_spacing = 0.2e-3;
transducer_pitch = 0.50e-3;
transducer_element_width = transducer_pitch - transducer_spacing;
%% Generate neccessary variables and input signal

w = 2 * pi * centre_freq;


period = 1 / centre_freq;
pulse_duration = period * n_cycles;
time_at_centre_of_pulse = pulse_duration / 2;

max_time = 2*sqrt(2*(image_depth^2))/velocity_at_centre_freq;
time = 0:period/t_step:max_time;
time_increment = period/t_step;
window = hann(length(time));%TO BE ENTERED
sine_wave = sin(2* pi * centre_freq * time);

input_signal = window(:) .* sine_wave(:);
%% Perform FFT
fft_pts = 2^nextpow2(length(time));
spectrum = fft(input_signal,fft_pts);
spectrum = spectrum(1:length(spectrum)/2);

%build frequency axis
freq_step = 1 / (fft_pts * time_increment);
freq = 0 : freq_step : (fft_pts*freq_step - freq_step)/2;

k = 2*pi*freq./velocity_at_centre_freq;
%% Calculate time delays to each reflector from each transducer
disp('Processing...');

transducer_width = n_transducers*transducer_spacing;
source_x_positions = linspace(-transducer_width/2,transducer_width/2,n_transducers);

x = linspace(-image_depth/2,image_depth/2,grid_points);
z = linspace(0,image_depth,grid_points);

transducer_reflector_lookup = zeros(n_transducers,length(reflector_loc));
reflector_loc = reflector_loc * reflector_loc_scale;

for transducer_idx=1:n_transducers
    for reflector_idx=1:size(reflector_loc,1)
        
        d = sqrt((reflector_loc(reflector_idx,1)-source_x_positions(transducer_idx)).^2 + reflector_loc(reflector_idx,2).^2);
        transducer_reflector_lookup(transducer_idx,reflector_idx) = d;
    end
end
%% Simulate propagation and reflection

% ONLY USING ONE REFLECTOR AT THE MOMENT

velocity = velocity_at_centre_freq;

final_signal = zeros(n_transducers,n_transducers,length(spectrum));
final_transfer_function = zeros(n_transducers,n_transducers,length(spectrum));

for transmit_idx = 1:n_transducers
    for receive_idx = 1:n_transducers
         for reflector_idx= 1:size(reflector_loc,1)
            
            delayTX = transducer_reflector_lookup(transmit_idx,reflector_idx);
            delayRX = transducer_reflector_lookup(receive_idx,reflector_idx);
            total_delay = delayTX + delayRX;
            decay = 1/sqrt(total_delay);
            transfer_function = (exp(-1i .* k .* total_delay));
            final_transfer_function(transmit_idx,receive_idx,:) = squeeze(final_transfer_function(transmit_idx,receive_idx,:)) + transfer_function(:,1);

         end
        delayed_spectrum = spectrum(:).*final_transfer_function(transmit_idx,receive_idx);
%        delayed_spectrum = spectrum(:).*transfer_function(:);
        final_signal(transmit_idx,receive_idx,:) = delayed_spectrum;
    end
end
%% Convert back to time domain and save

time = 0 : 1/freq(end) : 1/freq(end)*(fft_pts/2 - 1);

final_time_traces = zeros(n_transducers,n_transducers,fft_pts/2);

for transmit_idx = 1:n_transducers
    clc;
    disp('Preparing final time traces for imaging');
    fprintf('%g of %g completed...',transmit_idx,n_transducers);
    for receive_idx = 1:n_transducers
        final_time_traces(transmit_idx,receive_idx,:) = ifft(final_signal(transmit_idx,receive_idx,:),fft_pts/2);
    end
end
if(save_on_completion == 1)
    fprintf('\n\nSaving...');
    save('Transducer_data');
end
clc;
%% Image results
tfm_time = [];
output_image = zeros(grid_points,grid_points);
disp('Performing initial TFM calculation...');
for xx=1:grid_points
    tic
    for zz=1:grid_points
        
        for tx=1:n_transducers
            for rx=1:n_transducers
                distanceTX = sqrt((x(xx)-source_x_positions(tx)).^2 + z(zz).^2);
                distanceRX = sqrt((x(xx)-source_x_positions(rx)).^2 + z(zz).^2);
                delay = (distanceTX + distanceRX) / velocity_at_centre_freq;
                trace_idx = round(delay/(time(2)-time(1)));
                if trace_idx > fft_pts/2
                    trace_idx = mod(trace_idx, fft_pts/2);
                end
                if trace_idx < 1
                    trace_idx = 1;
                end
                
                output_image(xx,zz) = output_image(xx,zz) + (real(final_time_traces(tx,rx,trace_idx)));
            end
        end
    end
    clc;
    
    tfm_time = [tfm_time toc];
    average_tfm = sum(tfm_time) / length(tfm_time);
    disp('Generating TFM image.');
    fprintf('\n%g%% completed.\n',(xx/grid_points)*100);
    fprintf('Taking an average time of %g seconds to process each row',average_tfm);
    fprintf('\nEstimated %g seconds remaining...',average_tfm*(grid_points-xx));
end
clc;
output_image = abs(output_image);
output_image = output_image / max(output_image(:));
output_image = 20*log10(output_image);
imagesc(z,x,output_image);
caxis([-20 0]);
if(save_on_completion==1)
    save('Transducer_image','output_image');
end
disp('Done');



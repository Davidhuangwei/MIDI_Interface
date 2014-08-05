stop(A);

if Form ~= 0
    Form.Close;
else
    Hardware.release();
end

clear Form Hardware MIDI
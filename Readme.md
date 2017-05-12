# Fear The Sphere :white_circle: :white_circle:
B.Sc project made by students on the M.Sc in Media Technology programme. 

%%%%%%%%%%%%%
START PROGRAM
%%%%%%%%%%%%%

Setup "hotspot" or network and connect with PC and tablet.

Pair the sphero via bluetooth to the PC.

Change name of spherodevice in the main source file to the name of sphero ("Sphero-WRG" etc).

Place sphero in the middle of the tracker and wake it.

Change IP of the server in main.cpp to equal the ip of the PC.

Build and run the Visual Studio solution.

Wait up to 15 seconds for sphero to connect and move (double check sphero bluetooth connection and and name if it doesnt succeed).

Start the unity program on the tablet and connect to the previous IP adress (Server says "Connected" if succeeded).

Point tablet towards marker until it says "TRACKING FOUND".

Select a series of points on the screen where the sphero shall move to. (Don't lose tracking while choosing points)

Press "Send coords" and wait for sphero to move to them all

%%%%%%%%%%%%%%%%
Calibrate camera
%%%%%%%%%%%%%%%%

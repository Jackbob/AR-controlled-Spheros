# Fear The Sphere :white_circle: :white_circle:
B.Sc project made by students on the M.Sc in Media Technology programme. 

%%%%%%%%%%%%%
START PROGRAM
%%%%%%%%%%%%%

RETARGET SOLUTION TO WORK ON NEW WINDOWS PC (BUILDS ONLY FOR 64BIT)

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

Print out the image "chess.jpg"

Place the origin of the chess image on the marker's origin

Run "CameraCalibration.cpp"

Press 'g' to start calibration.

You're supposed to take 20 images.
The first one should be with the chess image placed as described above.

Try to cover the entire camera view by moving the image around for the remaining 19 images.
(The image should not be placed on the floor for every image)

Done. Start the program.
For more info. read the file "..."

%%%%%%%%%%%%%%%%%%
Calculate new base
%%%%%%%%%%%%%%%%%%

Run program

Make the sphero red

Place the sphero in four different positions. Press 'b' when sphero is in place.
The first three positions are marked in order on the playground.
The last position is in origo (middle of the marker)

Done.

Press 'c' any time during the program to make a new base or start over.
For more info. read the file "..."
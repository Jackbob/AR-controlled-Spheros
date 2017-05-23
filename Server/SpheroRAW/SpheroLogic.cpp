#include "SpheroLogic.h"


//Constructor and sphero connector
SpheroLogic::SpheroLogic(const char* name)
{
	spheroName = name;
	device = SpheroRAW_Create(name);
	while(!spheroConnected())
		device->connect();
	PrintDeviceStatus("Connecting: ", device);
	device->abortMacro();
	targetPositions = new std::vector<std::pair<float, float>>;
	moving = false;
}

//Destructor
SpheroLogic::~SpheroLogic()
{
	delete targetPositions;
	SpheroRAW_Destroy(device);
}

//Sphero movement function (to it's targets) (MUST BE CALLED AS SEPERATE THREAD)
void SpheroLogic::moveSphero()
{

	if (!moving)
	{
		device->abortMacro();
		float dist;
		bool finished = false;
		commandCount = 0;
		moving = true;
		//Loop through targets and move towards them in order
		for (auto target : *targetPositions) {
				do {
					dist = distToPoint(X, Y, target.first, target.second);
					calculatePath(target);
					commandCount++;
					Sleep(200);
				} while (dist > STOP_RADIUS);
		}
		moving = false;
	}
	else
	{
		std::cout << "Already issuing move command to sphero" << std::endl;
		return;
	}
}

//Func for moving sphero with keys
void SpheroLogic::keyMove()
{
	if (GetAsyncKeyState('W'))
	{
		device->abortMacro();
		device->roll(100, 0, 1);
		
	}
	else if (GetAsyncKeyState('S'))
	{
		device->abortMacro();
		device->roll(100, 180, 1);
	}
	else if (GetAsyncKeyState('A'))
	{
		device->abortMacro();
		device->roll(100, 270, 1);
	}
	else if (GetAsyncKeyState('D'))
	{
		device->abortMacro();
		device->roll(100, 90, 1);
	}
	else if (GetAsyncKeyState('R'))
	{
		device->abortMacro();
		device->setRGBLedOutput(255, 0, 0, true);
	}
	else if (GetAsyncKeyState('G'))
	{
		device->abortMacro();
		device->setRGBLedOutput(0, 255, 0, true);
	}
	else if (GetAsyncKeyState('B'))
	{
		device->abortMacro();
		device->setRGBLedOutput(0, 0, 255, true);
	}
	else if (GetAsyncKeyState('F'))
	{
		device->abortMacro();
		device->roll(0, 0, 0);
	}
}

bool SpheroLogic::spheroClick(float clickedX, float clickedY)
{
	if (distToPoint(X, Y, clickedX, clickedY) <= 10)
		return true;

	return false;
}

int SpheroLogic::getSpheroArrivals()
{
	if (prevTargetsRemaining != currentTargetsRemaining)
	{
		prevTargetsRemaining = currentTargetsRemaining;
		return currentTargetsRemaining;
	}

	return 0;
}

void SpheroLogic::reconnect()
{
	if (!spheroConnected())
	{
		std::cout << spheroName << "  has disconnected, attempting to reconnect....	";
		device = SpheroRAW_Create(spheroName);
		Sleep(1000);
		while (!spheroConnected())
			device->connect();

		PrintDeviceStatus("Connecting: ", device);
		Sleep(1000);
	}
}

//Sets positions for the sphero targets
void SpheroLogic::setTarget(std::string targetString)
{
	//Set new target if sphero not moving
	if (!moving)
	{
		float x, y;
		std::stringstream stream = std::stringstream(targetString);
		targetPositions->clear();
		stream >> x;
		//Read tablet output
		while (stream >> y && stream >> x)
		{
			std::cout << std::endl << -x*100.0f << "     " << y*100.0f << std::endl;
			targetPositions->push_back(std::make_pair(-x * 100.0f, y*100.0f));
		}
		currentTargetsRemaining = targetPositions->size();
		prevTargetsRemaining = currentTargetsRemaining;
	}
}

bool SpheroLogic::spheroConnected()
{
	if(device->state() == SpheroState_Connected)
		return true;

	return false;
}

//Public func for printing sphero status
void SpheroLogic::printDeviceStatus(std::string action)
{
	PrintDeviceStatus(action, device);
}

//Tests the sphero movement (only direction)
void SpheroLogic::testMove()
{
	int angle = getAngle(targetPositions->at(0));
	std::cout << "Sphero Position: " << X << " " << Y << std::endl;
	std::cout << "Target Position: " << targetPositions->at(0).first << " " << targetPositions->at(0).second << std::endl;
	device->abortMacro();
	device->roll(60, angle, 1);
}

//Sets the orientation of sphero
void SpheroLogic::setOrientation()
{	
	//Read current position and roll sphero
	Sleep(5000);
	float startX = X, startY = Y;
	device->abortMacro();
	device->roll(40, 0, 1);

	//Read angle of previous movement
	Sleep(5000);
	setOffsetAngle(startX, startY);

}

//Prints sphero status to console
void SpheroLogic::PrintDeviceStatus(std::string action, ISpheroDevice * device)
{
	std::cout << "Action: " << action << " Result: ";

	if (device == nullptr) {
		std::cout << "Error - Sphero handle is invalid" << std::endl;
		return;
	}

	switch (device->state()) {
	case SpheroState_None: { std::cout << "SpheroRAW not initialized" << std::endl; break; }
	case SpheroState_Error_BluetoothError: { std::cout << "Error - Couldn't initialize Bluetooth stack" << std::endl; break; }
	case SpheroState_Error_BluetoothUnavailable: { std::cout << "Error - No valid Bluetooth adapter found" << std::endl; break; }
	case SpheroState_Error_NotPaired: { std::cout << "Error - Specified Sphero not Paired" << std::endl; break; }
	case SpheroState_Error_ConnectionFailed: { std::cout << "Error - Connecting failed" << std::endl; break; }
	case SpheroState_Disconnected: { std::cout << "Sphero disconnected" << std::endl; break; }
	case SpheroState_Connected: { std::cout << "Sphero connected" << std::endl; break; }
	}

	std::cout << std::endl;
}

//Calculate distance from one point to another
float SpheroLogic::distToPoint(float X1, float Y1, float Xtarget, float Ytarget)
{
	return sqrt(pow(X1-Xtarget, 2.0f) + pow(Y1-Ytarget,2.0f));
}

//Get angle and move towards target
void SpheroLogic::calculatePath(std::pair<float, float> target)
{
	float distance = distToPoint(X, Y, target.first, target.second);
	int angle = getAngle(target);

	//Stop sphero if within STOP_RADIUS
	if (distance < STOP_RADIUS)
	{
		std::cout << "STOPPING";
		std::cout << std::endl << "Distance to point:  " << distance << std::endl;

		currentTargetsRemaining--;
		device->roll(0,  angle, 0);
		
	}
	//Roll sphero slow if within CLOSE_RADIUS
	else if (distance < CLOSE_RADIUS && (abs(prevAngle - angle) > ACCEPTED_ANGLE_OFFSET ||  commandCount % CMD_WAIT == 0))
	{
		std::cout << std::endl << "Distance to point:  " << distance << std::endl;
		std::cout << "Moving, close" << std::endl;
		prevAngle = angle;
		
		device->roll(30, angle, 2);
	}
	//Roll sphero faster if far away
	else if (abs(prevAngle - angle) > ACCEPTED_ANGLE_OFFSET || commandCount % CMD_WAIT == 0)
	{
		std::cout << std::endl << "Distance to point:  " << distance << std::endl;
		std::cout << "Moving, far" << std::endl;
		prevAngle = angle;

		device->roll(35, angle, 2);
	}
	//Clear sphero command buffer every 10th command
	else if (commandCount % 10 == 0)
		device->abortMacro();
}

//Reconnect sphero
void SpheroLogic::rest()
{
	device->sleep();
	while (!spheroConnected())
		device->connect();
	PrintDeviceStatus("Connecting: ", device);
}

//Change sphero color
void SpheroLogic::changeColor(int R, int G, int B)
{
	device->abortMacro();
	device->setRGBLedOutput(R, G, B, 1);
}

//Calculates offset angle for sphero
void SpheroLogic::setOffsetAngle(float startX, float startY)
{

	float angle = atan2(Y - startY, X - startX);
	angle = angle * (180.0f / 3.14f);

	if (angle < 180.0f && angle > 90.0f)
		angle = 450.0f - angle;
	else
		angle = 90.0f - angle;

	std::cout << "Angle:   " << angle << "     ";

	offAngle = angle;
}

//Updates spheros current possition (cm from middle)
void SpheroLogic::updateSpheroPos(float Xpos, float Ypos)
{
	X = Xpos/10.0f;
	Y = Ypos/10.0f;
}

//Calculate angle to target
int SpheroLogic::getAngle(std::pair<float, float> target)
{
	//Universal angle to target
	float angle = atan2(target.second - Y , target.first - X);
	angle = angle * (180.0f / 3.14f);

	//To sphero angle
	if (angle < 180.0f && angle > 90.0f)
		angle = 450.0f - angle;
	else
		angle = 90.0f - angle;

	//Compensate for offset
	angle = angle - offAngle;
	if (angle < 0)
		angle = 360 + angle;

	return int(angle);
}



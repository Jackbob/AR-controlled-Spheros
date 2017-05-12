#include "SpheroLogic.h"



SpheroLogic::SpheroLogic(const char* name)
{
	device = SpheroRAW_Create(name);
	while(!spheroConnected())
		device->connect();
	PrintDeviceStatus("Connecting: ", device);
	device->setAutoReconnect();
	device->abortMacro();
	targetPositions = new std::vector<std::pair<float, float>>;
	moving = false;
}


SpheroLogic::~SpheroLogic()
{
	delete targetPositions;
	SpheroRAW_Destroy(device);
}



void SpheroLogic::moveSphero()
{

	if (!moving)
	{
		//device->eraseOrbBasicStorage(0);
		//device->abortMacro();
		float dist;
		bool finished = false;
		commandCount = 0;
		moving = true;
		for (auto target : *targetPositions) {
				do {
					calculatePath(target);
					Sleep(200);
					commandCount++;
					dist = distToPoint(X, Y, target.first, target.second);
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

void SpheroLogic::setTarget(std::string targetString)
{
	if (!moving)
	{
		float x, y;
		std::stringstream stream = std::stringstream(targetString);
		targetPositions->clear();
		while (stream >> x && stream >> y)
		{
			std::cout << -y << "     " << x << std::endl;
			targetPositions->push_back(std::make_pair(-y * 100.0f, x*100.0f));
		}
	}
}

bool SpheroLogic::spheroConnected()
{
	if(device->state() == SpheroState_Connected)
		return true;

	return false;
}

void SpheroLogic::printDeviceStatus(std::string action)
{
	PrintDeviceStatus(action, device);
}

void SpheroLogic::testMove()
{
	int angle = getAngle(targetPositions->at(0));
	std::cout << "Sphero Position: " << X << " " << Y << std::endl;
	std::cout << "Target Position: " << targetPositions->at(0).first << " " << targetPositions->at(0).second << std::endl;
	device->abortMacro();
	device->roll(60, angle, 1);
}

void SpheroLogic::setOrientation()
{	
	device->setRGBLedOutput(255, 0, 0, 1);
	device->abortMacro();
	device->roll(40, 0, 1);

	Sleep(6000);
	setOffsetAngle();

}


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


float SpheroLogic::distToPoint(float X1, float Y1, float Xtarget, float Ytarget)
{
	return sqrt(pow(X1-Xtarget, 2.0f) + pow(Y1-Ytarget,2.0f));
}

//Get angle and move towards target
void SpheroLogic::calculatePath(std::pair<float, float> target)
{
	float distance = distToPoint(X, Y, target.first, target.second);
	std::cout << std::endl << "Distance to point:  " <<  distance << std::endl;
	int angle = getAngle(target);
	if (target == *(targetPositions->end()-1) && distance < STOP_RADIUS)
	{
		
		
		device->roll(0,  angle, 0);
		device->abortMacro();
	}
	else if (distance < CLOSE_RADIUS && (abs(prevAngle - angle) > ACCEPTED_ANGLE_OFFSET ||  commandCount % CMD_WAIT == 0))
	{
		std::cout << "Moving, close" << std::endl;
		prevAngle = angle;
		
		device->roll(30, angle, 2);
	}
	else if (abs(prevAngle - angle) > ACCEPTED_ANGLE_OFFSET || commandCount % CMD_WAIT == 0)
	{
		std::cout << "Moving, far" << std::endl;
		prevAngle = angle;

		device->roll(40, angle, 2);
	}
	else if (commandCount % 10 == 0)
		device->abortMacro();
}

void SpheroLogic::rest()
{
	device->sleep();
	while (!spheroConnected())
		device->connect();
	PrintDeviceStatus("Connecting: ", device);
}

void SpheroLogic::setOffsetAngle()
{

	float angle = atan2(Y,X);
	angle = angle * (180.0f / 3.14f);

	if (angle < 180.0f && angle > 90.0f)
		angle = 450.0f - angle;
	else
		angle = 90.0f - angle;

	std::cout << "Angle:   " << angle << "     ";

	offAngle = angle;
}

void SpheroLogic::updateSpheroPos(float Xpos, float Ypos)
{
	X = Xpos/10.0f;
	Y = Ypos/10.0f;
}

//Calculate angle to target
int SpheroLogic::getAngle(std::pair<float, float> target)
{

	float angle = atan2(target.second - Y , target.first - X);
	angle = angle * (180.0f / 3.14f);

	//std::cout << "Angle:   " << angle << "     ";
	
	if (angle < 180.0f && angle > 90.0f)
		angle = 450.0f - angle;
	else
		angle = 90.0f - angle;

	//std::cout << "SpheroAngle:     " << angle;
	angle = angle - offAngle;
	if (angle < 0)
		angle = 360 + angle;

	//std::cout << "    offsettangle:  " << angle << std::endl;

	return int(angle);
}



#pragma once

#include <string>
#include "SpheroRAWItf.h"
#include <windows.h>
#include <iostream>
#include <sstream>

#using <System.dll>

const float STOP_RADIUS = 30.0f;
const float CLOSE_RADIUS = 50.0f;
const int CMD_WAIT = 5;
const int ACCEPTED_ANGLE_OFFSET = 5;

public ref class SpheroLogic
{
private:
	ISpheroDevice* device;
	std::vector<std::pair<float, float>>* targetPositions;
	const char* spheroName;
	float X, Y, prevX, prevY;
	bool moving;
	int commandCount;
	int offAngle;
	//int prevAngle, currentTargetsRemaining, prevTargetsRemaining;

	void PrintDeviceStatus(std::string action, ISpheroDevice* device);
	float distToPoint(float X, float Y, float Xtarget, float Ytarget);
	void calculatePath(std::pair<float,float> target);
	void setOffsetAngle(float startX, float startY);
	
	int getAngle(std::pair<float, float> target);

public:
	SpheroLogic(const char* name);
	~SpheroLogic();
	int prevAngle, currentTargetsRemaining, prevTargetsRemaining;

	void moveSphero();
	void keyMove();
	void setTarget(std::string targetString);
	bool spheroConnected();
	void printDeviceStatus(std::string action);
	void updateSpheroPos(float Xpos, float Ypos);
	void testMove();
	void setOrientation();
	void rest();
	void changeColor(int R, int G, int B);
	float getSpheroX() { return X; }
	float getSpheroY() { return Y; }
	bool spheroClick(float clickedX, float clickedY);
	int getSpheroArrivals();

	void reconnect();
	void disconnect() { device->disconnect(); }
};


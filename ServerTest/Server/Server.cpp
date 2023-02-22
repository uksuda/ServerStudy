#include <iostream>
#include "MainServer.h"
#include "Log.h"

int main(int argc, char* argv[])
{
	MainServer mTest;
	bool isStart = mTest.initMainServer();
	if (isStart == false)
	{
		//printf("%s\n", "server start failed");
		CLog::log("server start failed");
		Sleep(10000);
	}

	mTest.resetServerTime();
	mTest.startServer();

	return 0;
}
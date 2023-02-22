#pragma once

#include "BaseDefine.h"
//#include <iostream>
//#include <stdarg.h>

#define LOG_TIME_LENGTH 32
#define LOG_LENGTH 1024

class CLog
{
private:
	CLog() {}
	~CLog() {}

public:
	static bool createLogFile();
	static void log(const char* szMessageFormat, ...);
	static void fileLog(std::string& strLog);

private:
	static std::string getLogFileName();
};
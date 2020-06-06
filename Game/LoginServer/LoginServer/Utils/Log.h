#pragma once

#include "BaseDefine.h"

constexpr unsigned char LOG_TIME_LENGTH = 32;
constexpr unsigned short LOG_LENGTH = 1024;

class Log
{
private:
	Log() = delete;
	~Log() {}

public:
	static bool createLogFile();
	static void log(const char* szMessageFormat, ...);
	static void fileLog(std::string& strLog);
};
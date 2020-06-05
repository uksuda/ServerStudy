#pragma once

#include <string>

class Utils
{
private:
	Utils() = delete;
	~Utils() {}

public:
	enum class E_TIME_VALUE_TYPE
	{
		E_YEAR = 0,
		E_MONTH,
		E_DAY,
		E_HOUR,
		E_MINUTE,
		E_SECOND
	};

public:
	unsigned short getCurrentTime();
	std::string getCurrentTime(bool isOnlyDate = false);
	std::string getLogFileName();
};
#pragma once

#include "BaseDefine.h"

class Utils
{
private:
	Utils() = delete;
	~Utils() {}

public:
	enum class E_TIME_TYPE
	{
		E_YEAR = 0,
		E_MONTH,
		E_DAY,
		E_HOUR,
		E_MIN,
		E_SEC,
		E_DAY_OF_WEEK,
	};

public:
	// 0000-00-00 00:00:00
	static WORD getCurrentTime(E_TIME_TYPE eType);
	static WORD getCurrentTime(E_TIME_TYPE eType, SYSTEMTIME& currentTime);
	static std::string getCurrentTime(bool bOnlyDate = false);
	static std::string getLogFileName();
};
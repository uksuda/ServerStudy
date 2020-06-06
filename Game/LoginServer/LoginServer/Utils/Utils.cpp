#include "Utils.h"

WORD Utils::getCurrentTime(E_TIME_TYPE eType)
{
	SYSTEMTIME currentTime;
	memset(&currentTime, 0, sizeof(SYSTEMTIME));
	GetLocalTime(&currentTime);
	
	return getCurrentTime(eType, currentTime);
}

WORD Utils::getCurrentTime(E_TIME_TYPE eType, SYSTEMTIME& currentTime)
{
	switch (eType)
	{
	case E_TIME_TYPE::E_YEAR:
		return currentTime.wYear;
	case E_TIME_TYPE::E_MONTH:
		return currentTime.wMonth;
	case E_TIME_TYPE::E_DAY:
		return currentTime.wDay;
	case E_TIME_TYPE::E_HOUR:
		return currentTime.wHour;
	case E_TIME_TYPE::E_MIN:
		return currentTime.wMinute;
	case E_TIME_TYPE::E_SEC:
		return currentTime.wSecond;
	case E_TIME_TYPE::E_DAY_OF_WEEK:
		return currentTime.wDayOfWeek;
	default:
		return 0;
	}
}

std::string Utils::getCurrentTime(bool bOnlyDate)
{
	time_t t = time(nullptr);

	//tm* timeTM = localtime(&t);
	//std::string strTime;
	//std::strftime(&strTime[0], 100, "%Y-%m-%d %H:%M:%S", localtime(&t));

	std::stringstream ss;
	
	if (bOnlyDate == true)
	{
		ss << std::put_time(localtime(&t), "%Y-%m-%d");
	}
	else
	{
		ss << std::put_time(localtime(&t), "%Y-%m-%d %H:%M:%S");
	}

	return ss.str();
}

std::string Utils::getLogFileName()
{
	std::string strFileName = "log\\log_";
	std::string strDate = getCurrentTime(true);

	strFileName.append(strDate);
	strFileName.append(".txt");
	return strFileName;
}

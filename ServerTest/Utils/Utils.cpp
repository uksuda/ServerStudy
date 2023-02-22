#include "Utils.h"

std::string Utils::getCurrentTime(bool bOnlyDate)
{
	time_t t = time(nullptr);

	//tm* timeTM = localtime(&t);
	std::stringstream ss;

	if (bOnlyDate)
	{
		ss << std::put_time(localtime(&t), "%Y-%m-%d");
	}
	else
	{
		ss << std::put_time(localtime(&t), "%Y-%m-%d %H:%M:%S");
	}

	return ss.str();
}

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
	case E_TIME_TYPE::E_TYPE_YEAR:
		return currentTime.wYear;
	case E_TIME_TYPE::E_TYPE_MONTH:
		return currentTime.wMonth;
	case E_TIME_TYPE::E_TYPE_DAY:
		return currentTime.wDay;
	case E_TIME_TYPE::E_TYPE_HOUR:
		return currentTime.wHour;
	case E_TIME_TYPE::E_TYPE_MIN:
		return currentTime.wMinute;
	case E_TIME_TYPE::E_TYPE_SEC:
		return currentTime.wSecond;
	default :
		return 0;
	}
}

std::string Utils::getIPtoString(SOCKADDR_IN sockAddr)
{
	char szIP[INET_ADDRSTRLEN];
	memset(szIP, 0, sizeof(szIP));

	inet_ntop(AF_INET, &sockAddr.sin_addr, szIP, INET_ADDRSTRLEN);
	//std::string strIP = szIP;
	//return strIP;
	return szIP;
}

ULONG Utils::getIPtoValue(std::string& strIP)
{
	// SOCKADDR_IN temp.sin_addr;
	ULONG uValue = 0;
	inet_pton(AF_INET, strIP.c_str(), &uValue);
	return uValue;
}

std::string Utils::convertToString(std::wstring wstrConvert)
{
	/*std::string strTemp;
	strTemp.assign(wstrConvert.begin(), wstrConvert.end());
	return strTemp;*/
	//return std::string(wstrConvert.begin(), wstrConvert.end());
	std::string str(wstrConvert.length(), 0);
	std::transform(wstrConvert.begin(), wstrConvert.end(), str.begin(), [](wchar_t c) {return static_cast<char>(c); });
	return str;
}

std::wstring Utils::convertToWstring(std::string strConvert)
{
	/*std::wstring wstrTemp;
	wstrTemp.assign(strConvert.begin(), strConvert.end());
	return wstrTemp;*/
	return std::wstring().assign(strConvert.begin(), strConvert.end());
}
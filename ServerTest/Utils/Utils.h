#pragma once

#include "BaseDefine.h"

class Utils
{
private:
	Utils() {}
	~Utils() {}

public:
	enum class E_TIME_TYPE
	{
		E_TYPE_YEAR = 0,
		E_TYPE_MONTH,
		E_TYPE_DAY,
		E_TYPE_HOUR,
		E_TYPE_MIN,
		E_TYPE_SEC
	};

public:
	// 0000-00-00 00:00:00
	static std::string getCurrentTime(bool bOnlyDate = false);
	static WORD getCurrentTime(E_TIME_TYPE eType);
	static WORD getCurrentTime(E_TIME_TYPE eType, SYSTEMTIME& currentTime);

	static std::string getIPtoString(SOCKADDR_IN sockAddr);
	static ULONG getIPtoValue(std::string& strIP);

	static std::string convertToString(std::wstring wstrConvert);
	static std::wstring convertToWstring(std::string strConvert);

	template<typename T>
	static T getRandomValue(T min, T max)
	{
		std::random_device rd;
		std::mt19937_64 mt(rd());
		std::uniform_int_distribution<T> dis(min, max);
		return dis(mt);
	}
};
#include "Log.h"
#include "Utils.h"

bool Log::createLogFile()
{
	CreateDirectory(L"log", nullptr);

	std::string strLogFileName = Utils::getLogFileName();
	FILE* pFile = fopen(strLogFileName.c_str(), "at");
	if (pFile == nullptr)
	{
		printf("%s fail %d\n", __func__, GetLastError());
		return false;
	}

	fclose(pFile);	
	return true;
}

void Log::log(const char* szMessageFormat, ...)
{
	if (szMessageFormat == nullptr)
	{
		return;
	}

	std::string strTime = Utils::getCurrentTime();
	char szLog[LOG_TIME_LENGTH + LOG_LENGTH];
	memset(szLog, 0, sizeof(szLog));

	char szLogFormat[LOG_LENGTH];
	memset(szLogFormat, 0, sizeof(szLogFormat));

	va_list vars;
	va_start(vars, szMessageFormat);
	vsnprintf(szLogFormat, sizeof(szLogFormat), szMessageFormat, vars);
	va_end(vars);
	
	snprintf(szLog, sizeof(szLog), "%s %s\n", strTime.c_str(), szLogFormat);
	std::string strLog = szLog;

	fileLog(strLog);
}

void Log::fileLog(std::string& strLog)
{
	if (strLog.empty() == true)
	{
		return;
	}

	std::string strLogFileName = Utils::getLogFileName();
	FILE* pFile = fopen(strLogFileName.c_str(), "at");
	if (pFile == nullptr)
	{
		printf("%s fail %d\n", __func__, GetLastError());
		return;
	}

	fprintf(pFile, strLog.c_str());
	fclose(pFile);
}
#include "Log.h"
#include "Utils.h"


bool CLog::createLogFile()
{
	CreateDirectory(L"log", nullptr);

	std::string strLogFileName = getLogFileName();
	FILE* pFile = fopen(strLogFileName.c_str(), "at");
	if (pFile == nullptr)
	{
		printf("%s fail %d\n", __FUNCTION__, GetLastError());
		return false;
	}

	fclose(pFile);

	return true;
}

void CLog::log(const char* szMessageFormat, ...)
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
	_vsnprintf(szLogFormat, sizeof(szLogFormat), szMessageFormat, vars);
	va_end(vars);

	_snprintf(szLog, sizeof(szLog), "%s %s\n", strTime.c_str(), szLogFormat);

	printf("%s\n", szLog);

	std::string strLog = szLog;
	fileLog(strLog);
}

void CLog::fileLog(std::string& strLog)
{
	if (strLog.empty() == true)
	{
		return;
	}

	std::string strLogFileName = getLogFileName();
	FILE* pFile = fopen(strLogFileName.c_str(), "at");
	if (pFile == nullptr)
	{
		printf("%s fail %d\n", __FUNCTION__, GetLastError());
		return;
	}

	fprintf(pFile, strLog.c_str());

	fclose(pFile);
}

std::string CLog::getLogFileName()
{
	std::string strFileName = "log\\log_";
	std::string strDate = Utils::getCurrentTime(true);

	strFileName.append(strDate);
	strFileName.append(".txt");
	return strFileName;
}
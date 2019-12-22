#include "Log.h"
#include <iostream>
#include <stdarg.h>

#define LOG_TEXT_LENGTH 256

CLog::CLog()
{

}

CLog::~CLog()
{

}

void CLog::LOG(const char* szMsg, ...)
{
	char szLog[LOG_TEXT_LENGTH];
	memset(szLog, 0, sizeof(szLog));

	va_list va;
	va_start(va, szMsg);
	vsnprintf(szLog, sizeof(szLog), szMsg, va);
	va_end(va);

	printf(szLog);
}
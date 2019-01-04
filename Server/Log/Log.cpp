#include "Log.h"
#include <stdio.h>

CLog::CLog()
{

}

CLog::~CLog()
{

}

void CLog::LOG(const char* szMsg)
{
	if (szMsg == nullptr)
		return;

	printf("%s\n", szMsg);	
}
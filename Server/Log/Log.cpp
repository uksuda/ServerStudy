#include "Log.h"
#include <stdio.h>

CLog::CLog()
{

}

CLog::~CLog()
{

}

void CLog::LOG(int iErrorNo)
{
	printf("Error Occured : %d\n", iErrorNo);
}

void CLog::LOG(const char* szMsg)
{
	printf("%s\n", szMsg);
}

void CLog::LOG(const char* szErrorName, int iErrorNo)
{
	printf("%s Error : %d\n", szErrorName, iErrorNo);
}
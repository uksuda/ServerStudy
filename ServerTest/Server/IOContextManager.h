#pragma once

#include "BaseDefine.h"

#define MAX_SEND_IO_COUNT	100

#define IOCONTEXT_MGR IOContextManager::getInstance()

class IOContextManager
{
DECLARE_SINGLETON(IOContextManager);

public:
	~IOContextManager();

public:
	bool initializeIOContextManager();

	stSendIOData* getSendIO();
	void returnSendIO(stSendIOData* pSendIO);

private:
	std::stack<stSendIOData*> m_stackSendIO;
	CRITICAL_SECTION m_CriticalSection;

public:
	void release();
};
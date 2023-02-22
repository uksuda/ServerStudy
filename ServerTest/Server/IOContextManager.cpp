#include "IOContextManager.h"

IMPLEMENT_SINGLETON(IOContextManager);

IOContextManager::IOContextManager()
{
	
}

IOContextManager::~IOContextManager()
{
	release();
}

bool IOContextManager::initializeIOContextManager()
{
	while (m_stackSendIO.size() < MAX_BUFFER_SIZE + 1)
	{
		stSendIOData* pSendIO = new stSendIOData;
		memset(pSendIO, 0, sizeof(stSendIOData));

		m_stackSendIO.emplace(pSendIO);
	}

	InitializeCriticalSection(&m_CriticalSection);

	return true;
}

stSendIOData* IOContextManager::getSendIO()
{
	if (m_stackSendIO.empty() == true)
	{
		return nullptr;
	}

	stSendIOData* pSendIO = nullptr;

	EnterCriticalSection(&m_CriticalSection);
	pSendIO = m_stackSendIO.top();
	LeaveCriticalSection(&m_CriticalSection);
	return pSendIO;
}
	
void IOContextManager::returnSendIO(stSendIOData* pSendIO)
{
	if (pSendIO == nullptr)
	{
		return;
	}

	memset(pSendIO, 0, sizeof(stSendIOData));

	EnterCriticalSection(&m_CriticalSection);
	m_stackSendIO.emplace(pSendIO);
	LeaveCriticalSection(&m_CriticalSection);
}

void IOContextManager::release()
{
	/*while (m_stackSendIO.empty() != true)
	{
		stSendIOData* pSendIO = m_stackSendIO.top();
		SAFE_DELETE(pSendIO);
		m_stackSendIO.pop();
	}*/
	
	std::stack<stSendIOData*> emptyStack;
	std::swap(m_stackSendIO, emptyStack);

	DeleteCriticalSection(&m_CriticalSection);
}
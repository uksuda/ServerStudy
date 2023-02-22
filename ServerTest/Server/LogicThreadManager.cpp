#include "LogicThreadManager.h"
#include "LogicThread.h"
#include "Log.h"

IMPLEMENT_SINGLETON(LogicThreadManager);

LogicThreadManager::LogicThreadManager()
	: m_hLogicCompletionPort(INVALID_HANDLE_VALUE)
{
	m_vecThread.clear();
	m_dwLogicCount = 0;
}

LogicThreadManager::~LogicThreadManager()
{
	release();
}

void LogicThreadManager::releaseThrowLogicCount()
{
	if (m_dwLogicCount == 0)
	{
		return;
	}

	--m_dwLogicCount;
}

bool LogicThreadManager::initThreadManager()
{
	if (m_hLogicCompletionPort == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	m_hLogicCompletionPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);
	if (m_hLogicCompletionPort == NULL)
	{
		return false;
	}

	m_vecThread.reserve(LOGIC_THREAD_COUNT);
	for (int i = 0; i < LOGIC_THREAD_COUNT; ++i)
	{
		m_vecThread.emplace_back(new LogicThread);
	}

	return true;
}

bool LogicThreadManager::runLogicThread()
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](LogicThread* pThread){
		if (pThread)
		{
			pThread->logicThread(m_hLogicCompletionPort);
		}
	});

	return true;
}

void LogicThreadManager::throwPacketToLogic(ClientSession* pSession, Packet& logicPacket)
{
	if (pSession == nullptr || logicPacket.isValid() == false)
	{
		return;
	}

	if (m_hLogicCompletionPort == INVALID_HANDLE_VALUE)
	{
		return;
	}

	stSessionContext* pSessionContext = new stSessionContext;
	memset(pSessionContext, 0, sizeof(stSessionContext));

	pSessionContext->m_pSendSession = pSession;
	pSessionContext->m_SendPacket = logicPacket;
	PostQueuedCompletionStatus(m_hLogicCompletionPort, 0, (ULONG_PTR)pSessionContext, NULL);

	++m_dwLogicCount;
}

void LogicThreadManager::stopLogicThread()
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](LogicThread* pThread){
		if (pThread)
		{
			pThread->stopRunning();
		}
	});

	postClearLogicThread();
}

void LogicThreadManager::postClearLogicThread()
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](LogicThread* pThread){
		PostQueuedCompletionStatus(m_hLogicCompletionPort, NULL, NULL, NULL);
	});

	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](LogicThread* pThread){
		if (pThread)
		{
			HANDLE threadHandle = pThread->getThreadHandle();
			if (threadHandle != INVALID_HANDLE_VALUE)
			{
				WaitForSingleObject(threadHandle, INFINITE);
			}
		}
	});
}

void LogicThreadManager::release()
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](LogicThread* pThread){
		SAFE_DELETE(pThread);
	});

	m_vecThread.clear();
	CloseHandle(m_hLogicCompletionPort);

	m_dwLogicCount = 0;
}
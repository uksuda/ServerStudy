#include "LogicThread.h"
#include "Log.h"

LogicThread::LogicThread()
	: m_isRunning(false)
	, m_hCompletionPort(INVALID_HANDLE_VALUE)
{

}

LogicThread::~LogicThread()
{
	release();
}

void LogicThread::run()
{
	DWORD dwByteTrans = 0;
	LPOVERLAPPED lpOverLapped = nullptr;
	stSessionContext* pContext = nullptr;

	while (m_isRunning)
	{
		BOOL bResult = GetQueuedCompletionStatus(m_hCompletionPort, &dwByteTrans, (PULONG_PTR)&pContext, (LPOVERLAPPED*)&lpOverLapped, INFINITE);
		if (bResult == false)
		{
			CLog::log("%s -- invalid logic request", __FUNCTION__);
			SAFE_DELETE(pContext);
			continue;
		}

		if (pContext == nullptr)
		{
			continue;
		}

		SAFE_DELETE(pContext);
	}
}

void LogicThread::stopRunning()
{
	if (m_isRunning == true)
	{
		return;
	}

	m_isRunning = false;
}

bool LogicThread::logicThread(HANDLE hCompletionPort)
{
	if (hCompletionPort == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	if (m_isRunning == true)
	{
		return false;
	}

	m_hCompletionPort = hCompletionPort;

	if (ThreadBase::startThread() == false)
	{
		return false;
	}

	m_isRunning = true;
	return true;
}

void LogicThread::release()
{

}
#include "WorkerThread.h"
#include "ClientSession.h"
#include "SessionManager.h"
#include "Log.h"

WorkerThread::WorkerThread()
	: m_isRunning(false)
	, m_hCompletionPort(INVALID_HANDLE_VALUE)
{

}

WorkerThread::~WorkerThread()
{
	release();
}

void WorkerThread::run()
{
	DWORD dwByteTrans = 0;
	LPOVERLAPPED lpOverLapped = nullptr;
	ClientSession* pSession = nullptr;

	while (m_isRunning)
	{
		BOOL bResult = GetQueuedCompletionStatus(m_hCompletionPort, &dwByteTrans, (PULONG_PTR)&pSession, (LPOVERLAPPED*)&lpOverLapped, INFINITE);

		if (pSession == nullptr && dwByteTrans == 0 && lpOverLapped == nullptr)
		{
			// server end
			CLog::log("server end");
			continue;
		}

		if (pSession == nullptr)
		{
			CLog::log("Invalid Session %d", WSAGetLastError());
			continue;
		}

		if (lpOverLapped == nullptr)
		{
			CLog::log("Invalid OverLapped %d", WSAGetLastError());
			continue;
		}

		// if (bResult == FALSE && dwByteTrans == 0)
		if (bResult == FALSE || dwByteTrans == 0)
		{
			// socket close
			unsigned int iUserSeq = pSession->getUserSequence();
			CLog::log("Client disconnected %d", iUserSeq);
			SESSION_MGR->removeClientSession(iUserSeq);
			continue;
		}

		// packet dispatch
		pSession->dispatchPacket(dwByteTrans, lpOverLapped);
		Sleep(10);
	}
}

void WorkerThread::stopRunning()
{
	if (m_isRunning == false)
	{
		return;
	}

	m_isRunning = false;
}

bool WorkerThread::workingThread(HANDLE hCompletionPort)
{
	if (hCompletionPort == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	if (m_isRunning)
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

void WorkerThread::release()
{

}
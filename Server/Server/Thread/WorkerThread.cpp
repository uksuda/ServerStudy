#include "WorkerThread.h"
#include "ClientSession.h"
#include "ClientSessionManager.h"
#include "Synchro.h"
#include "Log.h"

WorkerThread::WorkerThread()
	: m_ThreadHandle(INVALID_HANDLE_VALUE)
	, m_hComport(INVALID_HANDLE_VALUE)
	, m_iThreadID(0)
	, m_isRunning(false)
{

}

WorkerThread::~WorkerThread()
{
	CloseHandle(m_ThreadHandle);
}

void WorkerThread::workBegin(HANDLE hComPort)
{
	if (m_isRunning)
		return;

	m_hComport = hComPort;

	m_ThreadHandle = (HANDLE)_beginthreadex(nullptr, 0, this->entryThread, (LPVOID)this, 0, &m_iThreadID);
	if (m_ThreadHandle == 0)
	{
		// Thread fail
		return;
	}

	m_isRunning = true;
}

void WorkerThread::workRunning()
{
	DWORD bytesTrans;
	//DWORD flags = 0;

	LPOVERLAPPED lpOverlapped = nullptr;
	ClientSession* pClientSession = nullptr;

	while (m_isRunning)
	{
		BOOL bResult = GetQueuedCompletionStatus(m_hComport, &bytesTrans, (PULONG_PTR)&pClientSession, (LPOVERLAPPED*)&lpOverlapped, INFINITE);

		if (pClientSession == nullptr)
		{
			CLog::LOG("GetQueuedCompletion Key NULL");
			continue;
		}

		if (lpOverlapped == nullptr)
		{
			CLog::LOG("GetQueuedCompletion lpOverlapped NULL", GetLastError());
			continue;
		}

		ClientSession::stSessionInfo& refSessionInfo = pClientSession->getSessionInfo();

		if (bResult == FALSE && bytesTrans == 0)
		{
			// client socket disconnected
			CLog::LOG("Client disconnected", refSessionInfo.m_userSeq);
			SYNCHRO->enterCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);
			SESSIONMGR->removeSession(refSessionInfo.m_userSeq);
			SYNCHRO->leaveCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);
			continue;
		}

		// packet dispatch
		pClientSession->packetDispatch(bytesTrans);
	}
}

WorkerThread* WorkerThread::create()
{
	WorkerThread* pThread = new WorkerThread;
	if (pThread)
		return pThread;

	return nullptr;
}

unsigned int WINAPI WorkerThread::entryThread(LPVOID pParameter)
{
	WorkerThread* pThread = static_cast<WorkerThread*>(pParameter);

	if (pThread)
	{
		pThread->workRunning();
	}

	return 0;
}
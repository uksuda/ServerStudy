#include "WorkerThread.h"

WorkerThread::WorkerThread()
	: m_ThreadHandle(INVALID_HANDLE_VALUE)
	, m_hComport(INVALID_HANDLE_VALUE)
	, m_dwThreadID(0)
	, m_isRunning(false)
	, m_isLoop(false)
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

	m_ThreadHandle = (HANDLE)_beginthreadex(nullptr, 0, this->entryThread, (LPVOID)this, 0, &m_dwThreadID);
	if (m_ThreadHandle == 0)
	{
		// Thread fail
		return;
	}

	m_isRunning = true;
	m_isLoop = true;
}

void WorkerThread::workRunning()
{
	DWORD bytesTrans;
	DWORD flags = 0;

	while (m_isLoop)
	{
		GetQueuedCompletionStatus(m_hComport, &bytesTrans, nullptr, nullptr, INFINITE);
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
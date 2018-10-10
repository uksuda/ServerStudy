#include "WorkerThread.h"
#include "ServerUtils.h"

WorkerThread::WorkerThread()
	: m_ThreadHandle(INVALID_HANDLE_VALUE)
	, m_hComport(INVALID_HANDLE_VALUE)
	, m_dwThreadID(0)
	, m_isRunning(false)
{

}

WorkerThread::~WorkerThread()
{
	//CloseHandle(m_ThreadHandle);
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
}

void WorkerThread::workRunning()
{
	DWORD bytesTrans;
	DWORD flags = 0;

	while (true)
	{
		GetQueuedCompletionStatus(m_hComport, &bytesTrans, nullptr, nullptr, INFINITE);		
	}
}

WorkerThread* WorkerThread::createThread()
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
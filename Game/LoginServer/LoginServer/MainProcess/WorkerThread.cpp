#include "WorkerThread.h"

WorkerThread::WorkerThread()
	: m_isRunning(false)
	, m_hCompletionPort(INVALID_HANDLE_VALUE)
{

}

WorkerThread::~WorkerThread()
{
	release();
}

bool WorkerThread::startWorkerThread(HANDLE hCompletionPort)
{
	if (m_isRunning == true || hCompletionPort == INVALID_HANDLE_VALUE)
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

void WorkerThread::stopWorkerThread()
{
	if (m_isRunning == false || getThreadHandle() == INVALID_HANDLE_VALUE)
	{
		return;
	}

	m_isRunning = false;
	WaitForSingleObject(getThreadHandle(), INFINITE);
	return;
}

void WorkerThread::run()
{
	while (m_isRunning)
	{

	}
}

void WorkerThread::release()
{

}
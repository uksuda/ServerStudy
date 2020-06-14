#include "WorkerThreadManager.h"
#include "WorkerThread.h"

IMPLEMENT_SINGLETON(WorkerThreadManager);

WorkerThreadManager::~WorkerThreadManager()
{
	release();
}

bool WorkerThreadManager::initializeWorkerThread(int iThreadCount)
{
	return true;
}

bool WorkerThreadManager::startWorkerThread()
{
	return true;
}

void WorkerThreadManager::stopWorkerThread(HANDLE hCompletionPort)
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pWorker) {
		if (pWorker && pWorker->isRunning())
		{
			PostQueuedCompletionStatus(hCompletionPort, NULL, NULL, NULL);
		}
	});
}

void WorkerThreadManager::release()
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pWorker) {
		if (pWorker)
		{
			SAFE_DELETE(pWorker);
		}
	});
	m_vecThread.clear();
}
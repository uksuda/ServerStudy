#include "WorkerThreadManager.h"
#include "WorkerThread.h"
#include "Log.h"

IMPLEMENT_SINGLETON(WorkerThreadManager);

WorkerThreadManager::WorkerThreadManager()
{
	m_vecThread.clear();
}

WorkerThreadManager::~WorkerThreadManager()
{
	release();
}

bool WorkerThreadManager::initThreadManager()
{
	SYSTEM_INFO systemInfo;
	memset(&systemInfo, 0, sizeof(SYSTEM_INFO));
	GetSystemInfo(&systemInfo);

	DWORD dwWorkerCount = systemInfo.dwNumberOfProcessors * 2 + 1;
	m_vecThread.reserve(dwWorkerCount);

	for (DWORD dwCnt = 0; dwCnt < dwWorkerCount; ++dwCnt)
	{
		m_vecThread.emplace_back(new WorkerThread);
	}

	return true;
}

bool WorkerThreadManager::runWorker(HANDLE hCompletionPort)
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pWorker){
		if (pWorker)
		{
			pWorker->workingThread(hCompletionPort);
		}
	});

	return true;
}

void WorkerThreadManager::stopWorker(HANDLE hCompletionPort)
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pWorker){
		if (pWorker)
		{
			pWorker->stopRunning();
		}
	});

	postClearWorker(hCompletionPort);
}

void WorkerThreadManager::postClearWorker(HANDLE hCompletionPort)
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pWorker){
		PostQueuedCompletionStatus(hCompletionPort, NULL, NULL, NULL);
	});

	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pWorker){
		if (pWorker)
		{
			HANDLE threadHandle = pWorker->getThreadHandle();
			if (threadHandle != INVALID_HANDLE_VALUE)
			{
				WaitForSingleObject(threadHandle, INFINITE);
			}
		}
	});
}

void WorkerThreadManager::release()
{
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pWorker){
		SAFE_DELETE(pWorker);
	});
	m_vecThread.clear();
}
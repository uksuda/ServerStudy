#pragma once

#include "BaseDefine.h"

#define THREAD_MGR WorkerThreadManager::getInstance()

class WorkerThread;
class WorkerThreadManager
{
DECLARE_SINGLETON(WorkerThreadManager);

public:
	~WorkerThreadManager();

	using VEC_THREAD = std::vector<WorkerThread*>;

public:
	bool initializeWorkerThread(int iThreadCount);
	bool startWorkerThread();
	void stopWorkerThread(HANDLE hCompletionPort);

private:
	VEC_THREAD	m_vecThread;

public:
	void release();
};
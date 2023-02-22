#pragma once

#include "BaseDefine.h"

#define THREAD_MGR WorkerThreadManager::getInstance()

class WorkerThread;
class WorkerThreadManager
{
	DECLARE_SINGLETON(WorkerThreadManager);
public:
	~WorkerThreadManager();

	typedef std::vector<WorkerThread*> WORKER_THREAD;

public:
	bool initThreadManager();
	bool runWorker(HANDLE hCompletionPort);
	void stopWorker(HANDLE hCompletionPort);
	void postClearWorker(HANDLE hCompletionPort);

private:
	WORKER_THREAD m_vecThread;
	
public:
	void release();
};
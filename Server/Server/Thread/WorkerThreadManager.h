#ifndef _WORKER_THREAD_MANAGER_H__
#define _WORKER_THREAD_MANAGER_H__

#include "ServerHeader.h"

#include <vector>

#define THREADMGR WorkerThreadManager::getInstance()

class WorkerThread;
class WorkerThreadManager
{
DECLARE_SINGLETON(WorkerThreadManager);

public:
	~WorkerThreadManager();

	typedef std::vector<WorkerThread*> WORKER_THREAD;

public:
	bool isRunning();
	void setOff();
	void workBegin(HANDLE hComPort);
	bool initWorkThreadManager(int iThreadCount = 1);

private:
	WORKER_THREAD m_vecThread;

public:
	void release();
};

#endif
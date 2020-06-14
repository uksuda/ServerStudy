#pragma once

#include "BaseDefine.h"
#include "ThreadBase.h"

class WorkerThread : public ThreadBase
{
public:
	WorkerThread();
	virtual ~WorkerThread();

public:
	bool isRunning() const
	{
		return m_isRunning;
	}

	bool startWorkerThread(HANDLE hCompletionPort);
	void stopWorkerThread();

	virtual void run() override;

private:
	bool m_isRunning;
	HANDLE m_hCompletionPort;

public:
	void release();
};
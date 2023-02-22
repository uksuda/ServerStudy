#pragma once

#include "ThreadBase.h"

class WorkerThread : public ThreadBase
{
public:
	WorkerThread();
	virtual ~WorkerThread();

public:
	bool isThreadRunning() const
	{
		return m_isRunning;
	}

	virtual void run();

	void stopRunning();
	bool workingThread(HANDLE hCompletionPort);

private:
	bool m_isRunning;
	HANDLE m_hCompletionPort;

public:
	void release();
};
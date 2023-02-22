#pragma once

#include "ThreadBase.h"

class LogicThread : public ThreadBase
{
public:
	LogicThread();
	virtual ~LogicThread();

public:
	bool isThreadRunning() const
	{
		return m_isRunning;
	}

	virtual void run();

	void stopRunning();
	bool logicThread(HANDLE hCompletionPort);

private:
	bool m_isRunning;
	HANDLE m_hCompletionPort;

public:
	void release();
};
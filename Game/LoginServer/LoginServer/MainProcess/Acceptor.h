#pragma once

#include "ThreadBase.h"

class Acceptor : public ThreadBase
{
public:
	Acceptor();
	virtual ~Acceptor();

public:
	bool isRunning() const
	{
		return m_isRunning;
	}

	bool startAcceptorThread(HANDLE hCompletionPort);
	void stopAcceptorThread();

	virtual void run() override;

private:
	bool m_isRunning;
	HANDLE m_hCompletionPort;

public:
	void release();
};
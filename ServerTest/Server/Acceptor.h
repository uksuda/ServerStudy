#pragma once

#include "ThreadBase.h"

class ServerSocket;
class Acceptor : public ThreadBase
{
public:
	Acceptor();
	virtual ~Acceptor();

public:
	bool isThreadRunning() const
	{
		return m_isRunning;
	}

	virtual void run();

	void stopRunning();
	bool workingThread(HANDLE hCompletionPort, ServerSocket* pServerSocket);

private:
	bool m_isRunning;
	HANDLE m_hCompletionPort;

	ServerSocket* m_pServerSocket;

public:
	void release();
};
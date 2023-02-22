#pragma once

#include "BaseDefine.h"

class ServerSocket
{
public:
	ServerSocket();
	~ServerSocket();

public:
	HANDLE getCompletionPort() const
	{
		return m_hCompletionPort;
	}

public:
	bool initServerSocket();
	bool bindSocket(unsigned short sPort);
	bool listenSocket();
	void closeSocket();
	SOCKET acceptSocket(SOCKADDR_IN* pClientAddr = nullptr);

private:
	SOCKET m_ServerSocket;
	SOCKADDR_IN m_ServerAddr;
	HANDLE m_hCompletionPort;

public:
	void release();
};
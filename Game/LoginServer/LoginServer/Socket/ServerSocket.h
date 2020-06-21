#pragma once

#include "BaseDefine.h"

class ServerSocket
{
public:
	ServerSocket();
	~ServerSocket();

public:
	bool initializeServerSocket();
	bool bindServerSocket(int iServerPort);
	bool listenServerSocket(int iBackLogCount = SOMAXCONN);

	SOCKET acceptClient(SOCKADDR_IN& clientAddr);

	bool startupSocket();
	void cleanupSocket();

private:
	SOCKADDR_IN m_SockAddr;
	SOCKET m_ServerSocket;

public:
	void release();
};
#ifndef __CLIENT_SESSION_H__
#define __CLIENT_SESSION_H__

#include <WinSock2.h>
#include <Windows.h>

#include "ServerUtils.h"

class ClientSession
{
public:
	enum class IO_MODE
	{
		MODE_READ = 0,
		MODE_WRITE
	};

private:
	explicit ClientSession();
public:
	~ClientSession();

private:
	SOCKET m_ClientSocket;
	SOCKADDR_IN m_ClientAddr;
	OVERLAPPED m_Overlapped;
	IO_MODE eMode;
	WSABUF m_Wsabuf;
	char m_Buffer[BUFFER_SIZE];

private:
	bool initClientSession();

public:
	static ClientSession* createSession();
};

#endif
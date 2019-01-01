#ifndef __SERVER_SOCKET_H__
#define __SERVER_SOCKET_H__

#define BACK_LOG_COUNT 5

#include "ServerHeader.h"

class ServerSocket
{
private:
	explicit ServerSocket();
public:
	~ServerSocket();

public:
	void cleanUpSocket();
	SOCKET startAcception(SOCKADDR_IN& clientAddr);

private:
	SOCKET m_ServerSocket;

private:
	bool startUpSocket();
	bool initSocket(const char* szServerIP, int iServerPort, int iBackLogCount = BACK_LOG_COUNT);

public:
	static ServerSocket* createSocket(const char* szServerIP, int iServerPort);

};

#endif
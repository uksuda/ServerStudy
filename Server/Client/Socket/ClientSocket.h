#ifndef __CLIENT_SOCKET_H__
#define __CLIENT_SOCKET_H__

#ifdef WIN32

#include <WinSock2.h>
#include <Windows.h>
#include <Ws2tcpip.h>
#pragma comment(lib, "ws2_32.lib")

#else

// TODO 모바일 환경 작업 필요
#include <sys/socket.h>
#include <arpa/inet.h>

#define INVALID_SOCKET	-1
#define SOCKET_ERROR	-1

#define SD_BOTH         0x02

typedef int SOCKET;
//using SOCKET = int;

#endif


class ClientSocket
{
private:
	explicit ClientSocket();
public:
	~ClientSocket();

public:
	bool connectTo(const char* szServerIP, int iServerPort);
	void closeSocket();

	// send receive
	void clientStart();

private:
	SOCKET m_Socket;

private:
	bool initSocket();

public:
	static ClientSocket* createSocket();
};

#endif

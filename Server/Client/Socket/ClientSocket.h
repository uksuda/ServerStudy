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

#endif


class ClientSocket
{
public:
	enum class SOCKET_TAG
	{
		NAT_TAG_INGAME = 0,
		NAT_TAG_VERSION,
		NAT_TAG_END
	};

private:
	explicit ClientSocket();
public:
	~ClientSocket();

public:
	bool connectTo(const char* szServerIP, int iServerPort, SOCKET_TAG eSocketTag);
	void closeSocket();

	// send receive

private:
	SOCKET m_Socket;
	SOCKET_TAG m_SocketTag;

private:
	bool initSocket();

public:
	static ClientSocket* createSocket();
};

#endif

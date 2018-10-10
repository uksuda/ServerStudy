#include <WinSock2.h>
#include <Windows.h>
#pragma comment(lib, "ws2_32.lib")

#include "ServerSocket.h"
#include "ServerIP.h"
#include "ServerUtils.h"

ServerSocket::ServerSocket()
{

}
ServerSocket::~ServerSocket()
{

}

void ServerSocket::closeSocket(bool bTimeWait)
{
	if (bTimeWait == false)
	{
		LINGER      lingerStruct;
		lingerStruct.l_onoff = SOCKET_OPTION_TRUE;
		lingerStruct.l_linger = 0;

		setsockopt(m_ServerSocket, SOL_SOCKET, SO_LINGER, (char *)&lingerStruct, sizeof(lingerStruct));
	}

	shutdown(m_ServerSocket, SD_BOTH);
	closesocket(m_ServerSocket);
}
void ServerSocket::cleanUpSocket()
{
	WSACleanup();
}

SOCKET ServerSocket::startAcception()
{
	SOCKET hClientSock;
	SOCKADDR_IN clientAddr;

	int addrLen = sizeof(clientAddr);
	hClientSock = accept(m_ServerSocket, (SOCKADDR*)&clientAddr, &addrLen);
	return hClientSock;
}

bool ServerSocket::startUpSocket()
{
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
		return false;

	return true;
}

bool ServerSocket::initSocket(const char* szServerIP, int iServerPort, int iBackLogCount)
{
	if (startUpSocket() == false)
		return false;

	SOCKADDR_IN serverAddr;
	memset(&serverAddr, 0, sizeof(serverAddr));

	serverAddr.sin_family = AF_INET;
	serverAddr.sin_addr.s_addr = htonl(INADDR_ANY); // InetPton(serverAddr.sin_family, (PCWSTR)szServerIP, &serverAddr.sin_addr);
	serverAddr.sin_port = htons(SERVER_PORT);

	m_ServerSocket = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
	if (m_ServerSocket == INVALID_SOCKET)
	{
		cleanUpSocket();
		return false;
	}

	// TODO socket option fail check?
	bool isReuse = SOCKET_OPTION_TRUE;
	//int iBufferSize = BUFFER_SIZE;
	setsockopt(m_ServerSocket, SOL_SOCKET, SO_REUSEADDR, (char*)&isReuse, sizeof(isReuse));	
	//setsockopt(m_ServerSocket, SOL_SOCKET, SO_SNDBUF, (char*)&iBufferSize, sizeof(iBufferSize));
	//setsockopt(m_ServerSocket, SOL_SOCKET, SO_RCVBUF, (char*)&iBufferSize, sizeof(iBufferSize));

	if (bind(m_ServerSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR)
	{
		closeSocket();
		cleanUpSocket();
		return false;
	}

	if (listen(m_ServerSocket, iBackLogCount) == SOCKET_ERROR)
	{
		closeSocket();
		cleanUpSocket();
		return false;
	}
	
	return true;
}

ServerSocket* ServerSocket::createSocket(const char* szServerIP, int iServerPort)
{
	ServerSocket* pServerSocket = new ServerSocket;
	if (pServerSocket && pServerSocket->initSocket(szServerIP, iServerPort))
		return pServerSocket;

	return nullptr;
}
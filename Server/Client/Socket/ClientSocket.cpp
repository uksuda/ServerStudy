#include "ClientSocket.h"
#include "Packet.h"
#include "ServerUtils.h"
#include "Log.h"
#include <iostream>


ClientSocket::ClientSocket(int iSeed)
	: m_Socket(INVALID_SOCKET)
	, m_Seed(iSeed)
{

}

ClientSocket::~ClientSocket()
{
#ifdef WIN32
	WSACleanup();
#else

#endif
}

bool ClientSocket::connectTo(const char* szServerIP, int iServerPort)
{
	SOCKADDR_IN serverAddr;
	m_Socket = socket(PF_INET, SOCK_STREAM, 0);
	if (m_Socket == INVALID_SOCKET)
	{
		CLog::LOG("socket", GetLastError());
		return false;
	}

	bool isAlive = SOCKET_OPTION_TRUE;
	if (setsockopt(m_Socket, SOL_SOCKET, SO_KEEPALIVE, (char*)&isAlive, sizeof(isAlive)))
	{
		CLog::LOG("setsockopt : SO_KEEPALIVE");
		return false;
	}

#ifdef WIN32
	unsigned long dwBlocking = SOCKET_OPTION_TRUE;
	if (ioctlsocket(m_Socket, FIONBIO, &dwBlocking) == SOCKET_ERROR)
	{
		CLog::LOG("ioctlsocket : FIONBIO");
		return false;
	}
#else
	//fcntl(m_Socket, F_SETFL, O_NONBLOCK);
#endif

	memset(&serverAddr, 0, sizeof(serverAddr));
	serverAddr.sin_family = AF_INET;
	//InetPton(serverAddr.sin_family, (PCWSTR)szServerIP, &serverAddr.sin_addr);
	//serverAddr.sin_addr.s_addr = inet_addr(szServerIP);

	int iRet = 1;
#ifdef WIN32
	iRet = InetPtonA(serverAddr.sin_family, szServerIP, &serverAddr.sin_addr);
#else
	iRet = inet_pton(serverAddr.sin_family, szServerIP, &serverAddr.sin_addr);
#endif

	// 1 : success
	// 0 : invalid address family value
	// -1 : invalid address value - EAFNOSUPPORT
	if (iRet != 1)
	{
		CLog::LOG("inet_Addr invalid value : %d", iRet);
		return false;
	}

	serverAddr.sin_port = htons(iServerPort);

	if (connect(m_Socket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR)
	{
#ifdef WIN32
		if (WSAGetLastError() != WSAEWOULDBLOCK)
#else
		if (GetLastError() != EWOULDBLOCK)
#endif
		{
			CLog::LOG("connect", GetLastError());
			closeSocket();
			return false;
		}		
	}

	return true;
}

void ClientSocket::closeSocket()
{
	shutdown(m_Socket, SD_BOTH);
	closesocket(m_Socket);
}

void ClientSocket::clientStart()
{
	char szMessage[BUFFER_SIZE];
	memset(szMessage, 0, sizeof(szMessage));

	while (true)
	{
		printf("input message (exit 0): ");
		fgets(szMessage, sizeof(szMessage), stdin);

		int iRet = send(m_Socket, szMessage, sizeof(szMessage), NULL);
		if (iRet == SOCKET_ERROR)
		{
			if (GetLastError() == EWOULDBLOCK)
			{
				CLog::LOG("send buffer is full");
				continue;
			}
		}

		iRet = recv(m_Socket, szMessage, sizeof(szMessage), NULL);
		if (iRet == SOCKET_ERROR)
		{
			if (GetLastError() == EWOULDBLOCK)
			{
				continue;
			}
		}

		szMessage[BUFFER_SIZE - 1] = NULL;
		printf("received from server : %s\n\n", szMessage);
	}
}

bool ClientSocket::initSocket()
{
#ifdef WIN32
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		CLog::LOG("WSAStartup", WSAGetLastError());
		return false;
	}
#else
	return true;
#endif
	return true;
}

ClientSocket* ClientSocket::createSocket(int iSeed)
{
	ClientSocket* pSocket = new ClientSocket(iSeed);
	if (pSocket && pSocket->initSocket())
		return pSocket;

	return nullptr;
}
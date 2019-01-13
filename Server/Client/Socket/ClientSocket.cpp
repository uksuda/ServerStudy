#include "ClientSocket.h"
#include "ServerUtils.h"
#include "Log.h"
#include <iostream>


ClientSocket::ClientSocket()
	: m_Socket(INVALID_SOCKET)
{

}

ClientSocket::~ClientSocket()
{
	WSACleanup();
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

	unsigned long dwBlocking = SOCKET_OPTION_FALSE;
	if (ioctlsocket(m_Socket, FIONBIO, &dwBlocking) == SOCKET_ERROR)
	{
		CLog::LOG("ioctlsocket : FIONBIO");
		return false;
	}

	memset(&serverAddr, 0, sizeof(serverAddr));
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_addr.s_addr = InetPton(serverAddr.sin_family, (PCWSTR)szServerIP, &serverAddr.sin_addr);//inet_addr(szServerIP);
	serverAddr.sin_port = htons(iServerPort);

	if (connect(m_Socket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR)
	{
		CLog::LOG("connect", GetLastError());
		closeSocket();
		return false;
	}

	//fcntl(m_Socket, F_SETFL, O_NONBLOCK);
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

		if (!strcmp(szMessage, "0"))
		{
			CLog::LOG("Client End...");
			break;
		}

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
		printf("received from server : %s\n\n\n", szMessage);
	}
}


bool ClientSocket::initSocket()
{
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		CLog::LOG("WSAStartup", WSAGetLastError());
		return false;
	}

	return true;
}

ClientSocket* ClientSocket::createSocket()
{
	ClientSocket* pSocket = new ClientSocket;
	if (pSocket && pSocket->initSocket())
		return pSocket;

	return nullptr;
}

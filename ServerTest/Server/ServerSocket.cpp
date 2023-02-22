#include "ServerSocket.h"
#include "Log.h"

#define MAX_SOCKET_LISTEN 5

ServerSocket::ServerSocket()
	: m_ServerSocket(INVALID_SOCKET)
	, m_hCompletionPort(INVALID_HANDLE_VALUE)
{
	memset(&m_ServerAddr, 0, sizeof(SOCKADDR_IN));
}

ServerSocket::~ServerSocket()
{
	release();
}

bool ServerSocket::initServerSocket()
{
	WSADATA wsaData;
	memset(&wsaData, 0, sizeof(WSADATA));

	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		CLog::log("WSAStartup : %d", WSAGetLastError());
		return false;
	}

	m_ServerSocket = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, nullptr, 0, WSA_FLAG_OVERLAPPED);
	if (m_ServerSocket == INVALID_SOCKET)
	{
		CLog::log("WSASocket : %d", WSAGetLastError());
		return false;
	}

	m_hCompletionPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);
	if (m_hCompletionPort == NULL)
	{
		CLog::log("CreateIoCompletionPort : %d", WSAGetLastError());
		return false;
	}
	
	return true;
}

bool ServerSocket::bindSocket(unsigned short sPort)
{
	m_ServerAddr.sin_family = AF_INET;
	m_ServerAddr.sin_port = htons(sPort);
	m_ServerAddr.sin_addr.s_addr = htonl(INADDR_ANY);

	if (bind(m_ServerSocket, (SOCKADDR*)&m_ServerAddr, sizeof(m_ServerAddr)) == SOCKET_ERROR)
	{
		CLog::log("bind : %d", WSAGetLastError());
		return false;
	}

	return true;
}

bool ServerSocket::listenSocket()
{
	if (listen(m_ServerSocket, MAX_SOCKET_LISTEN) == SOCKET_ERROR)
	{
		CLog::log("listen : %d", WSAGetLastError());
		return false;
	}

	return true;
}

void ServerSocket::closeSocket()
{
	closesocket(m_ServerSocket);
}

SOCKET ServerSocket::acceptSocket(SOCKADDR_IN* pClientAddr)
{
	SOCKADDR_IN tempClientAddr;
	memset(&tempClientAddr, 0, sizeof(SOCKADDR_IN));

	int iAddrLength = sizeof(tempClientAddr);
	SOCKET hClientSocket = WSAAccept(m_ServerSocket, (SOCKADDR*)&tempClientAddr, &iAddrLength, nullptr, 0);
	
	if (pClientAddr != nullptr)
	{
		memcpy(pClientAddr, &tempClientAddr, sizeof(SOCKADDR_IN));
	}

	return hClientSocket;
}

void ServerSocket::release()
{
	closesocket(m_ServerSocket);
	CloseHandle(m_hCompletionPort);
	WSACleanup();
}
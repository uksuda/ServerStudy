#include "ServerSocket.h"

ServerSocket::ServerSocket()
	: m_ServerSocket(INVALID_SOCKET)
{
	memset(&m_SockAddr, 0, sizeof(m_SockAddr));
}

ServerSocket::~ServerSocket()
{
	release();
}

bool ServerSocket::initializeServerSocket()
{
	if (startupSocket() == false)
	{
		return false;
	}

	m_ServerSocket = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, nullptr, 0, WSA_FLAG_OVERLAPPED);
	if (m_ServerSocket == INVALID_SOCKET)
	{
		return false;
	}

	return true;
}

bool ServerSocket::bindServerSocket(int iServerPort)
{
	m_SockAddr.sin_family = AF_INET;
	m_SockAddr.sin_addr.s_addr = htonl(INADDR_ANY);
	m_SockAddr.sin_port = htons(iServerPort);

	if (bind(m_ServerSocket, (SOCKADDR*)&m_SockAddr, sizeof(m_SockAddr)) == SOCKET_ERROR)
	{
		return false;
	}

	return true;
}

bool ServerSocket::listenServerSocket(int iBackLogCount)
{
	if (listen(m_ServerSocket, iBackLogCount) == SOCKET_ERROR)
	{
		return false;
	}

	return true;
}

SOCKET ServerSocket::acceptClient(SOCKADDR_IN& clientAddr)
{
	int iLength = sizeof(clientAddr);
	return accept(m_ServerSocket, (SOCKADDR*)&clientAddr, &iLength);
}

bool ServerSocket::startupSocket()
{
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		return false;
	}

	return true;
}

void ServerSocket::cleanupSocket()
{
	WSACleanup();
}

void ServerSocket::release()
{
	closesocket(m_ServerSocket);
	cleanupSocket();
}
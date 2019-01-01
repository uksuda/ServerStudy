#include "ServerSocket.h"

ServerSocket::ServerSocket()
{

}
ServerSocket::~ServerSocket()
{
	closesocket(m_ServerSocket);
	cleanUpSocket();
}

void ServerSocket::cleanUpSocket()
{
	WSACleanup();
}

SOCKET ServerSocket::startAcception(SOCKADDR_IN& clientAddr)
{
	int addrLen = sizeof(clientAddr);
	return accept(m_ServerSocket, (SOCKADDR*)&clientAddr, &addrLen);
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
		return false;
	}

	// TODO socket option fail check?
	bool isReuse = SOCKET_OPTION_TRUE;
	setsockopt(m_ServerSocket, SOL_SOCKET, SO_REUSEADDR, (char*)&isReuse, sizeof(isReuse));	

	if (bind(m_ServerSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR)
	{
		return false;
	}

	if (listen(m_ServerSocket, iBackLogCount) == SOCKET_ERROR)
	{
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
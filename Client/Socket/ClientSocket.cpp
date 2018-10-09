#include "ClientSocket.h"
#include "ServerUtils.h"


ClientSocket::ClientSocket()
	: m_Socket(INVALID_SOCKET)
	, m_SocketTag(SOCKET_TAG::NAT_TAG_END)
{

}

ClientSocket::~ClientSocket()
{
	
}

bool ClientSocket::connectTo(const char* szServerIP, int iServerPort, SOCKET_TAG eSocketTag)
{
	m_SocketTag = eSocketTag;

	SOCKADDR_IN serverAddr;
	m_Socket = socket(PF_INET, SOCK_STREAM, 0);
	if (m_Socket == INVALID_SOCKET)
	{
		closeSocket();
		return false;
	}

	bool isAlive = SOCKET_OPTION_TRUE;
	if (setsockopt(m_Socket, SOL_SOCKET, SO_KEEPALIVE, (char*)&isAlive, sizeof(isAlive)))
	{
		closeSocket();
		return false;
	}

	unsigned long dwBlocking = SOCKET_OPTION_FALSE;
	if (ioctlsocket(m_Socket, FIONBIO, &dwBlocking) == SOCKET_ERROR)
	{
		closeSocket();
		return false;
	}

	memset(&serverAddr, 0, sizeof(serverAddr));
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_addr.s_addr = InetPton(serverAddr.sin_family, (PCWSTR)szServerIP, &serverAddr.sin_addr);//inet_addr(szServerIP);
	serverAddr.sin_port = htons(iServerPort);

	if (connect(m_Socket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR)
	{
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
	WSACleanup();
}

bool ClientSocket::initSocket()
{
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
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

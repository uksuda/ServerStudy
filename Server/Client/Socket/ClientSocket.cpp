#include "ClientSocket.h"
#include "Packet.h"
#include "ServerUtils.h"
#include "Log.h"
#include "DispatcherClient.h"
#include <iostream>


ClientSocket::ClientSocket(int iSeed)
	: m_Socket(INVALID_SOCKET)
	, m_Seed(iSeed)
	, m_iSendBufferPosition(0)
	, m_iReceiveBufferPosition(0)
{
	memset(m_SendBuffer, 0, sizeof(m_SendBuffer));
	memset(m_ReceiveBuffer, 0, sizeof(m_ReceiveBuffer));
}

ClientSocket::~ClientSocket()
{
#ifdef WIN32
	WSACleanup();
#else

#endif
	memset(m_SendBuffer, 0, sizeof(m_SendBuffer));
	memset(m_ReceiveBuffer, 0, sizeof(m_ReceiveBuffer));
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

	int iResult = connect(m_Socket, (SOCKADDR*)& serverAddr, sizeof(serverAddr));
	if (iResult == SOCKET_ERROR)
	{
		CLog::LOG("connect", WSAGetLastError());
		return false;
	}

	CLog::LOG("connected.");

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

	return true;
}

void ClientSocket::closeSocket()
{
	shutdown(m_Socket, SD_BOTH);
	closesocket(m_Socket);
}

void ClientSocket::resetSendBuffer()
{
	memset(m_SendBuffer, 0, sizeof(m_SendBuffer));
	m_iSendBufferPosition = 0;
}

void ClientSocket::resetReceiveBuffer()
{
	memset(m_ReceiveBuffer, 0, sizeof(m_ReceiveBuffer));
	m_iReceiveBufferPosition = 0;
}

bool ClientSocket::sendFlush()
{
	if (m_SendBuffer == nullptr || m_iSendBufferPosition == 0)
	{
		CLog::LOG("Send Buffer is null");
		return false;
	}

	int iRet = send(m_Socket, m_SendBuffer, m_iSendBufferPosition, NULL);
	if (iRet == SOCKET_ERROR)
	{
#ifdef WIN32
		if (WSAGetLastError() == WSAEWOULDBLOCK)
#else
		if (GetLastError() == EWOULDBLOCK)
#endif 
		{
			CLog::LOG("send buffer is busy");
			return false;
		}

		CLog::LOG("Socket send", GetLastError());
		return false;
	}

	if (m_iSendBufferPosition - iRet <= 0)
	{
		resetSendBuffer();
		return true;
	}

	m_iSendBufferPosition -= iRet;
	memmove(m_SendBuffer, m_SendBuffer + iRet, m_iSendBufferPosition);

	return true;
}

bool ClientSocket::sendPacket(Packet& packet)
{
	packet.setPacketHeaderData();
	if (packet.isPacket() == false)
	{
		return false;
	}
	
	if (m_iSendBufferPosition + packet.getPacketSize() > C_BUFFER_SIZE)
	{
		if (sendFlush() == false)
		{
			return false;
		}
	}
	
	memcpy(m_SendBuffer + m_iSendBufferPosition, packet.getPacketBuffer(), packet.getPacketSize());
	m_iSendBufferPosition += packet.getPacketSize();

	sendFlush();

	return true;
}

bool ClientSocket::receivePacket()
{
	int iRet = recv(m_Socket, m_ReceiveBuffer + m_iReceiveBufferPosition, PACKET_BUFFER_SIZE - m_iReceiveBufferPosition, NULL);
	if (iRet == SOCKET_ERROR)
	{
#ifdef WIN32
		if (WSAGetLastError() == WSAEWOULDBLOCK)
#else
		if (GetLastError() == EWOULDBLOCK)
#endif 
		{
			CLog::LOG("recv buffer is empty");
			return false;
		}

		CLog::LOG("Socket recv", GetLastError());
		return false;
	}

	m_iReceiveBufferPosition += iRet;
	
	if (m_iReceiveBufferPosition < PACKET_HEADER_SIZE)
	{
		return false;
	}

	Packet receivePacket(PACKET_ENUM(E_PID_STC::ID_INVALID));
	memcpy(receivePacket.getPacketBuffer(), m_ReceiveBuffer, m_iReceiveBufferPosition);

	receivePacket.setReceivePacketHeaderData();
	if (m_iReceiveBufferPosition < receivePacket.getPacketSize())
	{
		return false;
	}

	unsigned int iReceiveSize = receivePacket.getPacketSize();
	memcpy(receivePacket.getPacketReceiveBuffer(), m_ReceiveBuffer + PACKET_HEADER_SIZE, receivePacket.getPacketReceiveSize());
	
	if (m_iReceiveBufferPosition == iReceiveSize)
	{
		resetReceiveBuffer();
	}

	m_iReceiveBufferPosition -= iReceiveSize;
	memmove(m_ReceiveBuffer, m_ReceiveBuffer + iReceiveSize, m_iReceiveBufferPosition);

	// dispatch packet
	DISPATCHER_CLIENT->packetDispatch(receivePacket);

	return true;
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
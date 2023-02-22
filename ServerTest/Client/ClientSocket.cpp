#include "ClientSocket.h"
#include "Log.h"

ClientSocket::ClientSocket()
	: m_hClientSocket(INVALID_SOCKET)
	, m_ulSendTime(0)
	, m_ulRecvTime(0)
{
	memset(m_btSendBuffer, 0, sizeof(m_btSendBuffer));
	memset(m_btRecvBuffer, 0, sizeof(m_btRecvBuffer));
}

ClientSocket::~ClientSocket()
{
	release();
}

bool ClientSocket::initializeClientSocket()
{
	WSADATA wsaData;
	memset(&wsaData, 0, sizeof(WSADATA));
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		WSACleanup();
		CLog::log("%s -- WSAStartup fail - %d", __FUNCTION__, WSAGetLastError());
		return false;
	}

	m_hClientSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (m_hClientSocket == INVALID_SOCKET)
	{
		CLog::log("%s -- socket fail - %d", __FUNCTION__, WSAGetLastError());
		return false;
	}

	return true;
}

bool ClientSocket::connectToServer()
{
	SOCKADDR_IN sockAddr;
	memset(&sockAddr, 0, sizeof(SOCKADDR_IN));

	sockAddr.sin_family = AF_INET;
	//sockAddr.sin_addr.s_addr = inet_addr(SERVER_IP);
	std::string strIP = SERVER_IP;
	std::wstring wstrIP = Utils::convertToWstring(strIP);
	InetPton(AF_INET, wstrIP.c_str(), &(sockAddr.sin_addr.s_addr));
	sockAddr.sin_port = htons(SERVER_PORT);

	if (connect(m_hClientSocket, (SOCKADDR*)&sockAddr, sizeof(sockAddr)) == SOCKET_ERROR)
	{
		CLog::log("%s - %d", __FUNCTION__, WSAGetLastError());
		return false;
	}

	CLog::log("Client connect success");
	return true;
}

bool ClientSocket::sendPacket(Packet& sendPacket)
{
	if (sendPacket.isValid() == false)
	{
		return false;
	}

	if (send(m_hClientSocket, (char*)sendPacket.getPacketBuffer(), sendPacket.getPacketSize(), 0) == SOCKET_ERROR)
	{
		closeConnectionSocket();
		CLog::log("%s -- %d", __FUNCTION__, WSAGetLastError());
		return false;
	}

	BYTE btValue = 0;
	char szTemp[10];
	memset(szTemp, 0, sizeof(szTemp));
	sendPacket.getDataFromPacket(&btValue);
	sendPacket.getDataFromPacket(szTemp, 10);
	CLog::log("send packet %d - %d, %s", sendPacket.getPacketID(), btValue, szTemp);

	return true;
}

bool ClientSocket::recvPacket()
{
	int iRet = recv(m_hClientSocket, (char*)m_btRecvBuffer, sizeof(m_btRecvBuffer), 0);
	if (iRet == SOCKET_ERROR)
	{
		closeConnectionSocket();
		CLog::log("%s -- %d", __FUNCTION__, WSAGetLastError());
		return false;
	}

	if (iRet == 0)
	{
		closeConnectionSocket();
		CLog::log("%s -- recv 0 size packet", __FUNCTION__);
		return false;
	}

	unsigned short wPacketSize = Packet::getRecvPacketSize(m_btRecvBuffer, iRet);
	unsigned short wPacketID = Packet::getRecvPacketID(m_btRecvBuffer, iRet);

	if (wPacketID == INVALID_PACKET_ID || wPacketSize == 0)
	{
		CLog::log("%s -- invalid packet", __FUNCTION__);
		return false;
	}

	Packet recvPacket(wPacketID);
	recvPacket.getPacketDataFromRecvBuffer(m_btRecvBuffer, wPacketSize);
	
	BYTE btValue = 0;
	char szTemp[10];
	memset(szTemp, 0, sizeof(szTemp));

	recvPacket.getDataFromPacket(&btValue);
	recvPacket.getDataFromPacket(szTemp, 10);
	CLog::log("recv packet %d %d - %d, %s", wPacketID, wPacketSize, btValue, szTemp);

	return true;
}

void ClientSocket::closeConnectionSocket()
{
	if (m_hClientSocket == INVALID_SOCKET)
	{
		return;
	}

	CLog::log("%s called", __FUNCTION__);
	shutdown(m_hClientSocket, SD_BOTH);
	closesocket(m_hClientSocket);
	m_hClientSocket = INVALID_SOCKET;
}

void ClientSocket::release()
{
	closeConnectionSocket();
	WSACleanup();

	memset(m_btSendBuffer, 0, sizeof(m_btSendBuffer));
	memset(m_btRecvBuffer, 0, sizeof(m_btRecvBuffer));
}
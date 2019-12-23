#include "ClientSession.h"
#include "Log.h"
#include "ClientSessionManager.h"
#include <stdio.h>

ClientSession::ClientSession()
{
	memset(&m_stSessionInfo, 0, sizeof(stSessionInfo));
}

ClientSession::~ClientSession()
{
	shutdown(m_stSessionInfo.m_ClientSocket, SD_BOTH);
	closesocket(m_stSessionInfo.m_ClientSocket);
}

void ClientSession::packetDispatch(DWORD dwBytesTrans)
{
	if (dwBytesTrans == 0)
		return;

	if (m_stSessionInfo.eMode == IO_MODE::MODE_READ)
	{
		dispatchReceive(dwBytesTrans);
	}
	else if (m_stSessionInfo.eMode == IO_MODE::MODE_WRITE)
	{
		dispatchSend(dwBytesTrans);
	}
	else
	{
		// 
	}
}

void ClientSession::recvSocket()
{
	DWORD dwFlag = 0;
	DWORD dwNumBytes = 0;
	int iRetValue = 0;

	m_stSessionInfo.m_Wsabuf.buf = m_stSessionInfo.m_ReceiveBuffer + m_stSessionInfo.m_iReceivePosition;
	m_stSessionInfo.m_Wsabuf.len = S_BUFFER_SIZE - m_stSessionInfo.m_iReceivePosition;
	m_stSessionInfo.eMode = IO_MODE::MODE_READ;
	iRetValue = WSARecv(m_stSessionInfo.m_ClientSocket, &m_stSessionInfo.m_Wsabuf, 1, &dwNumBytes, &dwFlag, &m_stSessionInfo.m_Overlapped, NULL);
	if (iRetValue == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		CLog::LOG("WSARecv", WSAGetLastError());
	}
}

void ClientSession::sendPacket(Packet& sendPacket)
{
	if (sendPacket.isValid() == false)
	{
		return;
	}

	if (sendPacket.getPacketSize() + m_stSessionInfo.m_iSendPosition > sizeof(m_stSessionInfo.m_SendBuffer)/*S_BUFFER_SIZE*/)
	{
		sendFlush();
	}

	memcpy(m_stSessionInfo.m_SendBuffer + m_stSessionInfo.m_iSendPosition, sendPacket.getPacketBuffer(), sendPacket.getPacketSize());
	m_stSessionInfo.m_iSendPosition += sendPacket.getPacketSize();

	sendFlush();
}

void ClientSession::sendFlush()
{
	DWORD dwFlag = 0;
	DWORD dwNumBytes = 0;
	int iRetValue = 0;

	m_stSessionInfo.m_Wsabuf.buf = m_stSessionInfo.m_SendBuffer;
	m_stSessionInfo.m_Wsabuf.len = m_stSessionInfo.m_iSendPosition;
	m_stSessionInfo.eMode = IO_MODE::MODE_WRITE;

	iRetValue = WSASend(m_stSessionInfo.m_ClientSocket, &m_stSessionInfo.m_Wsabuf, 1, &dwNumBytes, dwFlag, &m_stSessionInfo.m_Overlapped, nullptr);
	if (iRetValue == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		CLog::LOG("WSASend", WSAGetLastError());
	}
}

bool ClientSession::initClientSession()
{
	return true;
}

void ClientSession::dispatchReceive(DWORD dwBytesTrans)
{
	m_stSessionInfo.m_iReceivePosition += dwBytesTrans;

	if (m_stSessionInfo.m_iReceivePosition < PACKET_HEADER_SIZE)
	{
		recvSocket();
		return;
	}

	unsigned int iPacketSize = Packet::getReceivedPacketSize(m_stSessionInfo.m_ReceiveBuffer, m_stSessionInfo.m_iReceivePosition);
	unsigned int iPacketID = Packet::getReceivedPacketID(m_stSessionInfo.m_ReceiveBuffer, m_stSessionInfo.m_iReceivePosition);

	if (m_stSessionInfo.m_iReceivePosition < iPacketSize)
	{
		recvSocket();
		return;
	}

	Packet receivePacket(iPacketID);
	memcpy(receivePacket.getPacketBuffer(), m_stSessionInfo.m_ReceiveBuffer, m_stSessionInfo.m_iReceivePosition);

	
	m_stSessionInfo.m_iReceivePosition -= iPacketSize;
	memmove(m_stSessionInfo.m_ReceiveBuffer, m_stSessionInfo.m_ReceiveBuffer + iPacketSize, m_stSessionInfo.m_iReceivePosition);

	// dispatch packet
}

void ClientSession::dispatchSend(DWORD dwBytesTrans)
{
	DWORD dwFlag = 0;
	DWORD dwNumBytes = 0;

	int iRetValue = 0;

	if (dwBytesTrans >= m_stSessionInfo.m_iSendPosition)
	{
		memset(m_stSessionInfo.m_SendBuffer, 0, S_BUFFER_SIZE);
		m_stSessionInfo.m_iSendPosition = 0;

		recvSocket();
		return;
	}

	m_stSessionInfo.m_iSendPosition -= dwBytesTrans;
	memmove(m_stSessionInfo.m_SendBuffer, m_stSessionInfo.m_SendBuffer + dwBytesTrans, m_stSessionInfo.m_iSendPosition);

	sendFlush();
}

ClientSession* ClientSession::createSession()
{
	ClientSession* pSession = new ClientSession;
	if (pSession && pSession->initClientSession())
	{
		return pSession;
	}

	return nullptr;
}
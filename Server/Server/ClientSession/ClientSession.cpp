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

bool ClientSession::initClientSession()
{
	return true;
}

void ClientSession::dispatchReceive(DWORD dwBytesTrans)
{
	DWORD dwFlag = 0;
	DWORD dwNumBytes = 0;
	int iRetValue = 0;

	m_stSessionInfo.m_iReceivePosition += dwBytesTrans;

	if (m_stSessionInfo.m_iReceivePosition < PACKET_HEADER_SIZE)
	{
		m_stSessionInfo.m_Wsabuf.buf = m_stSessionInfo.m_ReceiveBuffer + m_stSessionInfo.m_iReceivePosition;
		m_stSessionInfo.m_Wsabuf.len = S_BUFFER_SIZE - m_stSessionInfo.m_iReceivePosition;
		m_stSessionInfo.eMode = IO_MODE::MODE_READ;
		iRetValue = WSARecv(m_stSessionInfo.m_ClientSocket, &m_stSessionInfo.m_Wsabuf, 1, &dwNumBytes, &dwFlag, &m_stSessionInfo.m_Overlapped, NULL);
		if (iRetValue == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
		{
			CLog::LOG("WSARecv", WSAGetLastError());
		}
		return;
	}

	Packet receivePacket(PACKET_ENUM(E_PID_CTS::ID_INVALID));
	memcpy(receivePacket.getPacketBuffer(), m_stSessionInfo.m_ReceiveBuffer, m_stSessionInfo.m_iReceivePosition);

	receivePacket.setReceivePacketHeaderData();
	if (receivePacket.getPacketSize() > m_stSessionInfo.m_iReceivePosition)
	{
		m_stSessionInfo.m_Wsabuf.buf = m_stSessionInfo.m_ReceiveBuffer + m_stSessionInfo.m_iReceivePosition;
		m_stSessionInfo.m_Wsabuf.len = S_BUFFER_SIZE - m_stSessionInfo.m_iReceivePosition;
		m_stSessionInfo.eMode = IO_MODE::MODE_READ;
		iRetValue = WSARecv(m_stSessionInfo.m_ClientSocket, &m_stSessionInfo.m_Wsabuf, 1, &dwNumBytes, &dwFlag, &m_stSessionInfo.m_Overlapped, NULL);
		if (iRetValue == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
		{
			CLog::LOG("WSARecv", WSAGetLastError());
		}
		return;
	}

	unsigned int iReceiveSize = receivePacket.getPacketSize();
	memcpy(receivePacket.getPacketReceiveBuffer(), m_stSessionInfo.m_ReceiveBuffer + PACKET_HEADER_SIZE, receivePacket.getPacketReceiveSize());

	m_stSessionInfo.m_iReceivePosition -= iReceiveSize;
	memmove(m_stSessionInfo.m_ReceiveBuffer, m_stSessionInfo.m_ReceiveBuffer + iReceiveSize, m_stSessionInfo.m_iReceivePosition);

	// dispatch packet


	/*m_stSessionInfo.m_Wsabuf.buf = m_stSessionInfo.m_ReceiveBuffer + m_stSessionInfo.m_iReceivePosition;
	m_stSessionInfo.m_Wsabuf.len = S_BUFFER_SIZE - m_stSessionInfo.m_iReceivePosition;
	m_stSessionInfo.eMode = IO_MODE::MODE_READ;
	iRetValue = WSARecv(m_stSessionInfo.m_ClientSocket, &m_stSessionInfo.m_Wsabuf, 1, &dwNumBytes, &dwFlag, &m_stSessionInfo.m_Overlapped, NULL);
	if (iRetValue == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		CLog::LOG("WSARecv", WSAGetLastError());
	}*/
}

void ClientSession::dispatchSend(DWORD dwBytesTrans)
{
	/*printf("client %d : %d [send]. msg : %s", m_stSessionInfo.m_userSeq, dwBytesTrans, m_stSessionInfo.m_Buffer);

	memset(m_stSessionInfo.m_Buffer, 0, sizeof(BUFFER_SIZE));

	DWORD dwFlag = 0;
	DWORD dwRecvNumBytes = 0;

	m_stSessionInfo.m_Wsabuf.len = BUFFER_SIZE;
	m_stSessionInfo.m_Wsabuf.buf = m_stSessionInfo.m_Buffer;
	m_stSessionInfo.eMode = ClientSession::IO_MODE::MODE_READ;

	int iRetValue = WSARecv(m_stSessionInfo.m_ClientSocket, &m_stSessionInfo.m_Wsabuf, 1, &dwRecvNumBytes, &dwFlag, &m_stSessionInfo.m_Overlapped, NULL);
	if (iRetValue == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		CLog::LOG("WSARecv", WSAGetLastError());
	}*/
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
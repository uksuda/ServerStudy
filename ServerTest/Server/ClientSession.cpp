#include "ClientSession.h"
#include "SessionManager.h"
#include "IOContextManager.h"
#include "LogicThreadManager.h"
#include "Log.h"

ClientSession::ClientSession(SOCKET clientSocket, SOCKADDR_IN clientAddr)
	: m_iUserSeq(0)
	, m_ClientSocket(clientSocket)
	, m_ClientAddr(clientAddr)
	, m_iRecvPos(0)
	, m_ulRecvTime(0)
	, m_ulSendTime(0)
{
	memset(&m_stRecvIO, 0, sizeof(stIOData));
	memset(&m_RecvBuffer, 0, sizeof(m_RecvBuffer));
}

ClientSession::~ClientSession()
{
	release();
}

bool ClientSession::isConnected() const
{
	return !(m_ClientSocket == INVALID_SOCKET);
}

bool ClientSession::initSession()
{
	

	return true;
}

void ClientSession::recvData()
{
	if (isConnected() == false)
	{
		return;
	}

	if (m_iRecvPos >= sizeof(m_RecvBuffer))
	{
		CLog::log("recv buffer full - User : %d", m_iUserSeq);
		// reset recv buffer
		return;
	}

	DWORD dwFlag = 0;
	DWORD dwNumBytes = 0;

	m_stRecvIO.m_WsaBuf.buf = (char*)(m_RecvBuffer + m_iRecvPos);
	m_stRecvIO.m_WsaBuf.len = sizeof(m_RecvBuffer) - m_iRecvPos;
	m_stRecvIO.m_eMode = E_IO_MODE::E_IO_RECV;

	int iRet = WSARecv(m_ClientSocket, &m_stRecvIO.m_WsaBuf, 1, &dwNumBytes, &dwFlag, &m_stRecvIO.m_overLapped, nullptr);
	if (iRet == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		CLog::log("WSARecv : %d", WSAGetLastError());
		closesocket(m_ClientSocket);
		m_ClientSocket = INVALID_SOCKET;
	}
}

void ClientSession::sendData(stSendIOData* pSendIO)
{
	if (isConnected() == false || pSendIO == nullptr)
	{
		return;
	}

	DWORD dwFlag = 0;
	DWORD dwNumBytes = 0;

	int iRet = WSASend(m_ClientSocket, &pSendIO->m_WsaBuf, 1, &dwNumBytes, dwFlag, &pSendIO->m_overLapped, nullptr);
	if (iRet == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		CLog::log("WSASend : %d", WSAGetLastError());
		closesocket(m_ClientSocket);
		m_ClientSocket = INVALID_SOCKET;
	}	

	/*m_stIOData.m_WsaBuf.buf = m_SendBuffer;
	m_stIOData.m_WsaBuf.len = m_iSendPos;
	m_stIOData.m_eMode = E_IO_MODE::E_IO_WRITE;

	int iRet = WSASend(m_ClientSocket, &m_stIOData.m_WsaBuf, 1, &dwNumBytes, dwFlag, &m_stIOData.m_overLapped, nullptr);
	if (iRet == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		CLog::log("WSASend : %d", WSAGetLastError());
		closesocket(m_ClientSocket);
		m_ClientSocket = INVALID_SOCKET;
	}*/
}

void ClientSession::dispatchPacket(DWORD dwByteTrans, LPOVERLAPPED pOverlapped)
{
	if (dwByteTrans == 0 || pOverlapped == nullptr)
	{
		return;
	}

	stIOData* pIOData = reinterpret_cast<stIOData*>(pOverlapped);
	if (pIOData == nullptr)
	{
		this->closeConnection();
		return;
	}

	if (pIOData->m_eMode == E_IO_MODE::E_IO_RECV)
	{
		unsigned int iRecvBefore = m_iRecvPos;
		// recv
		m_iRecvPos += dwByteTrans;
		m_ulRecvTime = GetTickCount64();

		unsigned int iRecvAfter = m_iRecvPos;

		// test
		printf("recv data from client %d: %s\n", m_iUserSeq, m_RecvBuffer);
		printf("recv pos %d -> %d\n", iRecvBefore, iRecvAfter);

		// packet processing
		unsigned short wPacketSize = Packet::getRecvPacketSize(m_RecvBuffer, m_iRecvPos);
		unsigned short wPacketID = Packet::getRecvPacketID(m_RecvBuffer, m_iRecvPos);

		if (wPacketID == INVALID_PACKET_ID || wPacketSize == 0)
		{
			recvData();
			return;
		}

		Packet recvPacket(wPacketID);
		if (recvPacket.getPacketDataFromRecvBuffer(m_RecvBuffer, wPacketSize) == false)
		{
			recvData();
			return;
		}

		LOGICTHREAD_MGR->throwPacketToLogic(this, recvPacket);
		
		m_iRecvPos = (m_iRecvPos > wPacketSize) ? m_iRecvPos - wPacketSize : 0;
		memmove(m_RecvBuffer, m_RecvBuffer + wPacketSize, m_iRecvPos);

		recvData();
	}
	else if (pIOData->m_eMode == E_IO_MODE::E_IO_SEND)
	{
		stSendIOData* pSendIO = static_cast<stSendIOData*>(pIOData);
		if (pSendIO == nullptr)
		{
			return;
		}

		if (pSendIO->m_wDataLength > dwByteTrans)
		{
			pSendIO->m_wDataLength -= static_cast<WORD>(dwByteTrans);
			memmove(pSendIO->m_btSendBuffer, pSendIO->m_btSendBuffer + dwByteTrans, pSendIO->m_wDataLength);

			sendData(pSendIO);
			return;
		}
		else if (pSendIO->m_wDataLength < dwByteTrans)
		{
			// invalid
			IOCONTEXT_MGR->returnSendIO(pSendIO);
			this->closeConnection();
			return;
		}

		IOCONTEXT_MGR->returnSendIO(pSendIO);
		m_ulSendTime = GetTickCount64();
	}
	else
	{
		
	}
}

void ClientSession::closeConnection()
{
	shutdown(m_ClientSocket, SD_BOTH);
	closesocket(m_ClientSocket);
	m_ClientSocket = INVALID_SOCKET;
}

void ClientSession::release()
{
	closeConnection();
	//SAFE_DELETE(m_pPlayer);
}
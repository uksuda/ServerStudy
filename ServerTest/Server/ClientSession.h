#pragma once

#include "BaseDefine.h"

#define SESSION_BUFFER_SIZE 5 * MAX_BUFFER_SIZE

//class Player;
class ClientSession
{
public:
	ClientSession() = delete;
	ClientSession(SOCKET clientSocket, SOCKADDR_IN clientAddr);
	~ClientSession();

public:
	void setUserSequenct(UINT iSeq)
	{
		m_iUserSeq = iSeq;
	}
	const UINT getUserSequence() const
	{
		return m_iUserSeq;
	}	

	bool isConnected() const;

	bool initSession();
	void recvData();
	void sendData(stSendIOData* pSendIO);

	void dispatchPacket(DWORD dwByteTrans, LPOVERLAPPED pOverlapped);
	void closeConnection();

private:
	UINT m_iUserSeq;

	stIOData m_stRecvIO;
	SOCKET m_ClientSocket;
	SOCKADDR_IN m_ClientAddr;

	ULONGLONG m_ulRecvTime;
	unsigned char m_RecvBuffer[SESSION_BUFFER_SIZE];
	UINT m_iRecvPos;

	ULONGLONG m_ulSendTime;

public:
	void release();
};
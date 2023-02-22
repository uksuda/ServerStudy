#pragma once

#include "BaseDefine.h"

class ClientSocket
{
public:
	ClientSocket();
	~ClientSocket();

public:
	bool initializeClientSocket();
	bool connectToServer();

	bool sendPacket(Packet& sendPacket);
	bool recvPacket();

	void closeConnectionSocket();

private:
	SOCKET m_hClientSocket;

	ULONGLONG m_ulSendTime;
	ULONGLONG m_ulRecvTime;

	unsigned char m_btSendBuffer[MAX_BUFFER_SIZE];
	unsigned char m_btRecvBuffer[MAX_BUFFER_SIZE];

public:
	void release();
};
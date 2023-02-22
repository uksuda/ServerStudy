#pragma once

struct stIOData
{
	OVERLAPPED m_overLapped;
	WSABUF m_WsaBuf;
	E_IO_MODE m_eMode;
};

// io context
// 1. io context Manager
// 2, two io context per session(send / receive)

// 1 recv position hold & logic thread
// 1. 1 send position
// 2. individial send position

struct stSendIOData : public stIOData
{
	WORD m_wDataLength;
	unsigned char m_btSendBuffer[PACKET_BUFFER_SIZE];
};

class ClientSession;
struct stSessionContext
{
	ClientSession* m_pSendSession;
	Packet m_SendPacket;
};

struct stQueryData
{
	E_QUERY_EXEC_TYPE m_eQueryType;
	ClientSession* m_pQuerySession;
	std::string m_strQuery;
};
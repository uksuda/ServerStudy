#ifndef __CLIENT_SESSION_H__
#define __CLIENT_SESSION_H__

#include "ServerHeader.h"

#define S_BUFFER_SIZE PACKET_BUFFER_SIZE * 5

class ClientSession
{
public:
	enum class IO_MODE
	{
		MODE_READ = 0,
		MODE_WRITE
	};

	struct stSessionInfo
	{
		// io info
		OVERLAPPED m_Overlapped;
		WSABUF m_Wsabuf;

		char m_ReceiveBuffer[S_BUFFER_SIZE];
		unsigned int m_iReceivePosition;

		char m_SendBuffer[S_BUFFER_SIZE];
		unsigned int m_iSendPosition;

		IO_MODE eMode;
		
		// socket info
		SOCKET m_ClientSocket;
		SOCKADDR_IN m_ClientAddr;

		// user data
		unsigned int m_userSeq;
	};

private:
	explicit ClientSession();
public:
	~ClientSession();

public:
	stSessionInfo& getSessionInfo() { return m_stSessionInfo; }

public:
	void packetDispatch(DWORD dwBytesTrans);
	void recvSocket();
	void sendPacket(Packet& sendPacket);
	void sendFlush();

private:
	stSessionInfo m_stSessionInfo;

private:
	bool initClientSession();
	void dispatchReceive(DWORD dwBytesTrans);
	void dispatchSend(DWORD dwBytesTrans);
	
public:
	static ClientSession* createSession();
};

#endif
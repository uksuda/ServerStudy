#ifndef __CLIENT_SESSION_H__
#define __CLIENT_SESSION_H__

#include "ServerHeader.h"

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
		char m_Buffer[BUFFER_SIZE];
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
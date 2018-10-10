#ifndef __SERVER_SOCKET_H__
#define __SERVER_SOCKET_H__

#define BACK_LOG_COUNT 5

class ServerSocket
{
private:
	explicit ServerSocket();
public:
	~ServerSocket();

public:
	void closeSocket(bool bTimeWait = false);
	void cleanUpSocket();
	SOCKET startAcception();

private:
	SOCKET m_ServerSocket;

private:
	bool startUpSocket();
	bool initSocket(const char* szServerIP, int iServerPort, int iBackLogCount = BACK_LOG_COUNT);

public:
	static ServerSocket* createSocket(const char* szServerIP, int iServerPort);

};

#endif
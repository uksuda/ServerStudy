#ifndef __MAIN_SERVER_H__
#define __MAIN_SERVER_H__

#include "ServerHeader.h"

class ServerSocket;
class MainServer
{
private:
	explicit MainServer();
public:
	~MainServer();

public:
	void runServer();
	void updateServer(float fDelta);

private:
	bool m_bRunServer;
	HANDLE m_hComPort;
	ServerSocket* m_pServerSocket;	

private:
	bool initMainServer();

public:
	static MainServer* createMainServer();
};

#endif
#pragma once

#include "BaseDefine.h"

#define SERVER_UPDATE_TIME 100 // 1000 == 1s

class ServerSocket;
class Acceptor;
class MainServer
{
public:
	MainServer();
	~MainServer();

public:
	void resetServerTime();
	bool initMainServer();
	void startServer();
	void update(float fDeltaTime);

private:
	ServerSocket* m_pServerSocket;
	Acceptor* m_pAcceptor;

	SYSTEMTIME m_ServerStartTime;
	float m_fServerOperateTime;
public:
	void release();
};
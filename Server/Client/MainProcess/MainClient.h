#ifndef _MAIN_CLIENT_H__
#define _MAIN_CLIENT_H__

#include "ServerHeader.h"

class ClientSocket;
class MainClient
{
private:
	MainClient();

public:
	~MainClient();

public:
	static MainClient* createMainClient();

	void stopClient();
	bool initialize();
	void runClient();

private:
	ClientSocket* m_pSocket;

	bool m_bRunning;
	float m_fAccumulatedTime;

private:
	void update(float fDelta);
	void render();

	void inputMessage();

public:
	void release();
};

#endif
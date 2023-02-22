#pragma once

#include "BaseDefine.h"


#define TEST_TIME	100 // s
class ClientSocket;
class MainClient
{
public:
	MainClient();
	~MainClient();

public:
	bool initializeClient();

	void startTestLoop(unsigned int iTestTime);

private:
	unsigned long long m_ulStartTick;
	unsigned long long m_ulEndTick;

	//ClientSocket* m_pClientSocket;
	std::shared_ptr<ClientSocket> m_pClientSocket;

public:
	void release();
};
#pragma once

#include "ThreadBase.h"

#define SEND_MGR SendManager::getInstance()

class SendManager : public ThreadBase
{
DECLARE_SINGLETON(SendManager);

public:
	~SendManager();

public:
	void insertSendQueue(ClientSession* pSession, Packet& sendPacket);
	void insertSendQueue(stSessionContext* pSendContext);

	bool sendThreadStart();
	void stopRunning();

	virtual void run();

private:
	bool m_isRunning;
	std::queue<stSessionContext*> m_queueContext;
	CRITICAL_SECTION m_CriticalSection;

public:
	void release();
};
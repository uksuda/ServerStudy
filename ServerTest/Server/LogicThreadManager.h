#pragma once

#include "BaseDefine.h"

#define LOGICTHREAD_MGR LogicThreadManager::getInstance()
#define LOGIC_THREAD_COUNT 5

class LogicThread;
class LogicThreadManager
{
	DECLARE_SINGLETON(LogicThreadManager);
public:
	~LogicThreadManager();

	typedef std::vector<LogicThread*> LOGIC_THREAD;

public:
	DWORD getCurrentThrowLogicCount() const
	{
		return m_dwLogicCount;
	}
	void releaseThrowLogicCount();

	bool initThreadManager();
	bool runLogicThread();

	void throwPacketToLogic(ClientSession* pSession, Packet& logicPacket);

	void stopLogicThread();
	void postClearLogicThread();

private:
	LOGIC_THREAD m_vecThread;

	HANDLE m_hLogicCompletionPort;
	std::atomic<DWORD> m_dwLogicCount;

public:
	void release();
};
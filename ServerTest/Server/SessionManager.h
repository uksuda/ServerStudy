#pragma once

#include "BaseDefine.h"

#define SESSION_MGR SessionManager::getInstance()

class ClientSession;
class SessionManager
{
	DECLARE_SINGLETON(SessionManager);
public:
	~SessionManager();

	typedef std::map<UINT, ClientSession*> MAP_SESSION;
	typedef std::map<UINT, ClientSession*>::iterator SESSION_ITER;

public:
	unsigned int getCurrentSequence() const
	{
		return m_iTotalSequence;
	}

	bool initSessionManager();
	bool addNewClientSession(ClientSession* pSession);
	void removeClientSession(unsigned int iUserSeq);

	ClientSession* findClientSession(unsigned int iUserSeq);

private:
	MAP_SESSION m_mapSession;
	unsigned int m_iTotalSequence;
	CRITICAL_SECTION m_CriticalSection;

public:
	void release();
};
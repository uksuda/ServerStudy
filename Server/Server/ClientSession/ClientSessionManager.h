#ifndef _CLIENT_SESSION_MANAGER_H__
#define _CLIENT_SESSION_MANAGER_H__

#include "ServerHeader.h"

#include <map>

#define SESSIONMGR ClientSessionManager::getInstance()

class ClientSession;
class ClientSessionManager
{
DECLARE_SINGLETON(ClientSessionManager);

public:
	~ClientSessionManager();

	typedef std::map<unsigned int, ClientSession*> CLIENT_SESSION;
	typedef std::map<unsigned int, ClientSession*>::iterator SESSION_ITER;

public:
	unsigned int getCurrentSessionCount() { return m_SessionMap.size(); }

public:
	bool initClientSessionManager();
	void insertNewSession(ClientSession* pClientSession);
	void removeSession(unsigned int iKey);

private:
	CLIENT_SESSION m_SessionMap;

public:
	void release();

};

#endif
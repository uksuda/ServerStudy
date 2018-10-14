#ifndef _CLIENT_SESSION_MANAGER_H__
#define _CLIENT_SESSION_MANAGER_H__

#include "ServerHeader.h"

#include <map>

class ClientSession;
class ClientSessionManager
{
DECLARE_SINGLETON(ClientSessionManager);

public:
	~ClientSessionManager();

	typedef std::map<int, ClientSession*> CLIENT_SESSION;
	typedef std::map<int, ClientSession*>::iterator SESSION_ITER;

public:
	bool initClientSessionManager();
	void insertNewSession();
	void removeSession();

private:
	CLIENT_SESSION m_SessionMap;

public:
	void release();

};

#endif
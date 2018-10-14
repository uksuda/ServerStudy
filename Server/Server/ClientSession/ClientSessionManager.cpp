#include "ClientSessionManager.h"
#include "ClientSession.h"

#include <algorithm>

IMPLEMENT_SINGLETON(ClientSessionManager);

ClientSessionManager::~ClientSessionManager()
{
	release();
}

bool ClientSessionManager::initClientSessionManager()
{
	return true;
}

void ClientSessionManager::insertNewSession()
{

}

void ClientSessionManager::removeSession()
{

}

void ClientSessionManager::release()
{
	std::for_each(m_SessionMap.begin(), m_SessionMap.end(), [&](std::pair<int, ClientSession*> pPair) {
		SAFE_DELETE(pPair.second);
	});

	m_SessionMap.clear();
}
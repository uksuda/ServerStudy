#include "ClientSessionManager.h"
#include "ClientSession.h"
#include "Synchro.h"

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

void ClientSessionManager::insertNewSession(ClientSession* pClientSession)
{
	if (pClientSession == nullptr)
		return;

	m_SessionMap.insert(CLIENT_SESSION::value_type(pClientSession->getSessionInfo().m_userSeq, pClientSession));
}

void ClientSessionManager::removeSession(unsigned int iKey)
{
	auto sessionData = m_SessionMap.find(iKey);
	if (sessionData == m_SessionMap.end())
		return;

	SAFE_DELETE(sessionData->second);
	m_SessionMap.erase(iKey);
}

void ClientSessionManager::sendAllUser(Packet& sendPacket)
{
	//SYNCHRO->enterCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);
	std::for_each(m_SessionMap.begin(), m_SessionMap.end(), [&](std::pair<unsigned int, ClientSession*> sessionPair) {
		ClientSession* pSession = sessionPair.second;
		if (pSession != nullptr)
		{
			pSession->sendPacket(sendPacket);
		}
	});
	//SYNCHRO->leaveCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);
}

void ClientSessionManager::release()
{
	std::for_each(m_SessionMap.begin(), m_SessionMap.end(), [&](std::pair<unsigned int, ClientSession*> pPair) {
		SAFE_DELETE(pPair.second);
	});

	m_SessionMap.clear();
}
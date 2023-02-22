#include "SessionManager.h"
#include "ClientSession.h"

IMPLEMENT_SINGLETON(SessionManager);

SessionManager::SessionManager()
	: m_iTotalSequence(0)
{
	m_mapSession.clear();
}

SessionManager::~SessionManager()
{
	release();
}

bool SessionManager::initSessionManager()
{
	InitializeCriticalSection(&m_CriticalSection);
	return true;
}

bool SessionManager::addNewClientSession(ClientSession* pSession)
{
	if (pSession == nullptr)
	{
		return false;
	}

	EnterCriticalSection(&m_CriticalSection);
	++m_iTotalSequence;
	pSession->setUserSequenct(m_iTotalSequence);
	m_mapSession.insert(MAP_SESSION::value_type(m_iTotalSequence, pSession));
	LeaveCriticalSection(&m_CriticalSection);

	return true;
}

void SessionManager::removeClientSession(unsigned int iUserSeq)
{
	EnterCriticalSection(&m_CriticalSection);
	SESSION_ITER iter = m_mapSession.find(iUserSeq);
	if (iter == m_mapSession.end())
	{
		LeaveCriticalSection(&m_CriticalSection);
		return;
	}

	SAFE_DELETE(iter->second);
	m_mapSession.erase(iter);
	--m_iTotalSequence;
	LeaveCriticalSection(&m_CriticalSection);
}

ClientSession* SessionManager::findClientSession(unsigned int iUserSeq)
{
	SESSION_ITER iter = m_mapSession.find(iUserSeq);
	if (iter == m_mapSession.end())
	{
		return nullptr;
	}

	return iter->second;
}

void SessionManager::release()
{
	std::for_each(m_mapSession.begin(), m_mapSession.end(), [&](std::pair<UINT, ClientSession*> pair){
		if (pair.second)
		{
			SAFE_DELETE(pair.second);
		}
	});

	m_mapSession.clear();

	DeleteCriticalSection(&m_CriticalSection);
}
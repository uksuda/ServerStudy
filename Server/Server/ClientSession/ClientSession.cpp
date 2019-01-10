#include "ClientSession.h"

ClientSession::ClientSession()
{
	memset(&m_stSessionInfo, 0, sizeof(stSessionInfo));
}

ClientSession::~ClientSession()
{
	shutdown(m_stSessionInfo.m_ClientSocket, SD_BOTH);
	closesocket(m_stSessionInfo.m_ClientSocket);
}

void ClientSession::packetDispatch(DWORD dwBytesTrans)
{
	if (dwBytesTrans == 0)
		return;


}

bool ClientSession::initClientSession()
{
	return true;
}

ClientSession* ClientSession::createSession()
{
	ClientSession* pSession = new ClientSession;
	if (pSession && pSession->initClientSession())
	{
		return pSession;
	}

	return nullptr;
}
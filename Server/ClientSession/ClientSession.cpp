#include "ClientSession.h"

ClientSession::ClientSession()
{

}

ClientSession::~ClientSession()
{

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
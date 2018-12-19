#include "MainServer.h"
#include "ClientSessionManager.h"
#include "WorkerThreadManager.h"
#include "ServerSocket.h"


MainServer::MainServer()
	: m_pServerSocket(nullptr)
{

}

MainServer::~MainServer()
{
	SESSIONMGR->destroyInstance();
	THREADMGR->destroyInstance();
	m_pServerSocket->closeSocket();
	m_pServerSocket->cleanUpSocket();
}

void MainServer::runServer()
{
	while (true)
	{

	}
}

void MainServer::updateServer(float fDelta)
{

}

bool MainServer::initMainServer()
{
	SESSIONMGR->initClientSessionManager();

	//CreateIoCompletionPort();

	SYSTEM_INFO systemInfo;
	GetSystemInfo(&systemInfo);

	THREADMGR->initWorkThreadManager(systemInfo.dwNumberOfProcessors);

	m_pServerSocket = ServerSocket::createSocket(SERVER_HOST, SERVER_PORT);


	return true;
}

MainServer* MainServer::createMainServer()
{
	MainServer* pServer = new MainServer;
	if (pServer && pServer->initMainServer())
	{
		return pServer;
	}

	return nullptr;
}
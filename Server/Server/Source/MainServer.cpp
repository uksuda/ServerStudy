#include "MainServer.h"
#include "ClientSessionManager.h"
#include "WorkerThreadManager.h"
#include "ServerSocket.h"


MainServer::MainServer()
	: m_bRunServer(false)
	, m_hComPort(INVALID_HANDLE_VALUE)
	, m_pServerSocket(nullptr)	
{
	
}

MainServer::~MainServer()
{
	SESSIONMGR->destroyInstance();
	THREADMGR->destroyInstance();
	SAFE_DELETE(m_pServerSocket);
	CloseHandle(m_hComPort);
}

void MainServer::runServer()
{
	THREADMGR->workBegin(m_hComPort);
	if (THREADMGR->isRunning() == false)
	{
		// thread fail
		return;
	}

	while (m_bRunServer)
	{
		SOCKADDR_IN clientAddr;
		SOCKET hClientSocket = m_pServerSocket->startAcception(clientAddr);
		if (hClientSocket == INVALID_SOCKET)
		{
			// client socket error
			continue;
		}

	}


}

void MainServer::updateServer(float fDelta)
{

}

bool MainServer::initMainServer()
{
	if (SESSIONMGR->initClientSessionManager() == false)
	{
		return false;
	}

	SYSTEM_INFO systemInfo;
	GetSystemInfo(&systemInfo);

	int iThreadCount = systemInfo.dwNumberOfProcessors * 2 + 1;

	m_hComPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);
	if (m_hComPort == NULL)
	{
		//GetLastError();
		return false;
	}

	if (THREADMGR->initWorkThreadManager(iThreadCount) == false)
	{
		return false;
	}

	m_pServerSocket = ServerSocket::createSocket(SERVER_HOST, SERVER_PORT);
	if (m_pServerSocket == nullptr)
	{
		return false;
	}

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
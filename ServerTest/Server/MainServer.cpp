#include "MainServer.h"
#include "ServerSocket.h"
#include "Acceptor.h"
#include "WorkerThreadManager.h"
#include "SessionManager.h"
#include "Log.h"

MainServer::MainServer()
	: m_pServerSocket(nullptr)
	, m_pAcceptor(nullptr)
	, m_fServerOperateTime(0.f)
{
	memset(&m_ServerStartTime, 0, sizeof(SYSTEMTIME));
	SESSION_MGR;
	THREAD_MGR;

	CLog::createLogFile();
}

MainServer::~MainServer()
{
	release();
}

void MainServer::resetServerTime()
{
	GetLocalTime(&m_ServerStartTime);
	m_fServerOperateTime = 0.f;

	CLog::log("Server reset time : %04d-%02d-%02d %02d:%02d:%02d",
		m_ServerStartTime.wYear, m_ServerStartTime.wMonth, m_ServerStartTime.wDay, 
		m_ServerStartTime.wHour, m_ServerStartTime.wMinute, m_ServerStartTime.wSecond);
}

bool MainServer::initMainServer()
{
	m_pAcceptor = new Acceptor;
	m_pServerSocket = new ServerSocket;
	if (m_pServerSocket->initServerSocket() == false)
	{
		CLog::log("%s -- socket init failed", __FUNCTION__);
		return false;
	}

	if (m_pServerSocket->bindSocket(SERVER_PORT) == false)
	{
		CLog::log("server socket bind fail");
		return false;
	}

	if (m_pServerSocket->listenSocket() == false)
	{
		CLog::log("server socket listen fail");
		return false;
	}

	if (THREAD_MGR->initThreadManager() == false)
	{
		CLog::log("workthread init fail");
		return false;
	}

	if (SESSION_MGR->initSessionManager() == false)
	{
		CLog::log("session init fail");
		return false;
	}

	return true;
}

void MainServer::startServer()
{
	HANDLE hCompletionPort = m_pServerSocket->getCompletionPort();
	if (hCompletionPort == NULL || hCompletionPort == INVALID_HANDLE_VALUE)
	{
		CLog::log("fail start server - invalid CompletionPort");
		return;
	}

	if (m_pAcceptor->workingThread(hCompletionPort, m_pServerSocket) == false)
	{
		CLog::log("fail start server - acceptor fail");
		return;
	}

	if (THREAD_MGR->runWorker(hCompletionPort) == false)
	{
		CLog::log("fail start server - working thread fail");
		return;
	}

	resetServerTime();
	CLog::log("Server Started");
	ULONGLONG ulCheckTime = GetTickCount64();
	while (true)
	{
		ULONGLONG ulDelta = GetTickCount64() - ulCheckTime;
		if (ulDelta > SERVER_UPDATE_TIME)
		{
			ulCheckTime = GetTickCount64();
			float fDelta = SERVER_UPDATE_TIME * 0.001f;
			m_fServerOperateTime += fDelta;
			update(fDelta);
		}
	}
}

void MainServer::update(float fDeltaTime)
{

}

void MainServer::release()
{
	THREAD_MGR->stopWorker(m_pServerSocket->getCompletionPort());

	WorkerThreadManager::destroyInstance();
	SessionManager::destroyInstance();

	m_pAcceptor->stopRunning();
	SAFE_DELETE(m_pAcceptor);

	SAFE_DELETE(m_pServerSocket);
}
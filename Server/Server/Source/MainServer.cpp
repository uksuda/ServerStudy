#include "MainServer.h"
#include "ClientSession.h"
#include "ClientSessionManager.h"
#include "WorkerThreadManager.h"
#include "Synchro.h"
#include "ServerSocket.h"
#include "DispatcherServer.h"
#include "Log.h"


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
	SYNCHRO->destroyInstance();
	DISPATCHER_SERVER->destroyInstance();

	SAFE_DELETE(m_pServerSocket);
	CloseHandle(m_hComPort);
}

void MainServer::runServer()
{
	THREADMGR->workBegin(m_hComPort);
	if (THREADMGR->isRunning() == false)
	{
		// thread fail
		CLog::LOG("Thread begin Fail");
		return;
	}

	while (m_bRunServer)
	{
		SOCKADDR_IN clientAddr;
		SOCKET hClientSocket = m_pServerSocket->startAcception(clientAddr);

		if (hClientSocket == INVALID_SOCKET)
		{
			// client socket error
			CLog::LOG("Client Accept", WSAGetLastError());
			continue;
		}
		
		ClientSession* pSession = ClientSession::createSession();
		ClientSession::stSessionInfo& refSessionInfo = pSession->getSessionInfo();

		SYNCHRO->enterCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);

		refSessionInfo.m_ClientSocket = hClientSocket;
		refSessionInfo.m_ClientAddr = clientAddr;
		refSessionInfo.m_userSeq = SESSIONMGR->getCurrentSessionCount() + 1;
		
		SESSIONMGR->insertNewSession(pSession);
		SYNCHRO->leaveCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);

		char szIP[INET_ADDRSTRLEN];
		memset(szIP, 0, sizeof(szIP));

		char szMessage[64];
		memset(szMessage, 0, sizeof(szMessage));
		//inet_ntoa(clientAddr.sin_addr)

		inet_ntop(AF_INET, &(clientAddr.sin_addr), szIP, INET_ADDRSTRLEN);
		snprintf(szMessage, sizeof(szMessage), "Client connedted : %d, %s", refSessionInfo.m_userSeq, szIP);
		CLog::LOG(szMessage);
		
		HANDLE hComPort = CreateIoCompletionPort((HANDLE)refSessionInfo.m_ClientSocket, m_hComPort, (ULONG_PTR)pSession, 0);

		if (hComPort == NULL || hComPort != m_hComPort)
		{
			// error
			SYNCHRO->enterCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);
			SESSIONMGR->removeSession(refSessionInfo.m_userSeq);
			SYNCHRO->leaveCriticalSection(Synchro::SYNC_TARGET::SYNC_SESSION);
			CLog::LOG("CreateIoCompletionPort Connect", GetLastError());
			continue;
		}

		DWORD dwFlag = 0;
		DWORD dwRecvNumBytes = 0;

		refSessionInfo.m_Wsabuf.len = S_BUFFER_SIZE;
		refSessionInfo.m_Wsabuf.buf = refSessionInfo.m_ReceiveBuffer;
		refSessionInfo.eMode = ClientSession::IO_MODE::MODE_READ;
		
		int iRetValue = WSARecv(refSessionInfo.m_ClientSocket, &refSessionInfo.m_Wsabuf, 1, &dwRecvNumBytes, &dwFlag, &refSessionInfo.m_Overlapped, NULL);
		if (iRetValue == SOCKET_ERROR && (WSAGetLastError() != WSA_IO_PENDING))
		{
			CLog::LOG("WSARecv", WSAGetLastError());
			continue;
		}
	}

	THREADMGR->setOff();

	CLog::LOG("Server End...");
}

void MainServer::updateServer(float fDelta)
{

}

bool MainServer::initMainServer()
{
	if (SYNCHRO->initSynchro() == false)
	{
		CLog::LOG("Synchro Fail");
		return false;
	}

	if (SESSIONMGR->initClientSessionManager() == false)
	{
		CLog::LOG("Session Manager Fail");
		return false;
	}

	SYSTEM_INFO systemInfo;
	GetSystemInfo(&systemInfo);

	int iThreadCount = systemInfo.dwNumberOfProcessors * 2 + 1;

	m_hComPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);
	if (m_hComPort == NULL)
	{
		//GetLastError();
		CLog::LOG("CreateIoCompletionPort", GetLastError());

		return false;
	}

	if (THREADMGR->initWorkThreadManager(iThreadCount) == false)
	{
		CLog::LOG("Thread Manager Fail");
		return false;
	}

	m_pServerSocket = ServerSocket::createSocket(SERVER_HOST, SERVER_PORT);
	if (m_pServerSocket == nullptr)
	{
		CLog::LOG("ServerSocket Fail");
		return false;
	}

	DISPATCHER_SERVER->getInstance();

	char szMsg[64];
	sprintf_s(szMsg, sizeof(szMsg), "Server Started : %s Port : %d", SERVER_HOST, SERVER_PORT);
	CLog::LOG(szMsg);

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
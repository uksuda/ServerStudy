#include "Acceptor.h"
#include "ServerSocket.h"
#include "SessionManager.h"
#include "ClientSession.h"
#include "Log.h"

Acceptor::Acceptor()
	: m_isRunning(false)
	, m_hCompletionPort(INVALID_HANDLE_VALUE)
	, m_pServerSocket(nullptr)
{

}

Acceptor::~Acceptor()
{
	release();
}

void Acceptor::run()
{
	while (m_isRunning)
	{
		SOCKADDR_IN clientAddr;
		memset(&clientAddr, 0, sizeof(SOCKADDR_IN));

		SOCKET hClientSocket = m_pServerSocket->acceptSocket(&clientAddr);
		if (hClientSocket == INVALID_SOCKET)
		{
			CLog::log("Accept : %d", WSAGetLastError());
			closesocket(hClientSocket);
			continue;
		}

		ClientSession* pSession = new (std::nothrow) ClientSession(hClientSocket, clientAddr);
		if (pSession == nullptr || pSession->initSession() == false)
		{
			CLog::log("Client Session Create Failed");
			SAFE_DELETE(pSession);
			continue;
		}

		HANDLE hResult = CreateIoCompletionPort((HANDLE)hClientSocket, m_hCompletionPort, (ULONG_PTR)pSession, 0);
		if (hResult == NULL || hResult != m_hCompletionPort)
		{
			CLog::log("Client Session IO Connect Failed");
			SAFE_DELETE(pSession);
			continue;
		}

		LINGER stLinger;
		stLinger.l_linger = 0;
		stLinger.l_onoff = 0;

		setsockopt(hClientSocket, SOL_SOCKET, SO_LINGER, (const char*)&stLinger, sizeof(LINGER));

		SESSION_MGR->addNewClientSession(pSession);

		char szIP[INET_ADDRSTRLEN];
		memset(szIP, 0, sizeof(szIP));
		
		inet_ntop(AF_INET, &(clientAddr.sin_addr), szIP, INET_ADDRSTRLEN);
		CLog::log("Client connected : %d, %s", SESSION_MGR->getCurrentSequence(), szIP);

		pSession->recvData();
	}
}

void Acceptor::stopRunning()
{
	if (m_isRunning == false)
	{
		return;
	}

	m_isRunning = false;
	m_pServerSocket->closeSocket();

	HANDLE threadHandle = getThreadHandle();
	if (threadHandle != INVALID_HANDLE_VALUE)
	{
		WaitForSingleObject(threadHandle, INFINITE);
	}
}

bool Acceptor::workingThread(HANDLE hCompletionPort, ServerSocket* pServerSocket)
{
	if (hCompletionPort == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	if (pServerSocket == nullptr)
	{
		return false;
	}

	if (m_isRunning)
	{
		return false;
	}

	m_isRunning = true;
	m_hCompletionPort = hCompletionPort;
	m_pServerSocket = pServerSocket;

	if (ThreadBase::startThread() == false)
	{
		return false;
	}

	return true;
}

void Acceptor::release()
{

}
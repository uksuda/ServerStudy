#include "SendManager.h"
#include "IOContextManager.h"
#include "ClientSession.h"

IMPLEMENT_SINGLETON(SendManager);

SendManager::SendManager()
	: m_isRunning(false)
{
	InitializeCriticalSection(&m_CriticalSection);
}

SendManager::~SendManager()
{
	release();
}

void SendManager::insertSendQueue(ClientSession* pSession, Packet& sendPacket)
{
	if (pSession == nullptr || sendPacket.isValid() == false)
	{
		return;
	}

	stSessionContext* pSendContext = new stSessionContext;
	memset(pSendContext, 0, sizeof(stSessionContext));

	pSendContext->m_pSendSession = pSession;
	pSendContext->m_SendPacket = sendPacket;

	insertSendQueue(pSendContext);
}

void SendManager::insertSendQueue(stSessionContext* pSendContext)
{
	if (pSendContext == nullptr)
	{
		return;
	}

	EnterCriticalSection(&m_CriticalSection);
	m_queueContext.emplace(pSendContext);
	LeaveCriticalSection(&m_CriticalSection);
}

bool SendManager::sendThreadStart()
{
	if (m_isRunning == true)
	{
		return false;
	}

	m_isRunning = true;
	if (ThreadBase::startThread() == false)
	{
		return false;
	}

	return true;
}

void SendManager::stopRunning()
{
	if (m_isRunning == false)
	{
		return;
	}

	m_isRunning = false;
	HANDLE threadHandle = getThreadHandle();
	if (threadHandle != INVALID_HANDLE_VALUE)
	{
		WaitForSingleObject(threadHandle, INFINITE);
	}
}

void SendManager::run()
{
	while (m_isRunning)
	{
		while (m_queueContext.empty() == false)
		{
			stSessionContext* pSendContext = m_queueContext.front();
			if (pSendContext == nullptr || pSendContext->m_pSendSession == nullptr || pSendContext->m_SendPacket.isValid() == false)
			{				
				SAFE_DELETE(pSendContext);
				continue;
			}

			ClientSession* pSession = pSendContext->m_pSendSession;
			stSendIOData* pSendIOData = IOCONTEXT_MGR->getSendIO();
			if (pSendIOData == nullptr)
			{
				SAFE_DELETE(pSendContext);
				continue;
			}

			pSendIOData->m_eMode = E_IO_MODE::E_IO_SEND;
			pSendIOData->m_wDataLength = pSendContext->m_SendPacket.getPacketSize();
			memcpy(pSendIOData->m_btSendBuffer, pSendContext->m_SendPacket.getPacketBuffer(), pSendContext->m_SendPacket.getPacketSize());

			pSession->sendData(pSendIOData);

			SAFE_DELETE(pSendContext);
			m_queueContext.pop();
		}
	}
}

void SendManager::release()
{
	/*while (m_queueContext.empty() != true)
	{
		stSendContext* pSendContext = m_queueContext.front();
		SAFE_DELETE(pSendContext);
		m_queueContext.pop();
	}*/

	std::queue<stSessionContext*> emptyQueue;
	std::swap(m_queueContext, emptyQueue);

	DeleteCriticalSection(&m_CriticalSection);
}
#include "WorkerThreadManager.h"
#include "WorkerThread.h"
#include <algorithm>

IMPLEMENT_SINGLETON(WorkerThreadManager);

WorkerThreadManager::~WorkerThreadManager()
{
	release();
}

bool WorkerThreadManager::isRunning()
{
	int iSize = m_vecThread.size();
	for (int i = 0; i < iSize; ++i)
	{
		if (m_vecThread[i] && m_vecThread[i]->isRunning())
			return true;
	}

	return false;
}

void WorkerThreadManager::setOff()
{
	int iSize = m_vecThread.size();
	for (int i = 0; i < iSize; ++i)
	{
		if (m_vecThread[i])
		{
			m_vecThread[i]->setOff();
		}
	}
}
	
void WorkerThreadManager::workBegin(HANDLE hComPort)
{
	int iSize = m_vecThread.size();
	for (int i = 0; i < iSize; ++i)
	{
		if (m_vecThread[i])
		{
			m_vecThread[i]->workBegin(hComPort);
		}
	}
}

bool WorkerThreadManager::initWorkThreadManager(int iThreadCount)
{
	m_vecThread.reserve(iThreadCount);

	for (int i = 0; i < iThreadCount; ++i)
	{
		WorkerThread* pThread = WorkerThread::create();
		if (pThread == nullptr)
		{
			release();
			return false;
		}

		m_vecThread.push_back(pThread);
	}

	return true;
}

void WorkerThreadManager::release()
{
	int iSize = m_vecThread.size();
	for (int i = 0; i < iSize; ++i)
	{
		WaitForSingleObject(m_vecThread[i]->getThreadHandle(), INFINITE);
	}

	/*
	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pThread) {
		WaitForSingleObject(pThread->getThreadHandle(), INFINITE);
	});*/

	std::for_each(m_vecThread.begin(), m_vecThread.end(), [&](WorkerThread* pThread) {
		SAFE_DELETE(pThread);
	});

	m_vecThread.clear();
}
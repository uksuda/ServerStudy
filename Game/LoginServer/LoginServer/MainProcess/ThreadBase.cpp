#include "ThreadBase.h"

ThreadBase::ThreadBase()
	: m_ThreadHandle(INVALID_HANDLE_VALUE)
{

}

ThreadBase::~ThreadBase()
{
	release();
}

bool ThreadBase::startThread()
{
	if (m_ThreadHandle != INVALID_HANDLE_VALUE)
	{
		return false;
	}

	m_ThreadHandle = (HANDLE)_beginthreadex(nullptr, 0, ThreadBase::runningThread, this, 0, nullptr);
	if (m_ThreadHandle == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	return true;
}

UINT WINAPI ThreadBase::runningThread(LPVOID pArg)
{
	ThreadBase* pThread = static_cast<ThreadBase*>(pArg);
	if (pThread)
	{
		pThread->run();
	}

	return 0;
}

void ThreadBase::release()
{
	CloseHandle(m_ThreadHandle);
}
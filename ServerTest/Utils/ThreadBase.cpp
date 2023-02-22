#include "ThreadBase.h"
#include "Log.h"

ThreadBase::ThreadBase()
	: m_ThreadHandle(INVALID_HANDLE_VALUE)
{

}
ThreadBase::~ThreadBase()
{
	release();
}

UINT WINAPI ThreadBase::runningThread(LPVOID pArgument)
{
	if (pArgument == nullptr)
	{
		CLog::log("Thread arg is null");
		return -1;
	}

	ThreadBase* pThread = static_cast<ThreadBase*>(pArgument);
	if (pThread)
	{
		pThread->run();
	}
	
	return 0;
}

bool ThreadBase::startThread()
{
	m_ThreadHandle = (HANDLE)_beginthreadex(nullptr, 0, ThreadBase::runningThread, (LPVOID)this, 0, nullptr);
	if (m_ThreadHandle == INVALID_HANDLE_VALUE)
	{
		CLog::log("beginthreadex fail");
		return false;
	}

	return true;
}

void ThreadBase::run()
{

}

void ThreadBase::release()
{
	CloseHandle(m_ThreadHandle);
}
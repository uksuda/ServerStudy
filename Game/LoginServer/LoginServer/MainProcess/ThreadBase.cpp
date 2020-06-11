#include "ThreadBase.h"

ThreadBase::ThreadBase()
	: m_ThreadHandle(INVALID_HANDLE_VALUE)
{

}

ThreadBase::~ThreadBase()
{
	release();
}

UINT WINAPI ThreadBase::runThread(LPVOID pArg)
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
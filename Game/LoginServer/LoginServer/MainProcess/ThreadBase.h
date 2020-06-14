#pragma once

#include "BaseDefine.h"

class ThreadBase
{
protected:
	ThreadBase();
	virtual ~ThreadBase();

public:
	HANDLE getThreadHandle() const { return m_ThreadHandle; }

	bool startThread();

	static UINT WINAPI runningThread(LPVOID pArg);
	virtual void run() abstract;
	//virtual void run() = 0;

private:
	HANDLE m_ThreadHandle;

public:
	void release();
};
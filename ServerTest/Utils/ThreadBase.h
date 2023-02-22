#pragma once

#include "BaseDefine.h"

class ThreadBase
{
protected:
	ThreadBase();
	virtual ~ThreadBase();

public:
	HANDLE getThreadHandle() const
	{
		return m_ThreadHandle;
	}
public:
	static UINT WINAPI runningThread(LPVOID pArgument);

	bool startThread();
	
	// virtual void run() = 0;
	virtual void run() abstract;

private:
	HANDLE m_ThreadHandle;

public:
	void release();
};
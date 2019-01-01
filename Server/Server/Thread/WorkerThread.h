#ifndef __WORKER_THREAD_H__
#define __WORKER_THREAD_H__

#include "ServerHeader.h"

class WorkerThread
{
private:
	WorkerThread();
public:
	~WorkerThread();

public:
	HANDLE getThreadHandle() const { return m_ThreadHandle; }
	bool isRunning() const { return m_isRunning; }
	void setOff() { m_isLoop = false; }

public:
	void workBegin(HANDLE hComPort);
	void workRunning();

private:
	HANDLE m_ThreadHandle;
	HANDLE m_hComport;
	unsigned int m_dwThreadID;
	bool m_isRunning;
	bool m_isLoop;

public:
	static WorkerThread* create();
	static unsigned int WINAPI entryThread(LPVOID pParameter);
};

#endif
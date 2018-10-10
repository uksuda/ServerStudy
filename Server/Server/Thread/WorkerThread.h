#ifndef __WORKER_THREAD_H__
#define __WORKER_THREAD_H__

#include <Windows.h>
#include <process.h>

class WorkerThread
{
private:
	WorkerThread();
public:
	~WorkerThread();

public:
	bool isRunning() { return m_isRunning; }

public:
	void workBegin(HANDLE hComPort);
	void workRunning();

private:
	HANDLE m_ThreadHandle;
	HANDLE m_hComport;
	unsigned int m_dwThreadID;
	bool m_isRunning;

public:
	static WorkerThread* createThread();
	static unsigned int WINAPI entryThread(LPVOID pParameter);
};

#endif
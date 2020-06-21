#include "Acceptor.h"

Acceptor::Acceptor()
	: m_isRunning(false)
	, m_hCompletionPort(INVALID_HANDLE_VALUE)
{

}

Acceptor::~Acceptor()
{
	release();
}

bool Acceptor::startAcceptorThread(HANDLE hCompletionPort)
{
	if (m_isRunning == true || hCompletionPort == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	m_hCompletionPort = hCompletionPort;
	if (ThreadBase::startThread() == false)
	{
		return false;
	}

	m_isRunning = true;
	return true;
}

void Acceptor::stopAcceptorThread()
{
	if (m_isRunning == false || getThreadHandle() == INVALID_HANDLE_VALUE)
	{
		return;
	}

	m_isRunning = false;
	WaitForSingleObject(getThreadHandle(), INFINITE);
	return;
}

void Acceptor::run()
{
	while (m_isRunning)
	{

	}
}

void Acceptor::release()
{

}

#include "MainClient.h"
#include "ClientSocket.h"

MainClient::MainClient()
	: m_ulStartTick(0)
	, m_ulEndTick(0)
	, m_pClientSocket(nullptr)
{

}

MainClient::~MainClient()
{
	release();
}

bool MainClient::initializeClient()
{
	//m_pClientSocket = new ClientSocket;
	m_pClientSocket = std::make_shared<ClientSocket>();
	if (m_pClientSocket->initializeClientSocket() == false)
	{
		return false;
	}

	return true;
}

void MainClient::startTestLoop(unsigned int iTestTime)
{
	
}

void MainClient::release()
{
	//SAFE_DELETE(m_pClientSocket);
}

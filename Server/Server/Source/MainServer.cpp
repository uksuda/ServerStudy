#include "MainServer.h"


MainServer::MainServer()
	: m_ServerSocket(nullptr)
{

}

MainServer::~MainServer()
{

}

void MainServer::runServer()
{

}

void MainServer::updateServer(float fDelta)
{

}

bool MainServer::initMainServer()
{


	return true;
}

MainServer* MainServer::createMainServer()
{
	MainServer* pServer = new MainServer;
	if (pServer && pServer->initMainServer())
	{
		return pServer;
	}

	return nullptr;
}
#include "MainServer.h"

MainServer::MainServer()
{

}

MainServer::~MainServer()
{

}

void MainServer::runServer()
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
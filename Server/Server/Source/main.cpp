#include <iostream>
#include "MainServer.h"

int main()
{
	MainServer* server = MainServer::createMainServer();
	if (server == nullptr)
	{
		return -1;
	}

	server->setServerState(true);
	server->runServer();
	
	return 0;
}
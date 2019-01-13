#include <iostream>
#include "ServerIP.h"
#include "ClientSocket.h"


int main()
{
	ClientSocket* pClient = ClientSocket::createSocket();
	if (pClient && pClient->connectTo(SERVER_HOST, SERVER_PORT))
	{
		pClient->clientStart();
	}

	return 0;
}
#include <iostream>
#include <process.h>
#include "ServerIP.h"
#include "ClientSocket.h"
#include "Packet.h"

#ifdef SOCKET_TEST
#define MAX_CLIENT 1000
unsigned int WINAPI entryThread(LPVOID pParameter)
{
	auto Socket = (ClientSocket*)pParameter;
	if (Socket)
	{
		Socket->clientStart();
	}

	return 0;
}
#endif


int main()
{
	/*
	ClientSocket* pClient = ClientSocket::createSocket();
	if (pClient && pClient->connectTo(SERVER_HOST, SERVER_PORT))
	{
		pClient->clientStart();
	}

	if (pClient)
	{
		delete pClient;
		pClient = nullptr;
	}*/

	return 0;
}
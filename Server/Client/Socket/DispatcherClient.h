#ifndef _DISPATCHER_CLIENT_H__
#define _DISPATCHER_CLIENT_H__

#include "ServerHeader.h"
#include "PacketDispatcher.h"

#define DISPATCHER_CLIENT DispatcherClient::getInstance()
#define RECV_BUFFER BUFFER_SIZE * 5

class DispatcherClient : public PacketDispatcher
{
private:
	DispatcherClient() = delete;
	DispatcherClient(E_DISPATCH eType);

public:
	virtual ~DispatcherClient();

private:
	static DispatcherClient* m_pInstance;
	char m_ReceiveBuffer[RECV_BUFFER];

public:
	static DispatcherClient* getInstance();
	static void destroyInstance();

public:
	virtual bool PacketDispatch(Packet receivePacket);	
};

#endif
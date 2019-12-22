#ifndef _DISPATCHER_CLIENT_H__
#define _DISPATCHER_CLIENT_H__

#include "ServerHeader.h"
#include "PacketDispatcher.h"

#include <map>
#include <functional>

#define DISPATCHER_CLIENT DispatcherClient::getInstance()
#define RECV_BUFFER BUFFER_SIZE * 5

class DispatcherClient : public PacketDispatcher
{
private:
	DispatcherClient() = delete;
	DispatcherClient(E_DISPATCH eType);

public:
	virtual ~DispatcherClient();

	using DISPATCH_FUNC = std::function<void(Packet& receivePacket)>;
	using DISPATCH_MAP = std::map<E_PACKET_SERVER_TO_CLIENT, DISPATCH_FUNC>;
	using DISPATCH_ITER = std::map<E_PACKET_SERVER_TO_CLIENT, DISPATCH_FUNC>::iterator;

private:
	static DispatcherClient* m_pInstance;
	char m_ReceiveBuffer[RECV_BUFFER];

	DISPATCH_MAP m_mapFunc;

public:
	static DispatcherClient* getInstance();
	static void destroyInstance();

public:
	virtual bool packetDispatch(Packet& receivePacket);

private:
	void registDispatchFunc();
};

#endif
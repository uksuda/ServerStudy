#ifndef _DISPATCHER_SERVER_H__
#define _DISPATCHER_SERVER_H__

#include "ServerHeader.h"
#include "PacketDispatcher.h"

#include <map>
#include <functional>

#define DISPATCHER_SERVER DispatcherServer::getInstance()

class DispatcherServer : public PacketDispatcher
{
private:
	DispatcherServer() = delete;
	DispatcherServer(E_DISPATCH eType);

public:
	virtual ~DispatcherServer();

	using DISPATCH_FUNC = std::function<void(Packet receivePacket)>;
	using DISPATCH_MAP = std::map<E_PACKET_CLIENT_TO_SERVER, DISPATCH_FUNC>;
	using DISPATCH_ITER = std::map<E_PACKET_CLIENT_TO_SERVER, DISPATCH_FUNC>::iterator;

private:
	static DispatcherServer* m_pInstance;

	DISPATCH_MAP m_mapFunc;

public:
	static DispatcherServer* getInstance();
	static void destroyInstance();

public:
	//virtual bool packetDispatch(Packet receivePacket);
	bool packetDispatchServer(Packet receivePacket);

private:
	void registDispatchFunc();
};

#endif
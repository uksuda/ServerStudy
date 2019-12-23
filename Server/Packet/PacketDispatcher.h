#ifndef _PACKET_DISPATCHER_H__
#define _PACKET_DISPATCHER_H__

#pragma warning(disable : 26812)

class PacketDispatcher
{
public:
	enum class E_DISPATCH
	{
		DISPATCH_CLIENT = 0,
		DISPATCH_SERVER
	};

protected:
	PacketDispatcher() = delete;
	PacketDispatcher(E_DISPATCH eDispatch);
	virtual ~PacketDispatcher() = 0;

public:
	virtual bool packetDispatch(Packet& receivePacket);

private:
	E_DISPATCH m_eDispatcherType;
};

#endif
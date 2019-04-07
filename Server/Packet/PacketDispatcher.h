#ifndef _PACKET_DISPATCHER_H__
#define _PACKET_DISPATCHER_H__

class PacketDispatcher
{
protected:
	PacketDispatcher() = delete;
	PacketDispatcher(E_DISPATCH eDispatch);
	virtual ~PacketDispatcher();
	
public:
	enum class E_DISPATCH
	{
		DISPATCH_CLIENT = 0,
		DISPATCH_SERVER
	};
	
public:
	virtual bool PacketDispatch(Packet receivePacket);
	
private:
	E_DISPATCH eDispatcherType;
}

#endif
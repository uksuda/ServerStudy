#include "Packet.h"
#include "PacketDispatcher.h"

PacketDispatcher::PacketDispatcher(E_DISPATCH eDispatch)
	: eDispatcherType(eDispatch)
{

}

PacketDispatcher::~PacketDispatcher()
{

}

bool PacketDispatcher::PacketDispatch(Packet receivePacket)
{
	receivePacket.getPacketID();
}
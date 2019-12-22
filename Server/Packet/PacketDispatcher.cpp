#include "Packet.h"
#include "PacketDispatcher.h"

PacketDispatcher::PacketDispatcher(E_DISPATCH eDispatch)
	: m_eDispatcherType(eDispatch)
{

}

PacketDispatcher::~PacketDispatcher()
{

}

bool PacketDispatcher::packetDispatch(Packet& receivePacket)
{

	if (m_eDispatcherType == E_DISPATCH::DISPATCH_CLIENT)
	{
		E_PACKET_SERVER_TO_CLIENT ePacketID = static_cast<E_PACKET_SERVER_TO_CLIENT>(receivePacket.getPacketID());
		if (ePacketID == INVALID_PACKET_ID)
			return false;
		else
			return true;
	}
	else if (m_eDispatcherType == E_DISPATCH::DISPATCH_SERVER)
	{
		E_PACKET_CLIENT_TO_SERVER ePacketID = static_cast<E_PACKET_CLIENT_TO_SERVER>(receivePacket.getPacketID());
		if (ePacketID == INVALID_PACKET_ID)
			return false;
		else
			return true;
	}

	return false;
}
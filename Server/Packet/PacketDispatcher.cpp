#include "Packet.h"
#include "PacketDispatcher.h"

PacketDispatcher::PacketDispatcher(E_DISPATCH eDispatch)
	: m_eDispatcherType(eDispatch)
{

}

PacketDispatcher::~PacketDispatcher()
{

}

bool PacketDispatcher::packetDispatch(Packet receivePacket)
{

	if (m_eDispatcherType == E_DISPATCH::DISPATCH_CLIENT)
	{
		E_PID_STC ePacketID = static_cast<E_PID_STC>(receivePacket.getPacketID());
		if (ePacketID == E_PID_STC::ID_INVALID)
			return false;
		else
			return true;
	}
	else if (m_eDispatcherType == E_DISPATCH::DISPATCH_SERVER)
	{
		E_PID_CTS ePacketID = static_cast<E_PID_CTS>(receivePacket.getPacketID());
		if (ePacketID == E_PID_CTS::ID_INVALID)
			return false;
		else
			return true;
	}

	return false;
}
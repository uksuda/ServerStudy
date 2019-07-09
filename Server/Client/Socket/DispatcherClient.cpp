#include "DispatcherClient.h"

DispatcherClient* DispatcherClient::m_pInstance = nullptr;

DispatcherClient* DispatcherClient::getInstance()
{
	if (m_pInstance == nullptr)
	{
		m_pInstance = new DispatcherClient(E_DISPATCH::DISPATCH_CLIENT);
	}

	return m_pInstance;
}

void DispatcherClient::destroyInstance()
{
	SAFE_DELETE(m_pInstance);
}

DispatcherClient::DispatcherClient(E_DISPATCH eType)
	: PacketDispatcher(eType)
{
	memset(m_ReceiveBuffer, 0, sizeof(m_ReceiveBuffer));
}

DispatcherClient::~DispatcherClient()
{

}

bool DispatcherClient::PacketDispatch(Packet receivePacket)
{
	if (PacketDispatcher::PacketDispatch(receivePacket) == false)
	{
		return false;
	}



	return true;
}
#include "DispatcherClient.h"
#include "DispatchFunc.h"
#include "Log.h"

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
	m_mapFunc.clear();

	registDispatchFunc();
}

DispatcherClient::~DispatcherClient()
{
	m_mapFunc.clear();
}

bool DispatcherClient::packetDispatch(Packet& receivePacket)
{
	if (PacketDispatcher::packetDispatch(receivePacket) == false)
	{
		return false;
	}

	unsigned int iPacketID = receivePacket.getPacketID();
	E_PACKET_SERVER_TO_CLIENT ePacketID = static_cast<E_PACKET_SERVER_TO_CLIENT>(iPacketID);

	DISPATCH_ITER iter = m_mapFunc.find(ePacketID);
	if (iter == m_mapFunc.end())
	{
		CLog::LOG("Packet Dispatch Function invalid %d", iPacketID);
		return false;
	}

	iter->second(receivePacket);

	return true;
}

void DispatcherClient::registDispatchFunc()
{
	m_mapFunc.insert(DISPATCH_MAP::value_type(E_PACKET_SERVER_TO_CLIENT::ID_LOGIN_OK, DispatchFunc::funcLoginOK));
	m_mapFunc.insert(DISPATCH_MAP::value_type(E_PACKET_SERVER_TO_CLIENT::ID_CHAT_MESSAGE_FROM, DispatchFunc::funcChatMessage));
}
#include "DispatcherServer.h"
#include "DispatcherFunc.h"
#include "Log.h"

DispatcherServer* DispatcherServer::m_pInstance = nullptr;

DispatcherServer::DispatcherServer(E_DISPATCH eType)
	: PacketDispatcher(eType)
{
	m_mapFunc.clear();
	registDispatchFunc();
}

DispatcherServer::~DispatcherServer()
{
	m_mapFunc.clear();
}

DispatcherServer* DispatcherServer::getInstance()
{
	if (m_pInstance == nullptr)
	{
		m_pInstance = new DispatcherServer(E_DISPATCH::DISPATCH_SERVER);
	}

	return m_pInstance;
}

void DispatcherServer::destroyInstance()
{
	SAFE_DELETE(m_pInstance);
}

bool DispatcherServer::packetDispatchServer(Packet receivePacket)
{
	if (PacketDispatcher::packetDispatch(receivePacket) == false)
	{
		return false;
	}

	unsigned int iPacketID = receivePacket.getPacketID();
	E_PACKET_CLIENT_TO_SERVER ePacketID = static_cast<E_PACKET_CLIENT_TO_SERVER>(iPacketID);

	DISPATCH_ITER iter = m_mapFunc.find(ePacketID);
	if (iter == m_mapFunc.end())
	{
		CLog::LOG("Packet Dispatch Function invalid %d", iPacketID);
		return false;
	}

	iter->second(receivePacket);

	return true;
}

void DispatcherServer::registDispatchFunc()
{
	m_mapFunc.insert(DISPATCH_MAP::value_type(E_PACKET_CLIENT_TO_SERVER::ID_LOGIN_REQ, DispatchFunc::funcLoginReq));
	m_mapFunc.insert(DISPATCH_MAP::value_type(E_PACKET_CLIENT_TO_SERVER::ID_CHAT_MESSAGE, DispatchFunc::funcChatMessage));
}
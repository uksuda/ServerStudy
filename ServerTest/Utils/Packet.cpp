#include "Packet.h"
#include "BaseDefine.h"
#include "Log.h"

Packet::Packet()
	: m_wPacketID(INVALID_PACKET_ID)
	, m_wBufferPosition(PACKET_HEADER_SIZE)
	, m_wReadBufferPosition(PACKET_HEADER_SIZE)
{
	memset(m_btPacketBuffer, 0, sizeof(m_btPacketBuffer));
}

Packet::Packet(unsigned short wPacketID)
	: m_wPacketID(wPacketID)
	, m_wBufferPosition(PACKET_HEADER_SIZE)
	, m_wReadBufferPosition(PACKET_HEADER_SIZE)
{
	memset(m_btPacketBuffer, 0, sizeof(m_btPacketBuffer));
}

Packet::~Packet()
{
	
}

bool Packet::addPacket(unsigned char nData)
{
	if (m_wBufferPosition + sizeof(unsigned char) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(unsigned char).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(unsigned char));
	m_wBufferPosition += sizeof(unsigned char);

	return true;
}

bool Packet::addPacket(unsigned short nData)
{
	if (m_wBufferPosition + sizeof(unsigned short) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(unsigned short).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(unsigned short));
	m_wBufferPosition += sizeof(unsigned short);

	return true;
}

bool Packet::addPacket(unsigned int nData)
{
	if (m_wBufferPosition + sizeof(unsigned int) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(unsigned int).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(unsigned int));
	m_wBufferPosition += sizeof(unsigned int);

	return true;
}

bool Packet::addPacket(unsigned long nData)
{
	if (m_wBufferPosition + sizeof(unsigned long) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(unsigned long).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(unsigned long));
	m_wBufferPosition += sizeof(unsigned long);

	return true;
}

bool Packet::addPacket(unsigned long long nData)
{
	if (m_wBufferPosition + sizeof(unsigned long long) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(unsigned long long).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(unsigned long long));
	m_wBufferPosition += sizeof(unsigned long long);

	return true;
}

bool Packet::addPacket(bool nData)
{
	if (m_wBufferPosition + sizeof(bool) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(bool).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(bool));
	m_wBufferPosition += sizeof(bool);

	return true;
}
	
bool Packet::addPacket(float nData)
{
	if (m_wBufferPosition + sizeof(float) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(float).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(float));
	m_wBufferPosition += sizeof(float);

	return true;
}
	
bool Packet::addPacket(double nData)
{
	if (m_wBufferPosition + sizeof(double) > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s", typeid(double).name());
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, &nData, sizeof(double));
	m_wBufferPosition += sizeof(double);

	return true;
}

bool Packet::addPacket(char* pData, int iSize)
{
	if (m_wBufferPosition + iSize > PACKET_BUFFER_SIZE)
	{
		CLog::log("packet overflow : %s - %d", typeid(char).name(), iSize);
		return false;
	}

	memcpy(m_btPacketBuffer + m_wBufferPosition, pData, iSize);
	m_wBufferPosition += iSize;

	return true;
}

bool Packet::addPacketEnd()
{
	if (writePacketSize() == false)
	{
		CLog::log("%s -- size", __FUNCTION__);
		return false;
	}

	if (writePacketID() == false)
	{
		CLog::log("%s -- ID %d", __FUNCTION__, m_wPacketID);
		return false;
	}

	return true;
}

//
bool Packet::getDataFromPacket(unsigned char* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(unsigned char).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(unsigned char) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(unsigned char).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(unsigned char));
	m_wReadBufferPosition += sizeof(unsigned char);

	return true;
}
	
bool Packet::getDataFromPacket(unsigned short* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(unsigned short).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(unsigned short) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(unsigned short).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(unsigned short));
	m_wReadBufferPosition += sizeof(unsigned short);

	return true;
}
	
bool Packet::getDataFromPacket(unsigned int* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(unsigned int).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(unsigned int) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(unsigned int).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(unsigned int));
	m_wReadBufferPosition += sizeof(unsigned int);

	return true;
}
	
bool Packet::getDataFromPacket(unsigned long* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(unsigned long).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(unsigned long) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(unsigned long).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(unsigned long));
	m_wReadBufferPosition += sizeof(unsigned long);

	return true;
}
	
bool Packet::getDataFromPacket(unsigned long long* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(unsigned long long).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(unsigned long long) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(unsigned long long).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(unsigned long long));
	m_wReadBufferPosition += sizeof(unsigned long long);

	return true;
}

bool Packet::getDataFromPacket(bool* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(bool).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(bool) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(bool).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(bool));
	m_wReadBufferPosition += sizeof(bool);

	return true;
}
	
bool Packet::getDataFromPacket(float* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(float).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(float) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(float).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(float));
	m_wReadBufferPosition += sizeof(float);

	return true;
}
	
bool Packet::getDataFromPacket(double* pData)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s", __FUNCTION__, typeid(double).name());
		return false;
	}

	if (m_wReadBufferPosition + sizeof(double) > m_wBufferPosition)
	{
		CLog::log("read over buffer %s", typeid(double).name());
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, sizeof(double));
	m_wReadBufferPosition += sizeof(double);

	return true;
}

bool Packet::getDataFromPacket(char* pData, int iSize)
{
	if (pData == nullptr)
	{
		CLog::log("fail %s - null %s size %d", __FUNCTION__, typeid(char).name(), iSize);
		return false;
	}

	if (m_wReadBufferPosition + iSize > m_wBufferPosition)
	{
		CLog::log("read over buffer %s - %d", typeid(char).name(), iSize);
		return false;
	}

	memcpy(pData, m_btPacketBuffer + m_wReadBufferPosition, iSize);
	m_wReadBufferPosition += iSize;

	return true;
}

bool Packet::getPacketDataFromRecvBuffer(unsigned char* pData, unsigned int iBufferSize)
{
	m_wPacketID = getRecvPacketID(pData, iBufferSize);
	if (m_wPacketID == INVALID_PACKET_ID)
	{
		return false;
	}

	unsigned short wPacketSize = getRecvPacketSize(pData, iBufferSize);

	if (wPacketSize < PACKET_HEADER_SIZE)
	{
		CLog::log("fail %s - buffer size small than header %d", __FUNCTION__, wPacketSize);
		return false;
	}

	m_wBufferPosition = wPacketSize - PACKET_HEADER_SIZE;
	memcpy(m_btPacketBuffer, pData, wPacketSize);
	return true;
}

//
unsigned short Packet::getRecvPacketSize(unsigned char* pData, unsigned int iBufferSize)
{
	unsigned short wSize = 0;
	if (iBufferSize < sizeof(unsigned short))
	{
		CLog::log("fail %s - buffer size %d", __FUNCTION__, iBufferSize);
		return wSize;
	}

	memcpy(&wSize, pData, sizeof(unsigned short));

	if (wSize > PACKET_BUFFER_SIZE)
	{
		CLog::log("fail %s - buffer oversize %d", __FUNCTION__, wSize);
		wSize = 0;
	}

	return wSize;
}

unsigned short Packet::getRecvPacketID(unsigned char* pData, unsigned int iBufferSize)
{
	unsigned short wID = INVALID_PACKET_ID;
	if (iBufferSize < PACKET_HEADER_SIZE)
	{
		CLog::log("fail %s - buffer size %d", __FUNCTION__, iBufferSize);
		return wID;
	}

	memcpy(&wID, pData + sizeof(unsigned short), sizeof(unsigned short));
	return wID;
}

bool Packet::writePacketID()
{
	if (isValid() == false)
	{
		return false;
	}

	memcpy(m_btPacketBuffer + sizeof(unsigned short), &m_wPacketID, sizeof(unsigned short));
	return true;
}

bool Packet::writePacketSize()
{
	if (isValid() == false)
	{
		return false;
	}

	memcpy(m_btPacketBuffer, &m_wBufferPosition, sizeof(unsigned short));
	return true;
}
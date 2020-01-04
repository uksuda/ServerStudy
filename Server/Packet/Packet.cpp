#include "Packet.h"
#include "../Log/Log.h"
#include <iostream>

Packet::Packet(unsigned int iPacketID)
	: m_iPacketID(iPacketID)
	, m_iPacketSize(PACKET_HEADER_SIZE/*sizeof(m_iPacketID) + sizeof(m_iPacketSize)*/)
	, m_iReadPosition(PACKET_HEADER_SIZE)
{
	memset(m_btBuffer, 0, sizeof(m_btBuffer));
}

Packet::Packet(const Packet& packet)
	: m_iPacketID(packet.m_iPacketID)
	, m_iPacketSize(packet.m_iPacketSize)
	, m_iReadPosition(packet.m_iReadPosition)
{
	memcpy(m_btBuffer, packet.m_btBuffer, sizeof(m_btBuffer));
}

Packet::~Packet()
{

}

bool Packet::add(unsigned char nData)
{
	if (m_iPacketSize + sizeof(unsigned char) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(unsigned char));
	m_iPacketSize += sizeof(unsigned char);
	return true;
}

bool Packet::add(char nData)
{
	if (m_iPacketSize + sizeof(char) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(char));
	m_iPacketSize += sizeof(char);
	return true;
}

bool Packet::add(unsigned short nData)
{
	if (m_iPacketSize + sizeof(unsigned short) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(unsigned short));
	m_iPacketSize += sizeof(unsigned short);
	return true;
}

bool Packet::add(short nData)
{
	if (m_iPacketSize + sizeof(short) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(short));
	m_iPacketSize += sizeof(short);
	return true;
}

bool Packet::add(unsigned int nData)
{
	if (m_iPacketSize + sizeof(unsigned int) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(unsigned int));
	m_iPacketSize += sizeof(unsigned int);
	return true;
}

bool Packet::add(int nData)
{
	if (m_iPacketSize + sizeof(int) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(int));
	m_iPacketSize += sizeof(int);
	return true;
}

bool Packet::add(unsigned long nData)
{
	if (m_iPacketSize + sizeof(unsigned long) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(unsigned long));
	m_iPacketSize += sizeof(unsigned long);
	return true;
}

bool Packet::add(long nData)
{
	if (m_iPacketSize + sizeof(long) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(long));
	m_iPacketSize += sizeof(long);
	return true;
}

bool Packet::add(float nData)
{
	if (m_iPacketSize + sizeof(float) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(float));
	m_iPacketSize += sizeof(float);
	return true;
}

bool Packet::add(double nData)
{
	if (m_iPacketSize + sizeof(double) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(nData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &nData, sizeof(double));
	m_iPacketSize += sizeof(double);
	return true;
}

bool Packet::add(bool bData)
{
	if (m_iPacketSize + sizeof(bool) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s", typeid(bData).name());
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, &bData, sizeof(bool));
	m_iPacketSize += sizeof(bool);
	return true;
}

bool Packet::add(char* pData, int iDataLength)
{
	if (m_iPacketSize + sizeof(char) * iDataLength > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to add packet data : %s, %d", typeid(pData).name(), iDataLength);
		return false;
	}

	memcpy(m_btBuffer + m_iPacketSize, pData, sizeof(char) * iDataLength);
	m_iPacketSize += sizeof(char) * iDataLength;
	return true;
}

//
bool Packet::getDataFromPacket(unsigned char* pData)
{
	if (m_iReadPosition + sizeof(unsigned char) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(unsigned char).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(unsigned char));
	m_iReadPosition += sizeof(unsigned char);
	return true;
}

bool Packet::getDataFromPacket(char* pData)
{
	if (m_iReadPosition + sizeof(char) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(char).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(char));
	m_iReadPosition += sizeof(char);
	return true;
}

bool Packet::getDataFromPacket(char* pData, int iDataLength)
{
	if (m_iReadPosition + sizeof(char) * iDataLength > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s, %d", typeid(char).name(), iDataLength);
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(char) * iDataLength);
	m_iReadPosition += sizeof(char) * iDataLength;
	return true;
}

bool Packet::getDataFromPacket(unsigned short* pData)
{
	if (m_iReadPosition + sizeof(unsigned short) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(unsigned short).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(unsigned short));
	m_iReadPosition += sizeof(unsigned short);
	return true;
}

bool Packet::getDataFromPacket(short* pData)
{
	if (m_iReadPosition + sizeof(short) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(short).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(short));
	m_iReadPosition += sizeof(short);
	return true;
}

bool Packet::getDataFromPacket(unsigned int* pData)
{
	if (m_iReadPosition + sizeof(unsigned int) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(unsigned int).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(unsigned int));
	m_iReadPosition += sizeof(unsigned int);
	return true;
}

bool Packet::getDataFromPacket(int* pData)
{
	if (m_iReadPosition + sizeof(int) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(int).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(int));
	m_iReadPosition += sizeof(int);
	return true;
}

bool Packet::getDataFromPacket(unsigned long* pData)
{
	if (m_iReadPosition + sizeof(unsigned long) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(unsigned long).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(unsigned long));
	m_iReadPosition += sizeof(unsigned long);
	return true;
}

bool Packet::getDataFromPacket(long* pData)
{
	if (m_iReadPosition + sizeof(long) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(long).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(long));
	m_iReadPosition += sizeof(long);
	return true;
}

bool Packet::getDataFromPacket(float* pData)
{
	if (m_iReadPosition + sizeof(float) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(float).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(float));
	m_iReadPosition += sizeof(float);
	return true;
}

bool Packet::getDataFromPacket(double* pData)
{
	if (m_iReadPosition + sizeof(double) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(double).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(double));
	m_iReadPosition += sizeof(double);
	return true;
}

bool Packet::getDataFromPacket(bool* pData)
{
	if (m_iReadPosition + sizeof(bool) > PACKET_BUFFER_SIZE)
	{
		CLog::LOG("fail to get data from packet : %s", typeid(bool).name());
		return false;
	}

	memcpy(pData, m_btBuffer + m_iReadPosition, sizeof(bool));
	m_iReadPosition += sizeof(bool);
	return true;
}

bool Packet::getDataFromRecvBuffer(char* pData, unsigned int iSize)
{
	if (iSize < PACKET_HEADER_SIZE)
	{
		CLog::LOG("invalid packet size - small than header size %d", iSize);
		return false;
	}

	unsigned int iPacketSize = getReceivedPacketSize(pData, iSize);
	if (iPacketSize == 0 || iPacketSize < PACKET_HEADER_SIZE)
	{
		return false;
	}

	m_iPacketSize = iPacketSize;
	m_iPacketID = getReceivedPacketID(pData, iSize);

	memcpy(m_btBuffer, pData, m_iPacketSize);
	return true;
}

unsigned int Packet::getReceivedPacketID(char* pData, unsigned int iSize)
{
	unsigned int iPacketID = INVALID_PACKET_ID;
	if (iSize < PACKET_HEADER_SIZE)
	{
		CLog::LOG("invalid packet id");
		return iPacketID;
	}

	memcpy(&iPacketID, pData + sizeof(unsigned int), sizeof(unsigned int));
	return iPacketID;
}
unsigned int Packet::getReceivedPacketSize(char* pData, unsigned int iSize)
{
	unsigned int iPacketSize = 0;
	if (iSize < sizeof(unsigned int))
	{
		CLog::LOG("invalid packet size %d", iSize);
		return iPacketSize;
	}

	memcpy(&iPacketSize, pData, sizeof(unsigned int));
	return iPacketSize;
}

bool Packet::writePacketID()
{
	if (m_iPacketID == INVALID_PACKET_ID)
	{
		return false;
	}

	memcpy(m_btBuffer + sizeof(unsigned int), &m_iPacketID, sizeof(unsigned int));
	return true;
}

bool Packet::writePacketSize()
{
	if (m_iPacketSize == 0)
	{
		return false;
	}

	memcpy(m_btBuffer, &m_iPacketSize, sizeof(unsigned int));
	return true;
}
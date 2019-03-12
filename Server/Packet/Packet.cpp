#include "Packet.h"

//
//private:
//	unsigned int m_iPacketID;
//	unsigned int m_iPacketSize;
//
//	unsigned int m_iBufferPosition;
//	char m_Buffer[PACKET_BUFFER_SIZE];
Packet::Packet(unsigned int iPacketID)
	: m_iPacketID(iPacketID)
	, m_iPacketSize(0)
	, m_iBufferPosition(0)
{
	memset(m_Buffer, 0, sizeof(m_Buffer));
}

Packet::~Packet()
{

}

bool Packet::add(unsigned char nData)
{
	return true;
}

bool Packet::add(char nData)
{
	return true;
}

bool Packet::add(unsigned short nData)
{
	return true;
}

bool Packet::add(short nData)
{
	return true;
}

bool Packet::add(unsigned int nData)
{
	return true;
}

bool Packet::add(int nData)
{
	return true;
}

bool Packet::add(unsigned long nData)
{
	return true;
}

bool Packet::add(long nData)
{
	return true;
}

bool Packet::add(float nData)
{
	return true;
}

bool Packet::add(double nData)
{
	return true;
}

bool Packet::add(bool bData)
{
	return true;
}

bool Packet::add(char* pData, int iDataLength)
{
	return true;
}

bool Packet::getDataFromPacket(unsigned char* pData)
{
	return true;
}

bool Packet::getDataFromPacket(char* pData)
{
	return true;
}

bool Packet::getDataFromPacket(unsigned short* pData)
{
	return true;
}

bool Packet::getDataFromPacket(short* pData)
{
	return true;
}

bool Packet::getDataFromPacket(unsigned int* pData)
{
	return true;
}

bool Packet::getDataFromPacket(int* pData)
{
	return true;
}

bool Packet::getDataFromPacket(unsigned long* pData)
{
	return true;
}

bool Packet::getDataFromPacket(long* pData)
{
	return true;
}

bool Packet::getDataFromPacket(float* pData)
{
	return true;
}

bool Packet::getDataFromPacket(double* pData)
{
	return true;
}

bool Packet::getDataFromPacket(bool* pData)
{
	return true;
}

bool Packet::getDataFromPacket(void* pData)
{
	return true;
}

int Packet::passDataToBuffer(char* pBuffer)
{
	return true;
}

bool Packet::clearPacket()
{
	return true;
}
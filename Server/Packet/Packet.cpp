#include "Packet.h"
#include "../Log/Log.h"

Packet::Packet(unsigned int iPacketID)
	: m_iPacketID(iPacketID)
	, m_iPacketSize(0)
	, m_iBufferSize(PACKET_HEADER_SIZE/*sizeof(m_iPacketID) + sizeof(m_iPacketSize)*/)
	, m_iReadPosition(0)
{
	memset(m_Buffer, 0, sizeof(m_Buffer));
	memset(m_szError, 0, sizeof(m_szError));
}

Packet::~Packet()
{

}

bool Packet::add(unsigned char nData)
{
	if (m_iBufferSize + sizeof(nData) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(nData));
	m_iBufferSize += sizeof(nData);
	return true;
}

bool Packet::add(char nData)
{
	if (m_iBufferSize + sizeof(nData) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}
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
	if (m_iReadPosition + sizeof(pData) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(pData), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(pData));
	return true;
}

bool Packet::getDataFromPacket(char* pData)
{
	return true;
}

bool Packet::getDataFromPacket(char* pData, int iDataLength)
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

int Packet::passDataToBuffer(char* pBuffer)
{
	return true;
}

bool Packet::clearPacket()
{
	return true;
}

void Packet::setErrorMessage(const char* szTypeName, E_ERROR_TYPE eErrorType)
{
	if (szTypeName == nullptr)
		return;

	memset(m_szError, 0, sizeof(m_szError));

	switch (eErrorType)
	{
	case E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE:
		snprintf(m_szError, sizeof(m_szError), "Out of Buffer Size : %s", szTypeName);
		break;
	case E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE:
		snprintf(m_szError, sizeof(m_szError), "Out of Read Position : %s", szTypeName);
		break;
	default:
		break;
	}
}
#include "Packet.h"
#include "../Log/Log.h"
#include <iostream>

Packet::Packet(unsigned int iPacketID)
	: m_iPacketID(iPacketID)
	, m_iPacketSize(0)
	, m_iBufferSize(PACKET_HEADER_SIZE/*sizeof(m_iPacketID) + sizeof(m_iPacketSize)*/)
	, m_iReadPosition(PACKET_HEADER_SIZE)
{
	memset(m_Buffer, 0, sizeof(m_Buffer));
	memset(m_szError, 0, sizeof(m_szError));
}

Packet::Packet(const Packet& packet)
{

}

Packet::~Packet()
{

}

bool Packet::add(unsigned char nData)
{
	if (m_iBufferSize + sizeof(unsigned char) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(unsigned char));
	m_iBufferSize += sizeof(unsigned char);
	return true;
}

bool Packet::add(char nData)
{
	if (m_iBufferSize + sizeof(char) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(char));
	m_iBufferSize += sizeof(char);
	return true;
}

bool Packet::add(unsigned short nData)
{
	if (m_iBufferSize + sizeof(unsigned short) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(unsigned short));
	m_iBufferSize += sizeof(unsigned short);
	return true;
}

bool Packet::add(short nData)
{
	if (m_iBufferSize + sizeof(short) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(short));
	m_iBufferSize += sizeof(short);
	return true;
}

bool Packet::add(unsigned int nData)
{
	if (m_iBufferSize + sizeof(unsigned int) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(unsigned int));
	m_iBufferSize += sizeof(unsigned int);
	return true;
}

bool Packet::add(int nData)
{
	if (m_iBufferSize + sizeof(int) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(int));
	m_iBufferSize += sizeof(int);
	return true;
}

bool Packet::add(unsigned long nData)
{
	if (m_iBufferSize + sizeof(unsigned long) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(unsigned long));
	m_iBufferSize += sizeof(unsigned long);
	return true;
}

bool Packet::add(long nData)
{
	if (m_iBufferSize + sizeof(long) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(long));
	m_iBufferSize += sizeof(long);
	return true;
}

bool Packet::add(float nData)
{
	if (m_iBufferSize + sizeof(float) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(float));
	m_iBufferSize += sizeof(float);
	return true;
}

bool Packet::add(double nData)
{
	if (m_iBufferSize + sizeof(double) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(nData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &nData, sizeof(double));
	m_iBufferSize += sizeof(double);
	return true;
}

bool Packet::add(bool bData)
{
	if (m_iBufferSize + sizeof(bool) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(bData).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, &bData, sizeof(bool));
	m_iBufferSize += sizeof(bool);
	return true;
}

bool Packet::add(char* pData, int iDataLength)
{
	if (m_iBufferSize + sizeof(char) * iDataLength > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(char).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_DATA_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(m_Buffer + m_iBufferSize, pData, sizeof(char) * iDataLength);
	m_iBufferSize += sizeof(char) * iDataLength;
	return true;
}

bool Packet::getDataFromPacket(unsigned char* pData)
{
	if (m_iReadPosition + sizeof(unsigned char) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(unsigned char).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(unsigned char));
	return true;
}

bool Packet::getDataFromPacket(char* pData)
{
	if (m_iReadPosition + sizeof(char) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(char).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(char));
	return true;
}

bool Packet::getDataFromPacket(char* pData, int iDataLength)
{
	if (m_iReadPosition + sizeof(char) * iDataLength > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(char).name(), E_ERROR_TYPE::E_BUFFER_OUT_OF_DATA_SIZE);
		CLog::LOG(m_szError);
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(char) * iDataLength);
	return true;
}

bool Packet::getDataFromPacket(unsigned short* pData)
{
	if (m_iReadPosition + sizeof(unsigned short) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(unsigned short).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(unsigned short));
	return true;
}

bool Packet::getDataFromPacket(short* pData)
{
	if (m_iReadPosition + sizeof(short) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(short).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(short));
	return true;
}

bool Packet::getDataFromPacket(unsigned int* pData)
{
	if (m_iReadPosition + sizeof(unsigned int) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(unsigned int).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(unsigned int));
	return true;
}

bool Packet::getDataFromPacket(int* pData)
{
	if (m_iReadPosition + sizeof(int) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(int).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(int));
	return true;
}

bool Packet::getDataFromPacket(unsigned long* pData)
{
	if (m_iReadPosition + sizeof(unsigned long) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(unsigned long).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(unsigned long));
	return true;
}

bool Packet::getDataFromPacket(long* pData)
{
	if (m_iReadPosition + sizeof(long) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(long).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(long));
	return true;
}

bool Packet::getDataFromPacket(float* pData)
{
	if (m_iReadPosition + sizeof(float) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(float).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(float));
	return true;
}

bool Packet::getDataFromPacket(double* pData)
{
	if (m_iReadPosition + sizeof(double) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(double).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(double));
	return true;
}

bool Packet::getDataFromPacket(bool* pData)
{
	if (m_iReadPosition + sizeof(bool) > PACKET_BUFFER_SIZE)
	{
		setErrorMessage(typeid(bool).name(), E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE);
		CLog::LOG(m_szError);
		return false;
	}

	memcpy(pData, m_Buffer + m_iReadPosition, sizeof(bool));
	return true;
}

int Packet::passDataToBuffer(char* pBuffer)
{
	return true;
}

bool Packet::getHeader(unsigned int* pHeaderID, unsigned int* pPacketSize)
{
	if (pHeaderID == nullptr || pPacketSize == nullptr)
	{
		return false;
	}
	memcpy(pHeaderID, m_Buffer, sizeof(unsigned int));
	memcpy(pPacketSize, m_Buffer + sizeof(unsigned int), sizeof(unsigned int));
	return true;
}

bool Packet::clearPacket()
{
	m_iBufferSize = PACKET_HEADER_SIZE;
	m_iPacketID = 0;
	m_iPacketSize = 0;
	m_iReadPosition = PACKET_HEADER_SIZE;
	memset(m_Buffer, 0, sizeof(m_Buffer));
	memset(m_szError, 0, sizeof(m_szError));
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
	case E_ERROR_TYPE::E_BUFFER_OUT_OF_DATA_SIZE:
		snprintf(m_szError, sizeof(m_szError), "Out of Data Size : %s", szTypeName);
		break;
	case E_ERROR_TYPE::E_BUfFER_OUT_OF_READ_SIZE:
		snprintf(m_szError, sizeof(m_szError), "Out of Read Position : %s", szTypeName);
		break;
	case E_ERROR_TYPE::E_BUFFER_OUT_OF_DATA_READ_SIZE:
		snprintf(m_szError, sizeof(m_szError), "Out of Data Read Position : %s", szTypeName);
		break;
	default:
		break;
	}
}
#ifndef __PACKET_H__
#define __PACKET_H__

#include "PacketID.h"

#define BUFFER_SIZE 4088
#define PACKET_HEADER_SIZE 8
#define PACKET_BUFFER_SIZE PACKET_HEADER_SIZE + BUFFER_SIZE

#define PACKET_ENUM(x) static_cast<unsigned int>(x)

constexpr int ERROR_MSG_LENGTH = 256;

class Packet
{
public:
	enum class E_ERROR_TYPE
	{
		E_BUFFER_OUT_OF_SIZE = 0,
		E_BUFFER_OUT_OF_DATA_SIZE,
		E_BUfFER_OUT_OF_READ_SIZE,
		E_BUFFER_OUT_OF_DATA_READ_SIZE
	};

public:
	Packet() = delete;
	Packet(unsigned int iPacketID);
	Packet(const Packet& packet);
	~Packet();

public:
	bool add(unsigned char nData);
	bool add(char nData);
	bool add(unsigned short nData);
	bool add(short nData);
	bool add(unsigned int nData);
	bool add(int nData);
	bool add(unsigned long nData);
	bool add(long nData);
	bool add(float nData);
	bool add(double nData);
	bool add(bool bData);
	bool add(char* pData, int iDataLength);

	//
	bool getDataFromPacket(unsigned char* pData);
	bool getDataFromPacket(char* pData);
	bool getDataFromPacket(char* pData, int iDataLength);
	bool getDataFromPacket(unsigned short* pData);
	bool getDataFromPacket(short* pData);
	bool getDataFromPacket(unsigned int* pData);
	bool getDataFromPacket(int* pData);
	bool getDataFromPacket(unsigned long* pData);
	bool getDataFromPacket(long* pData);
	bool getDataFromPacket(float* pData);
	bool getDataFromPacket(double* pData);
	bool getDataFromPacket(bool* pData);

	int passDataToBuffer(char* pBuffer);
	void setPacketID(unsigned int iPacketID) { m_iPacketID = iPacketID; };
	bool getHeader(unsigned int* pHeaderID, unsigned int* pPacketSize);
	bool clearPacket();

private:
	unsigned int m_iPacketID;
	unsigned int m_iPacketSize;

	unsigned int m_iBufferSize;
	unsigned int m_iReadPosition;
	char m_Buffer[PACKET_BUFFER_SIZE];
	char m_szError[ERROR_MSG_LENGTH];

private:
	void setErrorMessage(const char* szTypeName, E_ERROR_TYPE eErrorType);
};

#endif
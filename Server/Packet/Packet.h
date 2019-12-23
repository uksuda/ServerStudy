#ifndef __PACKET_H__
#define __PACKET_H__

#include "PacketID.h"

#define BUFFER_SIZE 504
#define PACKET_HEADER_SIZE 8
#define PACKET_BUFFER_SIZE PACKET_HEADER_SIZE + BUFFER_SIZE

class Packet
{
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

	//
	bool isValid() const
	{
		return (INVALID_PACKET_ID != m_iPacketID);
	}

	char* getPacketBuffer()
	{
		return m_btBuffer;
	}

	unsigned int getPacketID() const
	{
		return m_iPacketID;
	}

	unsigned int getPacketSize() const
	{
		return m_iPacketSize;
	}

	//
	static unsigned int getReceivedPacketID(char* pData, unsigned int iSize);
	static unsigned int getReceivedPacketSize(char* pData, unsigned int iSize);

private:
	unsigned int m_iPacketID;
	unsigned int m_iPacketSize;

	unsigned int m_iReadPosition;
	char m_btBuffer[PACKET_BUFFER_SIZE];

private:
	bool writePacketID();
	bool writePacketSize();

	bool readPacketID(char* pData, unsigned int iSize);
	bool readPacketSize(char* pData, unsigned int iSize);
};

#endif
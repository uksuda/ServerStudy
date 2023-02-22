#pragma once

//#include "BaseDefine.h"
#include "PacketHeader.h"

#define PACKET_HEADER_SIZE 4 /*size 2 + packet id 2*/
#define PACKET_BUFFER_SIZE PACKET_HEADER_SIZE + 508

class Packet
{
public:
	//Packet() = delete;
	Packet();
	Packet(unsigned short wPacketID);
	~Packet();

public:
	bool addPacket(unsigned char nData);
	bool addPacket(unsigned short nData);
	bool addPacket(unsigned int nData);
	bool addPacket(unsigned long nData);
	bool addPacket(unsigned long long nData);

	bool addPacket(bool nData);
	bool addPacket(float nData);
	bool addPacket(double nData);

	bool addPacket(char* pData, int iSize);

	bool addPacketEnd();

	//
	bool getDataFromPacket(unsigned char* pData);
	bool getDataFromPacket(unsigned short* pData);
	bool getDataFromPacket(unsigned int* pData);
	bool getDataFromPacket(unsigned long* pData);
	bool getDataFromPacket(unsigned long long* pData);

	bool getDataFromPacket(bool* pData);
	bool getDataFromPacket(float* pData);
	bool getDataFromPacket(double* pData);

	bool getDataFromPacket(char* pData, int iSize);

	//
	void setPacketID(unsigned short wPacketID)
	{
		m_wPacketID = wPacketID;
	}

	unsigned char* getPacketBuffer()
	{
		return m_btPacketBuffer;
	}

	unsigned short getPacketID()
	{
		return m_wPacketID;
	}

	unsigned short getPacketSize()
	{
		return m_wBufferPosition;
	}

	bool isValid() const
	{
		return (INVALID_PACKET_ID != m_wPacketID);
	}

	//
	bool getPacketDataFromRecvBuffer(unsigned char* pData, unsigned int iBufferSize);

	static unsigned short getRecvPacketSize(unsigned char* pData, unsigned int iBufferSize);
	static unsigned short getRecvPacketID(unsigned char* pData, unsigned int iBufferSize);

private:
	unsigned short m_wPacketID;
	unsigned short m_wBufferPosition;
	unsigned short m_wReadBufferPosition;
	unsigned char m_btPacketBuffer[PACKET_BUFFER_SIZE];

private:
	bool writePacketID();
	bool writePacketSize();
};
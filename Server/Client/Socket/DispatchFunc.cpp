#include "DispatchFunc.h"
#include <iostream>

DispatchFunc::DispatchFunc()
{

}

DispatchFunc::~DispatchFunc()
{

}

void DispatchFunc::funcLoginOK(Packet& receivePacket)
{

}

void DispatchFunc::funcChatMessage(Packet& receivePacket)
{
	if (receivePacket.isValid() == false)
	{
		return;
	}

	char szMessage[MESSAGE_SIZE];
	memset(szMessage, 0, sizeof(szMessage));

	unsigned int iSize = receivePacket.getPacketSize();

	receivePacket.getDataFromPacket(szMessage, iSize - PACKET_HEADER_SIZE);

	printf("%s \n", szMessage);
}
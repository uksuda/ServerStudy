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
	std::cout << "receive something" << std::endl;
	char* pData = nullptr;
	//receivePacket.getDataFromPacket(pData, CHAT_MESSAGE)
}
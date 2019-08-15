#include "DispatcherFunc.h"
#include "Log.h"

DispatchFunc::DispatchFunc()
{

}

DispatchFunc::~DispatchFunc()
{

}

void DispatchFunc::funcLoginReq(Packet receivePacket)
{
	unsigned int iClientID = 0;
	receivePacket.getDataFromPacket(&iClientID);

	if (iClientID == 0)
	{
		CLog::LOG("invalid Client ID");
		return;
	}


}

void DispatchFunc::funcChatMessage(Packet receivePacket)
{

}

//ID_LOGIN_REQ = 1,
/*
unsigned int : ID
*/

//ID_CHAT_MESSAGE = 2,
/*
char * messageSize : message
*/
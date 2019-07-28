#ifndef _DISPATCHER_FUNC_H__
#define _DISPATCHER_FUNC_H__

#include "ServerHeader.h"

class DispatchFunc
{
private:
	DispatchFunc();
	~DispatchFunc();

public:
	static void funcLoginReq(Packet receivePacket);
	static void funcChatMessage(Packet receivePacket);
};

#endif
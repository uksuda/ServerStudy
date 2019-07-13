#ifndef _DISPATCH_FUNC_H__
#define _DISPATCH_FUNC_H__

#include "ServerHeader.h"

class DispatchFunc
{
private:
	DispatchFunc();
	~DispatchFunc();

public:
	static void funcLoginOK(Packet receivePacket);
};

#endif
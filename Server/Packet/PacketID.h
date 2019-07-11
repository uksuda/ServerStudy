#ifndef __PACKET_ID_H__
#define __PACKET_ID_H__

enum class E_PID_STC
{
	ID_INVALID = 0,
	ID_LOGIN_OK = 1,
	ID_END
};




enum class E_PID_CTS
{
	ID_INVALID = 0,
	ID_LOGIN_OK_REQ = 1,
	ID_END
};

#endif
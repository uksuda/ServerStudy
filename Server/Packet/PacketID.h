#ifndef __PACKET_ID_H__
#define __PACKET_ID_H__

enum class E_PACKET_ID_SEVER_TO_CLIENT
{
	ID_LOGIN_OK = 0,
	ID_END
};

enum class E_PACKET_ID_CLIENT_TO_SERVER
{
	ID_LOGIN_OK_REQ = 0,
	ID_END
};

#endif
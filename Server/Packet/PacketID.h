#ifndef __PACKET_ID_H__
#define __PACKET_ID_H__

enum class E_PID_STC // Server to Client
{
	ID_INVALID = 0,

	ID_LOGIN_OK = 1,

	ID_CHAT_MESSAGE_FROM = 2,
	/*
	unsigned int : chat user id
	char * messageSize : message
	*/

	ID_END
};




enum class E_PID_CTS // Client to Server
{
	ID_INVALID = 0,

	ID_LOGIN_REQ = 1,
	/*
	unsigned int : ID
	*/

	ID_CHAT_MESSAGE = 2,
	/*
	char * messageSize : message 
	*/

	ID_END
};

#endif
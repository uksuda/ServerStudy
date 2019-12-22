#ifndef __PACKET_ID_H__
#define __PACKET_ID_H__

#define INVALID_PACKET_ID 0

enum E_PACKET_SERVER_TO_CLIENT // Server to Client
{
	ID_LOGIN_OK = 1,

	ID_CHAT_MESSAGE_FROM = 2,
	/*
	unsigned int : chat user id
	char * messageSize : message
	*/

	ID_END_SERVER
};




enum E_PACKET_CLIENT_TO_SERVER // Client to Server
{
	ID_LOGIN_REQ = 1,
	/*
	unsigned int : ID
	*/

	ID_CHAT_MESSAGE = 2,
	/*
	char * messageSize : message 
	*/

	ID_END_CLIENT
};

#endif
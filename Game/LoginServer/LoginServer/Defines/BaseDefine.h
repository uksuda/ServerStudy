#pragma once

#include <WinSock2.h>
#include <Windows.h>
#include <process.h>
#include <Ws2tcpip.h>

#include <iostream>
#include <vector>
#include <map>
#include <list>
#include <algorithm>
#include <chrono>
#include <ctime>
#include <string>
#include <sstream>
#include <iomanip>

#pragma comment(lib, "ws2_32.lib")

#define SERVER_PORT 15001
#define SERVER_HOST "127.0.0.1"

#define SAFE_DELETE(p) {if (p) { delete (p); (p) = nullptr; }}
#define SAFE_DELETE_ARRAY(p) {if (p) { delete [] (p); (p) = nullptr; }}

#define DECLARE_SINGLETON(classname)							\
		private:												\
			classname() {}										\
			classname(classname& rhs) = delete;					\
			static classname* m_pInstance;						\
		public:													\
			static classname* getInstance();					\
			static void destroyInstance();

#define IMPLEMENT_SINGLETON(classname)							\
		classname* classname::m_pInstance = nullptr;			\
		classname* classname::getInstance(){					\
			if (m_pInstance == nullptr){						\
				m_pInstance = new classname;}					\
			return m_pInstance;}								\
		void classname::destroyInstance(){						\
			SAFE_DELETE(m_pInstance);}


#ifdef _DEBUG
#define DEBUG_ASSERT(exp) {if (exp == false) __asm int 3}
#else
#define DEBUG_ASSERT(exp) {}
#endif

#define CONVERT_STR(str) #str

#define PACKET_HEADER_SIZE	4 /*2 + 2*/
#define PACKET_BUFFER_SIZE	PACKET_HEADER_SIZE + 508

enum class E_IO_MODE
{
	E_MODE_NONE = 0,
	E_MODE_SEND,
	E_MODE_RECV
};

struct stIOContext : OVERLAPPED
{
	WSABUF m_wsaBuf;
	E_IO_MODE m_eMode;
};

struct stSendIOContext : public stIOContext
{
	BYTE m_btBuffer[PACKET_BUFFER_SIZE];
};



//#define MESSAGE_SIZE 1024
//
//#define SOCKET_OPTION_TRUE 1
//#define SOCKET_OPTION_FALSE 0
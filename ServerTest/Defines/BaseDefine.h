#pragma once

#include <WinSock2.h>
#include <Windows.h>
#include <process.h>
#include <WS2tcpip.h>

#include <random>
#include <future>
#include <thread>
#include <vector>
#include <list>
#include <algorithm>
#include <map>
#include <ctime>
#include <iostream>
#include <string>
#include <sstream>
#include <chrono>
#include <iomanip>
#include <atomic>
#include <queue>
#include <stack>

#pragma comment(lib, "Ws2_32.lib")

#define SERVER_IP "127.0.0.1"
#define SERVER_PORT 15001

#define MAX_BUFFER_SIZE 2048

#define NO_COPY(className)										\
	private:													\
		className(const className&) = delete;					\
		className& operator = (const className&) = delete;

#define DECLARE_SINGLETON(className)							\
	NO_COPY(className)											\
	private:													\
		className();											\
		static className* m_pInstance;							\
	public:														\
		static className* getInstance();						\
		static void destroyInstance();

#define IMPLEMENT_SINGLETON(className)							\
	className* className::m_pInstance = nullptr;				\
	className* className::getInstance(){						\
		if (m_pInstance == nullptr){							\
			m_pInstance = new className;}						\
		return m_pInstance;}									\
	void className::destroyInstance(){							\
		if (m_pInstance){										\
			delete m_pInstance;									\
			m_pInstance = nullptr;}}


#define SAFE_DELETE(pPointer)	{if (pPointer) {delete pPointer; pPointer = nullptr;}}
#define SAFE_DELETE_ARRAY(pArray)	{if (pArray) {delete [] pArray; pArray = nullptr;}}

#define STRING(x) #x


enum E_IO_MODE
{
	E_IO_INVALID = 0,
	E_IO_RECV,
	E_IO_SEND
};

#define ACCOUNT_LENGTH 8
#define PASSWORD_LENGTH 8

//#define PACKET_HEADER_SIZE 6 /*size 2 + packet id 4*/
//#define PACKET_BUFFER_SIZE PACKET_HEADER_SIZE + 506

#include "DataBaseType.h"
#include "Packet.h"
#include "CommonStruct.h"
#include "Utils.h"
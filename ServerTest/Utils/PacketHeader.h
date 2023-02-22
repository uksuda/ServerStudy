#pragma once

#define INVALID_PACKET_ID 0

enum E_PACKET_SERVER_TO_CLIENT : unsigned int
{
	E_LOGIN_ACK = 1,
	// byte 0: success 1: fail
	E_CHARACTER_LIST = 2,
	E_TEST_ACK = 3,
	// byte 2 : test ack
	// 
	// char[10] : test ack
};


enum E_PACKET_CLIENT_TO_SERVER : unsigned int
{
	E_LOGIN_REQ = 1,
	//byte 0: old 1: new
	//char[ACCOUNT_LENGTH] account 8
	//char[PASSWORD_LENGTH] password 8
	E_TEST_REQ = 2,
	// byte 1 : test req
	// char[10] : test req

};

#include <cstdint>
#include <map>
#include <functional>

namespace packet
{
	enum class Type : unsigned short
	{
		None = 0,
		Login = 1,
		Logout = 2,
	};

	struct Header
	{
		std::uint16_t size;
		Type type;
	};

	struct Login
	{
		char id[20];
		std::uint8_t len;
	};
}

class message_handler
{
public:

	message_handler()
	{
		bind(packet::Type::Login, &message_handler::handle_login);
	}

	template < typename PacketType >
	void bind(packet::Type packetType, void(message_handler::*handler)(const PacketType&))
	{
		handlers[packetType] = [this, handler](const void* ptr)
		{
			const PacketType& packet = *((const PacketType*)ptr);
			((*this).*handler)(packet);
		};
	}

	void handle_login(const packet::Login& packet)
	{
	}

private:

	std::map<packet::Type, std::function<void(const void*)>> handlers;
};

//int main()
//{
//	message_handler handler;
//
//	return 0;
//}

//#include <map>
//#include <functional>
//
//namespace packet
//{
//	enum class Type : unsigned short
//	{
//		None = 0,
//		Login = 1,
//		Logout = 2,
//	};
//
//	struct Header
//	{
//		std::uint16_t size;
//		Type type;
//	};
//
//	struct Login
//	{
//		char id[20];
//		std::uint8_t len;
//	};
//}
//
//class message_handler
//{
//public:
//
//	message_handler()
//	{
//		bind(packet::Type::Login, &message_handler::handle_login);
//	}
//
//	template < typename PacketType >
//	void bind(packet::Type packetType, void(message_handler::*handler)(const PacketType&))
//	{
//		handlers[packetType] = [this, handler](const void* ptr)
//		{
//			const PacketType& packet = *((const PacketType*)ptr);
//			((*this).*handler)(packet);
//		};
//	}
//
//	void handle_login(const packet::Login& packet)
//	{
//	}
//
//private:
//
//	std::map<packet::Type, std::function<void(const void*)>> handlers;
//};
//
//int main()
//{
//	message_handler handler;
//
//	return 0;
//}

//std::map<packet::Type, std::function<void(int, void*)>> handlers;
//
//template < typename PayloadType >
//void bind(packet::Type type, void(*handler)(int, const PayloadType&))
//{
//	handlers[type] = [handler](int networkId, void* packet)
//	{
//		const PayloadType& payload = *static_cast<PayloadType*>(packet);
//		handler(networkId, payload);
//	};
//}
//
//void handle_login(int networkId, const packet::Login& login)
//{
//}
//
//int main()
//{
//	bind(packet::Type::Login, handle_login);
//
//	return 0;
//}
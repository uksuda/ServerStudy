#include "MainClient.h"
#include "ClientSocket.h"
#include <iostream>
#include "Log.h"


MainClient::MainClient()
	: m_pSocket(nullptr)
	, m_bRunning(false)
	, m_fAccumulatedTime(0.f)
{

}

MainClient::~MainClient()
{

}

MainClient* MainClient::createMainClient()
{
	MainClient* pMain = new MainClient;
	if (pMain == nullptr || pMain->initialize() == false)
	{
		CLog::LOG("MainClient created failed");
		return nullptr;
	}

	return pMain;
}


void MainClient::stopClient()
{
	m_bRunning = false;
}

bool MainClient::initialize()
{
	m_pSocket = ClientSocket::createSocket();
	if (m_pSocket == nullptr)
	{
		return false;
	}

	if (m_pSocket->connectTo(SERVER_HOST, SERVER_PORT) == false)
	{
		return false;
	}

	unsigned int iTempID = 10;

	Packet sendPacket(PACKET_ENUM(E_PID_CTS::ID_LOGIN_REQ));
	sendPacket.add(iTempID);

	m_pSocket->sendPacket(sendPacket);

	return true;
}

void MainClient::runClient()
{
	if (m_bRunning)
	{
		return;
	}

	m_bRunning = true;

	ULONGLONG lTime = 0;
	
	while (m_bRunning)
	{
		ULONGLONG lInterval = GetTickCount64() - lTime;
		
		lTime = GetTickCount64();
		float fDelta = static_cast<float>(lInterval) * 0.001f;

		update(fDelta);
		render();
	}
}

void MainClient::update(float fDelta)
{
	m_pSocket->receivePacket();
	m_fAccumulatedTime += fDelta;
}

void MainClient::render()
{
	inputMessage();
}

void MainClient::inputMessage()
{
	std::string strMessage;
	std::cout << "input message : " << std::endl;
	std::cin >> strMessage;

	if (strMessage.size() == 0)
	{
		return;
	}

	if (strMessage.size() > MESSAGE_SIZE)
	{
		std::cout << "too long message." << std::endl;
		return;
	}

	Packet sendPacket(PACKET_ENUM(E_PID_CTS::ID_CHAT_MESSAGE));
	sendPacket.add((char*)strMessage.c_str(), strMessage.size());
	m_pSocket->sendPacket(sendPacket);
}

void MainClient::release()
{
	SAFE_DELETE(m_pSocket);
}
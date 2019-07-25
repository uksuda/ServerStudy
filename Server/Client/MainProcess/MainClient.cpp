#include "MainClient.h"
#include "ClientSocket.h"
#include "MainScene.h"
#include "Log.h"


MainClient::MainClient()
	: m_pSocket(nullptr)
	, m_pMainScene(nullptr)
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
	return true;
}

void MainClient::runClient()
{
	if (m_bRunning)
	{
		return;
	}

	m_bRunning = true;

	DWORD dwTime = 0;
	DWORD dwOneFrameTime = 167;

	float fOneFrameTime = 0.0167f;
	float fDelta = 0.f;
	
	while (m_bRunning)
	{
		DWORD dwInterval = GetTickCount() - dwTime;
		if (dwInterval < dwOneFrameTime)
		{
			continue;
		}

		dwTime = GetTickCount();
		fDelta += fOneFrameTime;

		update(fDelta);
		render();
	}
}

void MainClient::update(float fDelta)
{
	m_fAccumulatedTime += fDelta;

}

void MainClient::render()
{

}

void MainClient::release()
{
	SAFE_DELETE(m_pMainScene);
	SAFE_DELETE(m_pSocket);
}
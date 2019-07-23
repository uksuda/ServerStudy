#include "MainScene.h"
#include <iostream>

MainScene::MainScene()
{
	memset(m_szMessage, 0, sizeof(m_szMessage));
}

MainScene::~MainScene()
{
	release();
}

MainScene* MainScene::createMainScene()
{
	MainScene* pScene = new MainScene;
	if (pScene && pScene->initScene())
	{
		return pScene;
	}

	return nullptr;
}

bool MainScene::initScene()
{
	std::cout << "Client Start" << std::endl;
	return true;
}

void MainScene::inputMessage()
{
	std::string strMessage;

	std::cout << "input message : " << std::endl;
	std::cin >> strMessage;

	if (strMessage.size() == 0 || strMessage.size() > MESSAGE_LENGTH)
	{
		std::cout << "too long message. input again" << std::endl;
		return;
	}

	memcpy(m_szMessage, strMessage.c_str(), sizeof(strMessage.size()));
}

void MainScene::release()
{

}
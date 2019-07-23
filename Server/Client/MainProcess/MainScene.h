#ifndef _MAIN_SCENE_H__
#define _MAIN_SCENE_H__

#include "ServerHeader.h"

#define MESSAGE_LENGTH 512

class MainScene
{
private:
	MainScene();

public:
	~MainScene();

public:
	static MainScene* createMainScene();

	bool initScene();
	void inputMessage();

private:
	char m_szMessage[MESSAGE_LENGTH];

public:
	void release();
};


#endif
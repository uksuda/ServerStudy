#include <iostream>
#include "MainClient.h"

int main()
{
	MainClient* pMain = MainClient::createMainClient();
	if (pMain == nullptr)
	{
		return -1;
	}

	pMain->runClient();
	return 0;
}
#include "BaseDefine.h"
#include "ClientSocket.h"
#include "Log.h"


int main(int argc, char** argv)
{
	ClientSocket* pSocket = new ClientSocket;
	if (pSocket == nullptr || pSocket->initializeClientSocket() == false)
	{
		CLog::log("fail client socket");
		return 0;
	}

	pSocket->connectToServer();

	int iInterval = 0;
	unsigned short wPacketId = 0;
	unsigned char btTest = 0;
	unsigned short wTest = 0;
	unsigned int iTest = 0;

	char szTest[10];
	memset(szTest, 0, sizeof(szTest));
	_snprintf(szTest, sizeof(szTest), "1234567890");
	//while (true)
	{
		iInterval = Utils::getRandomValue<int>(100, 10000);
		wPacketId = Utils::getRandomValue<int>(1, 100);
		unsigned char btTest = Utils::getRandomValue<int>(1, 200);
		unsigned short wTest = Utils::getRandomValue<int>(1, 60000);
		unsigned int iTest = Utils::getRandomValue<int>(1, 100000000);

		Sleep(iInterval);

		Packet sendPacket;
		sendPacket.setPacketID(wPacketId);
		sendPacket.addPacket(btTest);
		sendPacket.addPacket(wTest);
		sendPacket.addPacket(iTest);
		sendPacket.addPacket(szTest, sizeof(szTest));
		sendPacket.addPacketEnd();

		pSocket->sendPacket(sendPacket);
	}

	Sleep(500000);

	return 0;
}
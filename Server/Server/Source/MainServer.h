#ifndef __MAIN_SERVER_H__
#define __MAIN_SERVER_H__

class ServerSocket;
class MainServer
{
private:
	explicit MainServer();
public:
	~MainServer();

public:
	void runServer();
	void updateServer(float fDelta);

private:
	ServerSocket* m_pServerSocket;

private:
	bool initMainServer();

public:
	static MainServer* createMainServer();
};

#endif
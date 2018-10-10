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

private:
	// ServerSocket
	// WorkerThread

private:
	bool initMainServer();

public:
	static MainServer* createMainServer();
};

#endif
#ifndef __DB_CONNECTOR_H__
#define __DB_CONNECTOR_H__

#include "ServerHeader.h"

#include <queue>

class DBConnector
{
	DECLARE_SINGLETON(DBConnector);
public:
	~DBConnector();
public:
	bool initDBConnector();

private:
	//std::queue<>

public:
	void release();
};

#endif
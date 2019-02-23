#include "DBConnector.h"

IMPLEMENT_SINGLETON(DBConnector);

DBConnector::~DBConnector()
{

}

bool DBConnector::initDBConnector()
{
	return true;
}

void DBConnector::release()
{

}
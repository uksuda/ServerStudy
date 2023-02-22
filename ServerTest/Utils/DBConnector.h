#pragma once

#include "ThreadBase.h"
#include <sql.h>
#include <sqlext.h>

#define SQL_STATE_SIZE SQL_SQLSTATE_SIZE + 1
#define SQL_ERROR_MSG_SIZE SQL_MAX_MESSAGE_LENGTH + 1

#define DB_CONNECTOR_MGR DBConnector::getInstance()

class DBConnector : public ThreadBase
{
DECLARE_SINGLETON(DBConnector);

public:
	virtual ~DBConnector();

	enum E_ERROR_HANDLE_TYPE
	{
		E_TYPE_INVALID = 0,
		E_TYPE_ENV,
		E_TYPE_DBC,
		E_TYPE_STMT,
		E_TYPE_DESC
	};

public:
	bool initializeDBConnector();
	bool DBConnect();
	void DBDisconnect();

	stQueryData* getQuery();
	void insertQuery(E_QUERY_EXEC_TYPE eQueryType, ClientSession* pSession, std::string strQuery);
	bool startDBThread();

	void stopRunning();
	virtual void run();
	
private:
	bool m_isRunning;

	SQLHENV m_hEnv;
	SQLHDBC m_hDbc;
	SQLHSTMT m_hStmt;

	std::queue<stQueryData*> m_queueQuery;

	CRITICAL_SECTION m_CriticalSection;

private:
	void sqlErrorLog(E_ERROR_HANDLE_TYPE eType);
	
public:
	void release();
};
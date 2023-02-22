#include "DBConnector.h"
#include "DBFunction.h"
#include "Log.h"

IMPLEMENT_SINGLETON(DBConnector);

#define ODBC_NAME L"LoginDB"
#define DB_ACCOUNT_ID L"loginac"
#define DB_ACCOUNT_PASSWORD L"rpdlafhrmdls!20"

#define DB_QUERRY_LENGTH 1024

DBConnector::DBConnector()
	: m_isRunning(false)
	, m_hEnv(nullptr)
	, m_hDbc(nullptr)
	, m_hStmt(nullptr)
{
	InitializeCriticalSection(&m_CriticalSection);
}

DBConnector::~DBConnector()
{
	release();
}

bool DBConnector::initializeDBConnector()
{
	if (SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &m_hEnv) != SQL_SUCCESS)
	{
		CLog::log("%s - fail : SQLAllocHandle HANDLE_ENV");
		sqlErrorLog(E_ERROR_HANDLE_TYPE::E_TYPE_ENV);
		return false;
	}
	
	if (SQLSetEnvAttr(m_hEnv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER)SQL_OV_ODBC3, SQL_IS_INTEGER) != SQL_SUCCESS)
	{
		CLog::log("%s - fail : SQLSetEnvAttr");
		sqlErrorLog(E_ERROR_HANDLE_TYPE::E_TYPE_ENV);
		return false;
	}

	if (SQLAllocHandle(SQL_HANDLE_DBC, m_hEnv, &m_hDbc) != SQL_SUCCESS)
	{
		CLog::log("%s - fail : SQLAllocHandle HANDLE DBC");
		sqlErrorLog(E_ERROR_HANDLE_TYPE::E_TYPE_DBC);
		return false;
	}

	CLog::log("DB init Success");
	return true;
}

bool DBConnector::DBConnect()
{
	SQLRETURN result = SQLConnect(m_hDbc, (SQLWCHAR*)ODBC_NAME, SQL_NTS, (SQLWCHAR*)DB_ACCOUNT_ID, SQL_NTS, (SQLWCHAR*)DB_ACCOUNT_PASSWORD, SQL_NTS);
	if (result != SQL_SUCCESS && result != SQL_SUCCESS_WITH_INFO)
	{
		CLog::log("%s - SQLConnect fail", __FUNCTION__);
		sqlErrorLog(E_ERROR_HANDLE_TYPE::E_TYPE_DBC);
		return false;
	}

	if (SQLAllocHandle(SQL_HANDLE_STMT, m_hDbc, &m_hStmt) != SQL_SUCCESS)
	{
		CLog::log("%s - SQLAllocHandle fail", __FUNCTION__);
		sqlErrorLog(E_ERROR_HANDLE_TYPE::E_TYPE_DBC);
		return false;
	}

	return true;
}

void DBConnector::DBDisconnect()
{
	if (m_hStmt)
	{
		SQLFreeHandle(SQL_HANDLE_STMT, m_hStmt);
	}

	if (m_hDbc)
	{
		SQLDisconnect(m_hDbc);
		SQLFreeHandle(SQL_HANDLE_DBC, m_hDbc);
		SQLFreeHandle(SQL_HANDLE_ENV, m_hEnv);
	}
}

stQueryData* DBConnector::getQuery()
{
	if (m_queueQuery.empty() == true)
	{
		return nullptr;
	}

	stQueryData* pQuery = nullptr;
	EnterCriticalSection(&m_CriticalSection);
	pQuery = m_queueQuery.front();
	m_queueQuery.pop();
	LeaveCriticalSection(&m_CriticalSection);
	return pQuery;
}

void DBConnector::insertQuery(E_QUERY_EXEC_TYPE eQueryType, ClientSession* pSession, std::string strQuery)
{
	if (eQueryType == E_QUERY_EXEC_TYPE::E_QUERY_INVALID || eQueryType > E_QUERY_EXEC_TYPE::E_QUERY_END)
	{
		CLog::log("%s -- invalid querry type %d", __FUNCTION__, eQueryType);
		return;
	}

	if (pSession == nullptr || strQuery.empty() == true)
	{
		CLog::log("%s -- invalid argument", __FUNCTION__);
		return;
	}

	EnterCriticalSection(&m_CriticalSection);

	stQueryData* pQuery = new stQueryData;
	memset(pQuery, 0, sizeof(stQueryData));
	
	pQuery->m_eQueryType = eQueryType;
	pQuery->m_pQuerySession = pSession;
	pQuery->m_strQuery = strQuery;

	m_queueQuery.emplace(pQuery);

	LeaveCriticalSection(&m_CriticalSection);
}

bool DBConnector::startDBThread()
{
	if (m_isRunning == true)
	{
		return false;
	}

	if (DBConnect() == false)
	{
		return false;
	}

	if (ThreadBase::startThread() == false)
	{
		return false;
	}

	m_isRunning = true;
	return true;
}

void DBConnector::stopRunning()
{
	if (m_isRunning == false)
	{
		return;
	}

	m_isRunning = false;

	HANDLE threadHandle = getThreadHandle();
	if (threadHandle != INVALID_HANDLE_VALUE)
	{
		WaitForSingleObject(threadHandle, INFINITE);
	}
}

void DBConnector::run()
{
	
	//char szTemp[128];
	//memset(szTemp, 0, sizeof(szTemp));

	//SQLWCHAR wszQuery[128] = { 0, };
	//memset(wszQuery, 0, sizeof(wszQuery));
	//auto bb = sizeof(wszQuery);

	////_snprintf(szTemp, sizeof(szTemp), "insert into char_quest (char_serial, quest_id, state_value, dated) values (%d, %d, %d, GETDATE())", 25, 2, 2);
	////MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, szTemp, sizeof(szTemp), wszQuery, sizeof(wszQuery));
	////
	//////wsprintf(wszQuery, L"insert into char_quest (char_serial, quest_id, state_value, dated) values (%d, %d, %d, GETDATE())", 25, 2, 2);
	////result = SQLExecDirect(m_hStmt, wszQuery, SQL_NTS);
	////if (result != SQL_SUCCESS)
	////{
	////	int a = 0;
	////}
	////
	////memset(szTemp, 0, sizeof(szTemp));
	////memset(wszQuery, 0, sizeof(wszQuery));

	//_snprintf(szTemp, sizeof(szTemp), "select * from char_quest");
	//MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, szTemp, sizeof(szTemp), wszQuery, sizeof(wszQuery));

	//UINT iCharSerial = 0;
	//UINT iQuestID = 0;
	//UINT iSateValue = 0;
	//SYSTEMTIME systemTime;
	//memset(&systemTime, 0, sizeof(SYSTEMTIME));

	//SQLINTEGER iNum1 = 0;
	//SQLINTEGER iNum2 = 0;
	//SQLINTEGER iNum3 = 0;
	//SQLINTEGER iNum4 = 0;

	///*SQLBindCol(m_hStmt, 1, SQL_C_LONG, &iCharSerial, sizeof(iCharSerial), &iNum1);
	//SQLBindCol(m_hStmt, 2, SQL_C_LONG, &iQuestID, sizeof(iQuestID), &iNum2);
	//SQLBindCol(m_hStmt, 3, SQL_C_LONG, &iSateValue, sizeof(iSateValue), &iNum3);
	//SQLBindCol(m_hStmt, 4, SQL_C_TYPE_TIMESTAMP, &systemTime, sizeof(systemTime), &iNum4);*/

	//result = SQLExecDirect(m_hStmt, wszQuery, SQL_NTS);

	//while (SQLFetch(m_hStmt) != SQL_NO_DATA)
	//{
	//	SQLGetData(m_hStmt, 1, SQL_C_LONG, &iCharSerial, 0, nullptr);
	//	SQLGetData(m_hStmt, 2, SQL_C_LONG, &iQuestID, 0, nullptr);
	//	SQLGetData(m_hStmt, 3, SQL_C_LONG, &iSateValue, 0, nullptr);
	//	SQLGetData(m_hStmt, 4, SQL_C_TYPE_TIMESTAMP, &systemTime, 0, nullptr);
	//	std::cout << iCharSerial << std::endl;
	//	std::cout << iQuestID << std::endl;
	//	std::cout << systemTime.wYear << " " << systemTime.wMonth << " " << systemTime.wDay << std::endl;
	//}

	std::wstring wstrQuery;
	stQueryData* pQuery = nullptr;
	SQLRETURN sqlResult = SQL_SUCCESS;
	while (m_isRunning)
	{
		wstrQuery.clear();
		sqlResult = SQL_SUCCESS;
		pQuery = nullptr;

		pQuery = getQuery();
		if (pQuery == nullptr)
		{
			continue;
		}

		wstrQuery = Utils::convertToWstring(pQuery->m_strQuery);
		sqlResult = SQLExecDirect(m_hStmt, (SQLWCHAR*)wstrQuery.c_str(), SQL_NTS);
		if (sqlResult != SQL_SUCCESS)
		{
			CLog::log("%s -- %s", __FUNCTION__, wstrQuery.c_str());
			sqlErrorLog(E_ERROR_HANDLE_TYPE::E_TYPE_STMT);
			continue;
		}

		if (pQuery->m_eQueryType != E_QUERY_EXEC_TYPE::E_QUERY_DIRECT)
		{
			DB_RESULT_FUNCTION->onQueryResult(pQuery, m_hStmt);
		}

		SAFE_DELETE(pQuery);
		Sleep(50);
	}
}

void DBConnector::sqlErrorLog(E_ERROR_HANDLE_TYPE eType)
{
	//SQLGetDiagRec(SQL_HANDLE_DBC, m_hDbc, 1, sqlState, nullptr, sqlMessage, sizeof(sqlMessage), nullptr);
	//SQLGetDiagField

	SQLSMALLINT handleType = 0;
	SQLHANDLE hSqlHandle = nullptr;

	SQLSMALLINT recordNumber = 0;
	SQLSMALLINT sqlMessageLength = 0;
	SQLRETURN sqlResult = 0;
	SQLINTEGER sqlNativeError = 0;
	
	SQLWCHAR sqlState[SQL_STATE_SIZE];
	memset(sqlState, 0, sizeof(sqlState));
	SQLWCHAR sqlMessage[SQL_ERROR_MSG_SIZE];
	memset(sqlMessage, 0, sizeof(sqlMessage));
	
	switch (eType)
	{
	case E_ERROR_HANDLE_TYPE::E_TYPE_ENV:
		handleType = SQL_HANDLE_ENV;
		hSqlHandle = m_hEnv;
		break;
	case E_ERROR_HANDLE_TYPE::E_TYPE_DBC:
		handleType = SQL_HANDLE_DBC;
		hSqlHandle = m_hDbc;
		break;
	case E_ERROR_HANDLE_TYPE::E_TYPE_STMT:
		handleType = SQL_HANDLE_STMT;
		hSqlHandle = m_hStmt;
		break;
	case E_ERROR_HANDLE_TYPE::E_TYPE_DESC:
		return;
	default:
		CLog::log("%s - invalid handle type %d", __FUNCTION__, eType);
		return;
	}
	
	WCHAR wcharTempLog[SQL_ERROR_MSG_SIZE * 2];
	memset(wcharTempLog, 0, sizeof(wcharTempLog));

	while (sqlResult != SQL_NO_DATA)
	{
		++recordNumber;
		sqlResult = SQLGetDiagRec(handleType, hSqlHandle, recordNumber, sqlState, &sqlNativeError, sqlMessage, sizeof(sqlMessage), &sqlMessageLength);
		_snwprintf(wcharTempLog, sizeof(wcharTempLog), L"DB : handle %d state[%s] code[%d] - %s", handleType, sqlState, sqlNativeError, sqlMessage);

		std::string strDBLog = Utils::convertToString(std::wstring(wcharTempLog));
		CLog::log(strDBLog.c_str());

		memset(wcharTempLog, 0, sizeof(wcharTempLog));
	}
}

void DBConnector::release()
{
	DBDisconnect();

	/*while (m_queueQuery.empty() != true)
	{
		stQueryData* pQueryData = m_queueQuery.front();
		SAFE_DELETE(pQueryData);
		m_queueQuery.pop();
	}*/

	std::queue<stQueryData*> emptyQueue;
	std::swap(m_queueQuery, emptyQueue);

	DeleteCriticalSection(&m_CriticalSection);
}
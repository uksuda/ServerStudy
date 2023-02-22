#include "DBFunction.h"
#include "Log.h"
#include <sql.h>
#include <sqlext.h>

IMPLEMENT_SINGLETON(DBFunction);

DBFunction::DBFunction()
{

}

DBFunction::~DBFunction()
{
	release();
}

bool DBFunction::initializeDBFunction()
{
	m_mapQueryFunction.insert(MAP_QUERY::value_type(E_QUERY_EXEC_TYPE::E_QUERY_SELECT, &DBFunction::testFunction));

	return true;
}

bool DBFunction::onQueryResult(E_QUERY_EXEC_TYPE eQueryType, ClientSession* pSession, void* hSqlStmt)
{
	if (eQueryType < E_QUERY_EXEC_TYPE::E_QUERY_SELECT || eQueryType >= E_QUERY_EXEC_TYPE::E_QUERY_END)
	{
		return false;
	}

	if (pSession == nullptr || hSqlStmt == nullptr)
	{
		return false;
	}

	QUERY_FUNC_ITER iter = m_mapQueryFunction.find(eQueryType);
	if (iter == m_mapQueryFunction.end())
	{
		return false;
	}

	return iter->second(pSession, hSqlStmt);
}

bool DBFunction::onQueryResult(stQueryData* pQueryData, void* hSqlStmt)
{
	if (pQueryData == nullptr || hSqlStmt == nullptr)
	{
		return false;
	}

	if (pQueryData->m_pQuerySession == nullptr)
	{
		return false;
	}

	return onQueryResult(pQueryData->m_eQueryType, pQueryData->m_pQuerySession, hSqlStmt);
}

bool DBFunction::testFunction(ClientSession* pSession, void* pArg)
{
	if (pSession == nullptr || pArg == nullptr)
	{
		return false;
	}

	SQLHSTMT hStmt = static_cast<SQLHSTMT>(pArg);

	printf("testFunction called\n");
	return true;
}

void DBFunction::release()
{
	m_mapQueryFunction.clear();
}


#pragma once

#include "BaseDefine.h"

#define DB_RESULT_FUNCTION DBFunction::getInstance()

class DBFunction
{
DECLARE_SINGLETON(DBFunction);

public:
	virtual ~DBFunction();

public:
	typedef std::function<bool(ClientSession*, void*)> QUERY_FUNCTION;
	typedef std::map<WORD, QUERY_FUNCTION> MAP_QUERY;
	typedef std::map<WORD, QUERY_FUNCTION>::iterator QUERY_FUNC_ITER;

public:
	bool initializeDBFunction();
	bool onQueryResult(E_QUERY_EXEC_TYPE eQueryType, ClientSession* pSession, void* hSqlStmt);
	bool onQueryResult(stQueryData* pQueryData, void* hSqlStmt);

public:
	static bool testFunction(ClientSession* pSession, void* pArg = nullptr);

private:
	MAP_QUERY m_mapQueryFunction;

public:
	void release();
};
#pragma once

enum E_QUERY_EXEC_TYPE
{
	E_QUERY_INVALID = 0,
	E_QUERY_DIRECT,			// insert, delete
	E_QUERY_SELECT,			// select
	E_QUERY_END
};
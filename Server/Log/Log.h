#ifndef __LOG_H__
#define __LOG_H__

class CLog
{
public:
	explicit CLog();
	~CLog();

public:
	static void LOG(int iErrorNo);
	static void LOG(const char* szMsg);
	static void LOG(const char* szErrorName, int iErrorNo);
};

#endif

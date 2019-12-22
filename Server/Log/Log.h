#ifndef __LOG_H__
#define __LOG_H__

class CLog
{
public:
	explicit CLog();
	~CLog();

public:
	static void LOG(const char* szMsg, ...);
};

#endif

#pragma once

#include "BaseDefine.h"

class ThreadBase
{
protected:
	ThreadBase();
	virtual ~ThreadBase();

private:
	HANDLE m_ThreadHandle;

public:
	void release();
};
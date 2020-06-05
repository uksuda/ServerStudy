#pragma once

class ThreadBase
{
protected:
	ThreadBase();
	virtual ~ThreadBase();

public:
	void release();
};
#ifndef __SYNCHRO_H__
#define __SYNCHRO_H__

#include "ServerHeader.h"

#include <vector>

#define SYNCHRO Synchro::getInstance()

class Synchro
{
DECLARE_SINGLETON(Synchro);
public:
	~Synchro();

public:
	enum class SYNC_TARGET
	{
		SYNC_SESSION = 0,
		SYNC_DB,
		SYNC_LOGIC,
		SYNC_END
	};

	typedef std::vector<CRITICAL_SECTION*> VEC_CS;
	//using VEC_CS = std::vector<CRITICAL_SECTION*>;

public:
	void enterCriticalSection(SYNC_TARGET eTarget);
	void leaveCriticalSection(SYNC_TARGET eTarget);
	bool initSynchro();

private:
	VEC_CS m_vecCS;

private:
	bool checkCriticalSectionValue(SYNC_TARGET eTarget);
	bool checkCriticalSectionValue(int iTarget);

public:
	void release();
};

#endif
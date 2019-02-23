#include "Synchro.h"
#include <algorithm>

IMPLEMENT_SINGLETON(Synchro);

Synchro::~Synchro()
{
	release();
}

void Synchro::enterCriticalSection(SYNC_TARGET eTarget)
{
	int iTarget = static_cast<int>(eTarget);
	if (checkCriticalSectionValue(iTarget) == false)
		return;

	if (m_vecCS[iTarget])
	{
		EnterCriticalSection(m_vecCS[iTarget]);
	}
}

void Synchro::leaveCriticalSection(SYNC_TARGET eTarget)
{
	int iTarget = static_cast<int>(eTarget);
	if (checkCriticalSectionValue(iTarget) == false)
		return;

	if (m_vecCS[iTarget])
	{
		LeaveCriticalSection(m_vecCS[iTarget]);
	}
}

bool Synchro::initSynchro()
{
	int iVecSize = static_cast<int>(SYNC_TARGET::SYNC_END);
	m_vecCS.reserve(iVecSize);

	for (int i = 0; i < iVecSize; ++i)
	{
		CRITICAL_SECTION* pCS = new (std::nothrow) CRITICAL_SECTION;
		InitializeCriticalSection(pCS);
		m_vecCS.push_back(pCS);
	}

	return true;
}

bool Synchro::checkCriticalSectionValue(SYNC_TARGET eTarget)
{
	int iTarget = static_cast<int>(eTarget);
	if (iTarget < 0 || iTarget >= static_cast<int>(SYNC_TARGET::SYNC_END))
	{
		return false;
	}

	return true;
}

bool Synchro::checkCriticalSectionValue(int iTarget)
{
	if (iTarget < 0 || iTarget >= static_cast<int>(SYNC_TARGET::SYNC_END))
	{
		return false;
	}

	return true;
}

void Synchro::release()
{
	std::for_each(m_vecCS.begin(), m_vecCS.end(), [&](CRITICAL_SECTION* pCS) {
		if (pCS)
		{
			DeleteCriticalSection(pCS);
		}
	});

	m_vecCS.clear();
}
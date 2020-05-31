#pragma once

#include <string>

class Utils
{
private:
	Utils() = delete;
	~Utils() {}

public:
	std::string getCurrentTime(bool isOnlyDate = false);
	std::string getLogFileName();
};
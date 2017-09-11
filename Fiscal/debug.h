#ifndef DEBUG_H
#define DEBUG_H

#include <string>
#include "Excepcion.h"

namespace Debug
{
	void Debug(const char *Module, const char *Format, ...);
	void Debug(const std::string &Module, const std::string &Info);
	void Debug(const std::string &Module, const Excepcion &e);
	void Dump(const std::string &Module, const char *Information, unsigned Length);
}


#endif

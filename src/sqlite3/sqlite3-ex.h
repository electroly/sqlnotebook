#pragma once
#include <stdint.h>
#include "../../ext/sqlite/sqlite3.h"

extern "C" {
    int SxGetToken(const unsigned char* z, int* tokenType);
}

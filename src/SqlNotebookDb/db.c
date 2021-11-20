#ifdef __cplusplus
#error This file should be compiled as C.
#endif

#define SQLITE_ENABLE_FTS5 1
#define SQLITE_ENABLE_API_ARMOR 1
#define SQLITE_SOUNDEX 1
#define SQLITE_ENABLE_JSON1 1
#define SQLITE_ENABLE_DBSTAT_VTAB 1
#define SQLITE_DEFAULT_FOREIGN_KEYS 1
#define SQLITE_THREADSAFE 2
#define SQLITE_OMIT_TCL_VARIABLE 1
#ifdef _DEBUG
#define SQLITE_MEMDEBUG 1
#endif
#define SQLITE_API __declspec(dllexport)
#include "../../ext/sqlite/sqlite3.c"

__declspec(dllexport) int SxGetToken(const unsigned char* z, int* tokenType) {
    return sqlite3GetToken(z, tokenType);
}

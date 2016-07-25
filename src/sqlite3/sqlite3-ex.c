// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
#include "../../ext/sqlite/sqlite3.c"

int SxGetToken(const unsigned char* z, int* tokenType) {
    return sqlite3GetToken(z, tokenType);
}

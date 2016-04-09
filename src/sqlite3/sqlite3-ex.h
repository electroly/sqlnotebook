#pragma once
#include <stdint.h>
#include "../../ext/sqlite/sqlite3.h"

extern "C" {
    struct yyParser* SxParserAlloc(void);
    struct Parse* SxParseAlloc(sqlite3* db, const char* str);
    void SxParserFree(sqlite3* db, struct yyParser* parser, struct Parse* parse);
    void SxAdvanceParse(struct yyParser* parser, int tokenType, const char* tokenPtr, int tokenLen, struct Parse* parse);
    int SxGetParseErrorCount(struct Parse* parse);
    char* SxGetParseErrorMessage(struct Parse* parse);
    void SxParseErrorMessageFree(char* msg);
    int SxGetToken(const unsigned char* z, int* tokenType);
}

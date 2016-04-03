#ifdef __cplusplus
#error This file should be compiled as C.
#endif

#include "../../ext/sqlite/sqlite3.c"

struct yyParser* SxParserAlloc() {
    return sqlite3ParserAlloc(sqlite3Malloc);
}

void SxParserFree(sqlite3* db, struct yyParser* pEngine, struct Parse* pParse) {
    /* adapted from sqlite3.c */
    sqlite3ParserFree(pEngine, sqlite3_free);
    if (pParse->pVdbe && pParse->nErr>0 && pParse->nested == 0) {
        sqlite3VdbeDelete(pParse->pVdbe);
        pParse->pVdbe = 0;
    }
#ifndef SQLITE_OMIT_SHARED_CACHE
    if (pParse->nested == 0) {
        sqlite3DbFree(db, pParse->aTableLock);
        pParse->aTableLock = 0;
        pParse->nTableLock = 0;
    }
#endif
#ifndef SQLITE_OMIT_VIRTUALTABLE
    sqlite3_free(pParse->apVtabLock);
#endif
    sqlite3DeleteTable(db, pParse->pNewTable);
    sqlite3WithDelete(db, pParse->pWithToFree);
    sqlite3DeleteTrigger(db, pParse->pNewTrigger);
    for (int i = pParse->nzVar - 1; i >= 0; i--) {
        sqlite3DbFree(db, pParse->azVar[i]);
    }
    sqlite3DbFree(db, pParse->azVar);
    while (pParse->pAinc) {
        AutoincInfo *p = pParse->pAinc;
        pParse->pAinc = p->pNext;
        sqlite3DbFree(db, p);
    }
    while (pParse->pZombieTab) {
        Table *p = pParse->pZombieTab;
        pParse->pZombieTab = p->pNextZombie;
        sqlite3DeleteTable(db, p);
    }
    free(pParse);
}

struct Parse* SxParseAlloc(sqlite3* db, const char* str) {
    Parse *parse = calloc(1, sizeof(Parse));
    parse->db = db;
    parse->nQueryLoop = 1;
    parse->rc = SQLITE_OK;
    parse->zTail = str;
    return parse;
}

void SxAdvanceParse(struct yyParser* parser, int tokenType, const char* tokenPtr, int tokenLen, struct Parse* parse) {
    Token token;
    token.z = tokenPtr;
    token.n = tokenLen;
    sqlite3Parser(parser, tokenType, token, parse);
}

int SxGetParseErrorCount(struct Parse* parse) {
    return parse->nErr;
}

char* SxGetParseErrorMessage(struct Parse* parse) {
    if (parse->zErrMsg) {
        return _strdup(parse->zErrMsg);
    } else {
        return _strdup("");
    }
}

void SxParseErrorMessageFree(char* msg) {
    free(msg);
}

int SxGetToken(const unsigned char* z, int* tokenType) {
    return sqlite3GetToken(z, tokenType);
}

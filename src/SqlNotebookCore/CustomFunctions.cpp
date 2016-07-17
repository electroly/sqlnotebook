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

#include <msclr/auto_handle.h>
#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;
using namespace System::IO;
using namespace System::IO::Compression;
using namespace System::Text;
using namespace msclr;
using namespace Newtonsoft::Json;

static void DeleteGcrootNotebook(void* ptr) {
    delete (gcroot<Notebook^>*)ptr;
}

void Notebook::RegisterCustomFunction(const char* functionName, int numArgs,
void(*func)(sqlite3_context*, int, sqlite3_value **)) {
    SqliteCall(sqlite3_create_function_v2(
        /* db */ _sqlite,
        /* zFunctionName */ functionName,
        /* nArg */ numArgs,
        /* eTextRep */ SQLITE_UTF16,
        /* pApp */ new gcroot<Notebook^>(this),
        /* xFunc */ func,
        /* xStep */ nullptr,
        /* xFinal */ nullptr,
        /* xDestroy */ DeleteGcrootNotebook
        ));
}

static Notebook^ GetNotebook(sqlite3_context* ctx) {
    return *(gcroot<Notebook^>*)sqlite3_user_data(ctx);
}

static long GetIntArg(sqlite3_value* arg, String^ paramName, String^ functionName) {
    if (sqlite3_value_type(arg) == SQLITE_INTEGER) {
        return sqlite3_value_int64(arg);
    } else {
        throw gcnew Exception(String::Format(
            "{0}: The \"{1}\" argument must be an INTEGER value.", functionName, paramName));
    }
}

static String^ GetStrArg(sqlite3_value* arg, String^ paramName, String^ functionName) {
    if (sqlite3_value_type(arg) == SQLITE_TEXT) {
        return Util::Str((const wchar_t*)sqlite3_value_text16(arg));
    } else {
        throw gcnew Exception(String::Format(
            "{0}: The \"{1}\" argument must be a TEXT value.", functionName, paramName));
    }
}

static void SqliteResultError(sqlite3_context* ctx, String^ message) {
    auto messageWstr = Util::WStr(message);
    sqlite3_result_error16(ctx, messageWstr.c_str(), -1);
}

static void ErrorNumberFunc(sqlite3_context* ctx, int, sqlite3_value**) {
    auto notebook = GetNotebook(ctx);
    Notebook::SqliteResult(ctx, notebook->UserData->LastError->ErrorNumber);
}

static void ErrorMessageFunc(sqlite3_context* ctx, int, sqlite3_value**) {
    auto notebook = GetNotebook(ctx);
    Notebook::SqliteResult(ctx, notebook->UserData->LastError->ErrorMessage);
}

static void ErrorStateFunc(sqlite3_context* ctx, int, sqlite3_value**) {
    auto notebook = GetNotebook(ctx);
    Notebook::SqliteResult(ctx, notebook->UserData->LastError->ErrorState);
}

static void ReadFileFunc(sqlite3_context* ctx, int argc, sqlite3_value** args) {
    if (argc < 1 || argc > 2) {
        SqliteResultError(ctx, "READ_FILE: Between 1 and 2 arguments are required.");
        return;
    }
    auto notebook = GetNotebook(ctx);
    auto filePathStr = GetStrArg(args[0], "file-path", "READ_FILE");
    auto encodingNum = argc < 2 ? 0 : GetIntArg(args[1], "file-encoding", "READ_FILE");

    if (encodingNum < 0 || encodingNum > 65535) {
        SqliteResultError(ctx, "READ_FILE: The \"file-encoding\" argument must be between 0 and 65535.");
        return;
    }

    String^ contents;
    try {
        if (encodingNum == 0) {
            contents = File::ReadAllText(filePathStr);
        } else {
            auto encoding = Encoding::GetEncoding(encodingNum);
            contents = File::ReadAllText(filePathStr, encoding);
        }
    } catch (Exception^ ex) {
        SqliteResultError(ctx, String::Format("READ_FILE: {0}", ex->Message));
        return;
    }
    Notebook::SqliteResult(ctx, contents);
}

static void SplitFunc(sqlite3_context* ctx, int argc, sqlite3_value** args) {
    auto text = GetStrArg(args[0], "text", "SPLIT");
    auto separator = GetStrArg(args[1], "separator", "SPLIT");
    auto whichSubstring = GetIntArg(args[2], "which-substring", "SPLIT");

    text += separator; // so we don't have to handle the last unterminated substring in a special way

    if (separator->Length == 0) {
        SqliteResultError(ctx, "SPLIT: The argument \"separator\" must not be an empty string.");
        return;
    }

    if (whichSubstring < 0) {
        SqliteResultError(ctx, "SPLIT: The argument \"which-substring\" must be at least 0.");
        return;
    }

    int justAfterLastSeparatorIndex = 0; // the character after the separator
    int substringCount = 0;
    while (true) {
        auto startOfNextSeparatorIndex = text->IndexOf(separator, justAfterLastSeparatorIndex);
        if (startOfNextSeparatorIndex == -1) {
            Notebook::SqliteResult(ctx, DBNull::Value);
            return;
        }

        substringCount++;

        if (substringCount == whichSubstring + 1) {
            auto substring = text->Substring(justAfterLastSeparatorIndex,
                startOfNextSeparatorIndex - justAfterLastSeparatorIndex);
            Notebook::SqliteResult(ctx, substring);
            return;
        }

        justAfterLastSeparatorIndex = startOfNextSeparatorIndex + separator->Length;
    }
}

void Notebook::InstallCustomFunctions() {
    RegisterCustomFunction("error_number", 0, ErrorNumberFunc);
    RegisterCustomFunction("error_message", 0, ErrorMessageFunc);
    RegisterCustomFunction("error_state", 0, ErrorStateFunc);
    RegisterCustomFunction("read_file", -1, ReadFileFunc);
    RegisterCustomFunction("split", 3, SplitFunc);
}

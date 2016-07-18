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

private ref class CustomFunctionContext {
    public:
    Notebook^ Notebook;
    void (*Function)(sqlite3_context*, int, sqlite3_value**);
};

static Notebook^ GetNotebook(sqlite3_context* ctx) {
    return (*(gcroot<CustomFunctionContext^>*)sqlite3_user_data(ctx))->Notebook;
}

static Object^ GetArg(sqlite3_value* arg) {
    switch (sqlite3_value_type(arg)) {
        case SQLITE_INTEGER:
            return (Int64)sqlite3_value_int64(arg);
        case SQLITE_FLOAT:
            return (Double)sqlite3_value_double(arg);
        case SQLITE_NULL:
            return DBNull::Value;
        case SQLITE_TEXT:
            return Util::Str((const wchar_t*)sqlite3_value_text16(arg));
        case SQLITE_BLOB:
        {
            auto cb = sqlite3_value_bytes(arg);
            auto inputArray = (Byte*)sqlite3_value_blob(arg);
            auto outputArray = gcnew array<Byte>(cb);
            Marshal::Copy(IntPtr(inputArray), outputArray, 0, cb);
            return outputArray;
        }
        default:
            throw gcnew Exception("Data type not supported.");
    }
}

static String^ GetTypeName(int type) {
    switch (type) {
        case SQLITE_INTEGER: return "INTEGER";
        case SQLITE_FLOAT: return "FLOAT";
        case SQLITE_TEXT: return "TEXT";
        case SQLITE_NULL: return "NULL";
        case SQLITE_BLOB: return "BLOB";
        default: return String::Format("UNKNOWN ({0})", type);
    }
}

static Int64 GetIntArg(sqlite3_value* arg, String^ paramName, String^ functionName) {
    auto type = sqlite3_value_type(arg);
    if (type == SQLITE_INTEGER) { 
        return (Int64)GetArg(arg);
    } else {
        throw gcnew Exception(String::Format(
            "{0}: The \"{1}\" argument must be an INTEGER value, but type {2} was provided.",
            functionName, paramName, GetTypeName(type)));
    }
}

static String^ GetStrArg(sqlite3_value* arg, String^ paramName, String^ functionName) {
    auto type = sqlite3_value_type(arg);
    if (type == SQLITE_TEXT) {
        return (String^)GetArg(arg);
    } else {
        throw gcnew Exception(String::Format(
            "{0}: The \"{1}\" argument must be a TEXT value, but type {2} was provided.",
            functionName, paramName, GetTypeName(type)));
    }
}

static array<Byte>^ GetBlobArg(sqlite3_value* arg, String^ paramName, String^ functionName) {
    auto type = sqlite3_value_type(arg);
    if (type == SQLITE_BLOB) {
        return (array<Byte>^)GetArg(arg);
    } else {
        throw gcnew Exception(String::Format(
            "{0}: The \"{1}\" argument must be a BLOB value, but type {2} was provided.",
            functionName, paramName, GetTypeName(type)));
    }
}

static void SqliteResultError(sqlite3_context* ctx, String^ message) {
    auto messageWstr = Util::WStr(message);
    sqlite3_result_error16(ctx, messageWstr.c_str(), -1);
}

// ---

static void ErrorNumber(sqlite3_context* ctx, int, sqlite3_value**) {
    auto notebook = GetNotebook(ctx);
    Notebook::SqliteResult(ctx, notebook->UserData->LastError->ErrorNumber);
}

static void ErrorMessage(sqlite3_context* ctx, int, sqlite3_value**) {
    auto notebook = GetNotebook(ctx);
    Notebook::SqliteResult(ctx, notebook->UserData->LastError->ErrorMessage);
}

static void ErrorState(sqlite3_context* ctx, int, sqlite3_value**) {
    auto notebook = GetNotebook(ctx);
    Notebook::SqliteResult(ctx, notebook->UserData->LastError->ErrorState);
}

static void ReadFile(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    if (argc < 1 || argc > 2) {
        throw gcnew Exception("READ_FILE: Between 1 and 2 arguments are required.");
    }
    auto notebook = GetNotebook(ctx);
    auto filePathStr = GetStrArg(argv[0], "file-path", "READ_FILE");
    auto encodingNum = argc < 2 ? 0 : GetIntArg(argv[1], "file-encoding", "READ_FILE");

    if (encodingNum < 0 || encodingNum > 65535) {
        throw gcnew Exception("READ_FILE: The \"file-encoding\" argument must be between 0 and 65535.");
    }

    String^ contents;
    try {
        if (encodingNum == 0) {
            contents = File::ReadAllText(filePathStr);
        } else {
            auto encoding = Encoding::GetEncoding((int)encodingNum);
            contents = File::ReadAllText(filePathStr, encoding);
        }
    } catch (Exception^ ex) {
        throw gcnew Exception(String::Format("READ_FILE: {0}", ex->Message));
    }
    Notebook::SqliteResult(ctx, contents);
}

static void Split2(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto text = GetStrArg(argv[0], "text", "SPLIT");
    auto separator = GetStrArg(argv[1], "separator", "SPLIT");

    if (separator->Length == 0) {
        throw gcnew Exception("SPLIT: The argument \"separator\" must not be an empty string.");
    }

    auto separators = gcnew array<String^>(1);
    separators[0] = separator;
    auto splitted = text->Split(separators, StringSplitOptions::None);
    auto blob = Notebook::ConvertToSqlArray(gcnew List<Object^>(splitted));
    Notebook::SqliteResult(ctx, blob);
}

static void Split3(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto text = GetStrArg(argv[0], "text", "SPLIT");
    auto separator = GetStrArg(argv[1], "separator", "SPLIT");
    auto whichSubstring = GetIntArg(argv[2], "which-substring", "SPLIT");

    if (separator->Length == 0) {
        throw gcnew Exception("SPLIT: The argument \"separator\" must not be an empty string.");
    }
    if (whichSubstring < 0) {
        throw gcnew Exception("SPLIT: The argument \"which-substring\" cannot be a negative number.");
    }

    auto separators = gcnew array<String^>(1);
    separators[0] = separator;
    auto splitted = text->Split(separators, StringSplitOptions::None);
    if (whichSubstring < splitted->Length) {
        Notebook::SqliteResult(ctx, splitted[whichSubstring]);
    } else {
        Notebook::SqliteResult(ctx, DBNull::Value);
    }
}

static void ArrayN(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto objects = gcnew List<Object^>();
    for (int i = 0; i < argc; i++) {
        objects->Add(GetArg(argv[i]));
    }
    auto bytes = Notebook::ConvertToSqlArray(objects);
    Notebook::SqliteResult(ctx, bytes);
}

static void ArrayGet(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto blob = GetBlobArg(argv[0], "array", "ARRAY_GET");
    auto index = (int)GetIntArg(argv[1], "element-index", "ARRAY_GET");
    auto count = Notebook::GetArrayCount(blob);
    if (index < 0 || index >= count) {
        Notebook::SqliteResult(ctx, DBNull::Value);
    } else {
        auto element = Notebook::GetArrayElement(blob, index);
        Notebook::SqliteResult(ctx, element);
    }
}

static void ArraySet(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto blob = GetBlobArg(argv[0], "array", "ARRAY_SET");
    auto index = (int)GetIntArg(argv[1], "element-index", "ARRAY_SET");
    auto value = GetArg(argv[2]);
    auto count = Notebook::GetArrayCount(blob);
    if (index < 0 || index >= count) {
        throw gcnew Exception("ARRAY_SET: Argument \"element-index\" is out of range.");
    } else {
        auto insertElements = gcnew List<Object^>(1);
        insertElements->Add(value);
        auto newArrayBlob = Notebook::SliceArrayElements(blob, index, 1, insertElements);
        Notebook::SqliteResult(ctx, newArrayBlob);
    }
}

static void ArrayInsert(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    if (argc < 3) {
        throw gcnew Exception("ARRAY_INSERT: At least 3 argument are required.");
    }
    auto blob = GetBlobArg(argv[0], "array", "ARRAY_INSERT");
    auto index = (int)GetIntArg(argv[1], "element-index", "ARRAY_INSERT");
    auto values = gcnew List<Object^>();
    for (int i = 2; i < argc; i++) {
        values->Add(GetArg(argv[i]));
    }
    auto count = Notebook::GetArrayCount(blob);
    if (index < 0 || index > count) {
        throw gcnew Exception("ARRAY_INSERT: Argument \"element-index\" is out of range.");
    } else {
        auto newArrayBlob = Notebook::SliceArrayElements(blob, index, 0, values);
        Notebook::SqliteResult(ctx, newArrayBlob);
    }
}

static void ArrayAppend(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    if (argc < 2) {
        throw gcnew Exception("ARRAY_APPEND: At least 2 arguments are required.");
    }
    auto blob = GetBlobArg(argv[0], "array", "ARRAY_APPEND");
    auto values = gcnew List<Object^>();
    for (int i = 1; i < argc; i++) {
        values->Add(GetArg(argv[i]));
    }
    auto count = Notebook::GetArrayCount(blob);
    auto newArrayBlob = Notebook::SliceArrayElements(blob, count, 0, values);
    Notebook::SqliteResult(ctx, newArrayBlob);
}

static void ArrayCount(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto blob = GetBlobArg(argv[0], "array", "ARRAY_COUNT");
    auto count = Notebook::GetArrayCount(blob);
    Notebook::SqliteResult(ctx, count);
}

static void ArrayConcat1(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto blob = GetBlobArg(argv[0], "array", "ARRAY_CONCAT");
    auto elements = Notebook::GetArrayElements(blob);
    auto concatted = String::Join("", elements);
    Notebook::SqliteResult(ctx, concatted);
}

static void ArrayConcat2(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    auto blob = GetBlobArg(argv[0], "array", "ARRAY_CONCAT");
    auto separator = GetStrArg(argv[1], "separator", "ARRAY_CONCAT");
    auto elements = Notebook::GetArrayElements(blob);
    auto concatted = String::Join(separator, elements);
    Notebook::SqliteResult(ctx, concatted);
}

static void ArrayMerge(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    if (argc < 2) {
        throw gcnew Exception("ARRAY_MERGE: At least 2 arguments are required.");
    }
    auto values = gcnew List<Object^>();
    for (int i = 0; i < argc; i++) {
        auto blob = GetBlobArg(argv[i], String::Format("array #{0}", i + 1), "ARRAY_MERGE");
        auto blobValues = Notebook::GetArrayElements(blob);
        values->AddRange(blobValues);
    }
    auto merged = Notebook::ConvertToSqlArray(values);
    Notebook::SqliteResult(ctx, merged);
}

// ---

static void DeleteGcrootCustomFunctionContext(void* ptr) {
    delete (gcroot<CustomFunctionContext^>*)ptr;
}

static void ExecuteCustomFunction(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    try {
        auto context = *(gcroot<CustomFunctionContext^>*)sqlite3_user_data(ctx);
        context->Function(ctx, argc, argv);
    } catch (Exception^ ex) {
        SqliteResultError(ctx, ex->Message);
    }
}

void Notebook::RegisterCustomFunction(const char* functionName, int numArgs,
void(*func)(sqlite3_context*, int, sqlite3_value**), bool deterministic) {
    auto context = gcnew CustomFunctionContext();
    context->Notebook = this;
    context->Function = func;

    SqliteCall(sqlite3_create_function_v2(
        /* db */ _sqlite,
        /* zFunctionName */ functionName,
        /* nArg */ numArgs,
        /* eTextRep */ SQLITE_UTF16 | (deterministic ? SQLITE_DETERMINISTIC : 0),
        /* pApp */ new gcroot<CustomFunctionContext^>(context),
        /* xFunc */ ExecuteCustomFunction,
        /* xStep */ nullptr,
        /* xFinal */ nullptr,
        /* xDestroy */ DeleteGcrootCustomFunctionContext
    ));
}

void Notebook::InstallCustomFunctions() {
    RegisterCustomFunction("error_number", 0, ErrorNumber, false);
    RegisterCustomFunction("error_message", 0, ErrorMessage, false);
    RegisterCustomFunction("error_state", 0, ErrorState, false);
    RegisterCustomFunction("read_file", -1, ReadFile, false);
    RegisterCustomFunction("split", 2, Split2, true);
    RegisterCustomFunction("split", 3, Split3, true);
    RegisterCustomFunction("array", -1, ArrayN, true);
    RegisterCustomFunction("array_get", 2, ArrayGet, true);
    RegisterCustomFunction("array_set", 3, ArraySet, true);
    RegisterCustomFunction("array_insert", -1, ArrayInsert, true);
    RegisterCustomFunction("array_append", -1, ArrayAppend, true);
    RegisterCustomFunction("array_count", 1, ArrayCount, true);
    RegisterCustomFunction("array_concat", 1, ArrayConcat1, true);
    RegisterCustomFunction("array_concat", 2, ArrayConcat2, true);
    RegisterCustomFunction("array_merge", -1, ArrayMerge, true);
}

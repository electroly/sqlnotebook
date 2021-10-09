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
using namespace System::Text::Json;

private ref class CustomFunctionContext {
    public:
    Notebook^ Notebook;
    void (*Function)(sqlite3_context*, int, sqlite3_value**);
};

private ref class GenericFunctionContext {
    public:
    Notebook^ Notebook;
    CustomScalarFunction^ Function;
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

static void SqliteResultError(sqlite3_context* ctx, String^ message) {
    auto messageWstr = Util::WStr(message);
    sqlite3_result_error16(ctx, messageWstr.c_str(), -1);
}

static void DeleteGcrootCustomFunctionContext(void* ptr) {
    delete (gcroot<CustomFunctionContext^>*)ptr;
}

static void DeleteGcrootGenericFunctionContext(void* ptr) {
    delete (gcroot<GenericFunctionContext^>*)ptr;
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

static void ExecuteGenericFunction(sqlite3_context* ctx, int argc, sqlite3_value** argv) {
    try {
        GenericFunctionContext^ context = *(gcroot<GenericFunctionContext^>*)sqlite3_user_data(ctx);
        auto args = gcnew List<Object^>();
        for (int i = 0; i < argc; i++) {
            args->Add(GetArg(argv[i]));
        }
        auto result = (context->Function)->Execute(args);
        Notebook::SqliteResult(ctx, result);
    } catch (Exception^ ex) {
        SqliteResultError(ctx, ex->Message);
    }
}

void Notebook::RegisterGenericFunction(CustomScalarFunction^ function) {
    auto context = gcnew GenericFunctionContext();
    context->Notebook = this;
    context->Function = function;
    auto nameCstr = Util::CStr(function->Name);

    SqliteCall(sqlite3_create_function_v2(
        /* db */ _sqlite,
        /* zFunctionName */ nameCstr.c_str(),
        /* nArg */ function->ParamCount,
        /* eTextRep */ SQLITE_UTF16 | (function->IsDeterministic ? SQLITE_DETERMINISTIC : 0),
        /* pApp */ new gcroot<GenericFunctionContext^>(context),
        /* xFunc */ ExecuteGenericFunction,
        /* xStep */ nullptr,
        /* xFinal */ nullptr,
        /* xDestroy */ DeleteGcrootGenericFunctionContext
    ));
}

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

void Notebook::InstallCustomFunctions() {
    RegisterCustomFunction("error_number", 0, ErrorNumber, false);
    RegisterCustomFunction("error_message", 0, ErrorMessage, false);
    RegisterCustomFunction("error_state", 0, ErrorState, false);
}

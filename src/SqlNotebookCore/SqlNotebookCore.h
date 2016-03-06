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

#pragma once
#include <string>
#include <vector>
#include "sqlite3.h"
using namespace System;
using namespace System::Data;
using namespace System::Collections::Generic;
using namespace System::Threading;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Concurrent;

void g_SqliteCall(sqlite3* sqlite, int result); // handle the errcode by throwing if non-SQLITE_OK

namespace SqlNotebookCore {
    public ref class Notebook sealed : public IDisposable {
        public:
        Notebook(String^ filePath);
        ~Notebook();
        !Notebook();
        void Invoke(Action^ action); // run the action on the sqlite thread

        // all of these methods must be run on the sqlite thread
        void Execute(String^ sql);
        void Execute(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        void Execute(String^ sql, IReadOnlyList<Object^>^ args);
        DataTable^ Query(String^ sql);
        DataTable^ Query(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        DataTable^ Query(String^ sql, IReadOnlyList<Object^>^ args);

        private:
        bool _isDisposed;
        String^ _filePath;
        sqlite3* _sqlite;
        
        // the sqlite thread pump
        Task^ _thread;
        CancellationTokenSource^ _threadCancellationTokenSource;
        BlockingCollection<Action^>^ _threadQueue;

        DataTable^ QueryCore(String^ sql, IReadOnlyDictionary<String^, Object^>^ namedArgs, 
            IReadOnlyList<Object^>^ orderedArgs, bool returnResult);
        void InstallCsvModule();
        void InstallPgModule();
        void SqliteCall(int result);
        void SqliteThread();
        void Init();
    };

    public ref class Util abstract sealed {
        public:
        static std::wstring WStr(String^ mstr); // to UTF-16
        static std::string CStr(String^ mstr); // to UTF-8
        static String^ Str(const wchar_t* utf16Str);
        static String^ Str(const char* utf8Str);
    };

    public ref class SqliteException sealed : public Exception {
        public:
        SqliteException(String^ message) : Exception(message) {}
    };
}

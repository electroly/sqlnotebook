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
#include <msclr/gcroot.h>
#include "../sqlite3/sqlite3-ex.h"
using namespace SqlNotebookCoreModules;
using namespace System;
using namespace System::Data;
using namespace System::Collections::Generic;
using namespace System::Threading;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Concurrent;
using namespace msclr;

void g_SqliteCall(sqlite3* sqlite, int result); // handle the errcode by throwing if non-SQLITE_OK

namespace SqlNotebookCore {
    public enum class TokenType {
        Semi = 1,
        Explain = 2,
        Query = 3,
        Plan = 4,
        Begin = 5,
        Transaction = 6,
        Deferred = 7,
        Immediate = 8,
        Exclusive = 9,
        Commit = 10,
        End = 11,
        Rollback = 12,
        Savepoint = 13,
        Release = 14,
        To = 15,
        Table = 16,
        Create = 17,
        If = 18,
        Not = 19,
        Exists = 20,
        Temp = 21,
        Lp = 22,
        Rp = 23,
        As = 24,
        Without = 25,
        Comma = 26,
        Id = 27,
        Indexed = 28,
        Abort = 29,
        Action = 30,
        After = 31,
        Analyze = 32,
        Asc = 33,
        Attach = 34,
        Before = 35,
        By = 36,
        Cascade = 37,
        Cast = 38,
        ColumnKw = 39,
        Conflict = 40,
        Database = 41,
        Desc = 42,
        Detach = 43,
        Each = 44,
        Fail = 45,
        For = 46,
        Ignore = 47,
        Initially = 48,
        Instead = 49,
        LikeKw = 50,
        Match = 51,
        No = 52,
        Key = 53,
        Of = 54,
        Offset = 55,
        Pragma = 56,
        Raise = 57,
        Recursive = 58,
        Replace = 59,
        Restrict = 60,
        Row = 61,
        Trigger = 62,
        Vacuum = 63,
        View = 64,
        Virtual = 65,
        With = 66,
        Reindex = 67,
        Rename = 68,
        CtimeKw = 69,
        Any = 70,
        Or = 71,
        And = 72,
        Is = 73,
        Between = 74,
        In = 75,
        IsNull = 76,
        NotNull = 77,
        Ne = 78,
        Eq = 79,
        Gt = 80,
        Le = 81,
        Lt = 82,
        Ge = 83,
        Escape = 84,
        BitAnd = 85,
        BitOr = 86,
        LShift = 87,
        RShift = 88,
        Plus = 89,
        Minus = 90,
        Star = 91,
        Slash = 92,
        Rem = 93,
        Concat = 94,
        Collate = 95,
        Bitnot = 96,
        String = 97,
        JoinKw = 98,
        Constraint = 99,
        Default = 100,
        Null = 101,
        Primary = 102,
        Unique = 103,
        Check = 104,
        References = 105,
        Autoincr = 106,
        On = 107,
        Insert = 108,
        Delete = 109,
        Update = 110,
        Set = 111,
        Deferrable = 112,
        Foreign = 113,
        Drop = 114,
        Union = 115,
        All = 116,
        Except = 117,
        Intersect = 118,
        Select = 119,
        Values = 120,
        Distinct = 121,
        Dot = 122,
        From = 123,
        Join = 124,
        Using = 125,
        Order = 126,
        Group = 127,
        Having = 128,
        Limit = 129,
        Where = 130,
        Into = 131,
        Integer = 132,
        Float = 133,
        Blob = 134,
        Variable = 135,
        Case = 136,
        When = 137,
        Then = 138,
        Else = 139,
        Index = 140,
        Alter = 141,
        Add = 142,
        ToText = 143,
        ToBlob = 144,
        ToNumeric = 145,
        ToInt = 146,
        ToReal = 147,
        IsNot = 148,
        EndOfFile = 149,
        UnclosedString = 150,
        Function = 151,
        Column = 152,
        AggFunction = 153,
        AggColumn = 154,
        UMinus = 155,
        UPlus = 156,
        Register = 157,
        Asterisk = 158,
        Span = 159,
        Space = 160,
        Illegal = 161
    };

    public ref class Token sealed {
        public:
        TokenType Type;
        String^ Text;
        String^ ToString() override;
        size_t Utf8Start;
        size_t Utf8Length;
    };

    public ref class SimpleDataTable sealed {
        public:
        IReadOnlyList<String^>^ Columns;
        IReadOnlyList<array<Object^>^>^ Rows;
        SimpleDataTable(IReadOnlyList<String^>^ columns, IReadOnlyList<array<Object^>^>^ rows);
        Object^ Get(int rowNumber, String^ column);
        int GetIndex(String^ column);

        private:
        IReadOnlyDictionary<String^, int>^ _columnIndices;
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

    public ref class UserCancelException sealed : public Exception {
        public:
        UserCancelException(String^ message) : Exception(message) {}
    };

    public ref class HttpRequestEventArgs sealed : public EventArgs {
        public:
        String^ Url;
        array<Byte>^ Result;
    };

    public ref class HttpServer sealed : public IDisposable {
        public:
        HttpServer(uint16_t port);
        ~HttpServer();
        !HttpServer();
        event EventHandler<HttpRequestEventArgs^>^ Request;

        internal:
        void SendRequestEvent(HttpRequestEventArgs^ e);

        private:
        bool _isDisposed;
        void* _mhd;
        void* _thisRef; // gcroot<HttpServer^>*
    };

    public ref class NotebookItemRecord sealed {
        public:
        String^ Name;
        String^ Type;
        String^ Data;
    };

    public ref class ScriptParameterRecord sealed {
        public:
        String^ ScriptName;
        List<String^>^ ParamNames = gcnew List<String^>();
    };

    public ref class LastErrorRecord sealed {
        public:
        Object^ ErrorNumber;
        Object^ ErrorMessage;
        Object^ ErrorState;
    };

    public ref class ConsoleHistoryRecord sealed {
        public:
        String^ Name;
        List<String^>^ History = gcnew List<String^>();
    };

    public ref class NotebookUserData sealed {
        public:
        List<NotebookItemRecord^>^ Items = gcnew List<NotebookItemRecord^>();
        List<ScriptParameterRecord^>^ ScriptParameters = gcnew List<ScriptParameterRecord^>();
        LastErrorRecord^ LastError = gcnew LastErrorRecord();
        List<ConsoleHistoryRecord^>^ ConsoleHistories = gcnew List<ConsoleHistoryRecord^>();
    };

    public ref class Notebook sealed : public IDisposable {
        public:
        Notebook(String^ filePath, bool isNew);
        ~Notebook();
        !Notebook();
        void Invoke(Action^ action); // run the action on the sqlite thread
        String^ GetFilePath();
        void BeginUserCancel();
        void EndUserCancel();
        property NotebookUserData^ UserData { NotebookUserData^ get() { return _userData; } }
        IReadOnlyDictionary<String^, String^>^ GetScripts(); // lowercase script name -> script code
        static IReadOnlyList<Token^>^ Tokenize(String^ input);
        SimpleDataTable^ SpecialReadOnlyQuery(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);

        // all of these methods must be run on the sqlite thread
        void Execute(String^ sql);
        void Execute(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        void Execute(String^ sql, IReadOnlyList<Object^>^ args);
        SimpleDataTable^ Query(String^ sql);
        SimpleDataTable^ Query(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        SimpleDataTable^ Query(String^ sql, IReadOnlyList<Object^>^ args);
        Object^ QueryValue(String^ sql);
        Object^ QueryValue(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        Object^ QueryValue(String^ sql, IReadOnlyList<Object^>^ args);
        void Save();
        void SaveAs(String^ filePath);
        String^ FindLongestValidStatementPrefix(String^ input);
        bool IsTransactionActive();

        private:
        bool _isDisposed;
        String^ _originalFilePath;
        String^ _workingCopyFilePath;
        sqlite3* _sqlite;
        Object^ _lock;
        NotebookUserData^ _userData;
        bool _cancelling;
        static Object^ _tokenizeLock = gcnew Object();
        List<IntPtr>^ _sqliteModules = gcnew List<IntPtr>();

        static SimpleDataTable^ QueryCore(String^ sql, IReadOnlyDictionary<String^, Object^>^ namedArgs,
            IReadOnlyList<Object^>^ orderedArgs, bool returnResult, sqlite3* db, Func<bool>^ cancelling);
        void InstallPgModule();
        void InstallMsModule();
        void InstallMyModule();
        void InstallCustomFunctions();
        void InstallGenericModule(CustomTableFunction^ impl);
        void SqliteCall(int result);
        void Init();
        bool GetCancelling();
        void RegisterCustomFunction(const char* functionName, int numArgs,
            void(*func)(sqlite3_context*, int, sqlite3_value **), bool deterministic);
        void RegisterGenericFunction(CustomScalarFunction^ function);

        internal:
        static void SqliteResult(sqlite3_context* ctx, Object^ value);
    };

    public ref class NotebookTempFiles sealed abstract {
        public:
        static void Init();
        static String^ GetTempFilePath(String^ extension);
        static void DeleteFiles();

        private:
        static String^ _path;
    };
}

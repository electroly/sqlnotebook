#pragma once
#include <string>
#include <vector>
#include <msclr/gcroot.h>
#include "../sqlite3/sqlite3-ex.h"
using namespace SqlNotebookScript;
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

    public ref class Notebook sealed : public IDisposable, public INotebook {
        public:
        Notebook(String^ filePath, bool isNew);
        ~Notebook();
        !Notebook();
        void Invoke(Action^ action); // run the action on the sqlite thread
        String^ GetFilePath();
        void BeginUserCancel();
        void EndUserCancel();
        property NotebookUserData^ UserData { virtual NotebookUserData^ get() { return _userData; } }
        IReadOnlyDictionary<String^, String^>^ GetScripts(); // lowercase script name -> script code
        virtual IReadOnlyList<Token^>^ Tokenize(String^ input);
        static IReadOnlyList<Token^>^ StaticTokenize(String^ input);
        SimpleDataTable^ SpecialReadOnlyQuery(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);

        // all of these methods must be run on the sqlite thread
        virtual void Execute(String^ sql);
        virtual void Execute(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        virtual void Execute(String^ sql, IReadOnlyList<Object^>^ args);
        virtual SimpleDataTable^ Query(String^ sql);
        virtual SimpleDataTable^ Query(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        virtual SimpleDataTable^ Query(String^ sql, IReadOnlyList<Object^>^ args);
        virtual Object^ QueryValue(String^ sql);
        virtual Object^ QueryValue(String^ sql, IReadOnlyDictionary<String^, Object^>^ args);
        virtual Object^ QueryValue(String^ sql, IReadOnlyList<Object^>^ args);
        void Save();
        void SaveAs(String^ filePath);
        virtual bool IsTransactionActive();

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
        void WriteFileVersionAndUserDataToDatabase();
        void ReadUserDataFromDatabase();
        static int GetFileVersion(String^ filePath);
        static void MigrateFileVersion1to2(String^ filePath);

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

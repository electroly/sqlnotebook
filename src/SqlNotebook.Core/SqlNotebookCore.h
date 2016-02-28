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

namespace SqlNotebookCore {

    public struct NativeSqliteModule {
        sqlite3_module super;
    };

    public enum class SqliteIndexConstraintOperator {
        Eq = SQLITE_INDEX_CONSTRAINT_EQ,
        Gt = SQLITE_INDEX_CONSTRAINT_GT,
        Le = SQLITE_INDEX_CONSTRAINT_LE,
        Lt = SQLITE_INDEX_CONSTRAINT_LT,
        Ge = SQLITE_INDEX_CONSTRAINT_GE,
        Match = SQLITE_INDEX_CONSTRAINT_MATCH,
        Like = SQLITE_INDEX_CONSTRAINT_LIKE,
        Glob = SQLITE_INDEX_CONSTRAINT_GLOB,
        RegExp = SQLITE_INDEX_CONSTRAINT_REGEXP
    };

    public ref class SqliteIndexConstraint sealed {
        public:
        initonly int Column; // Column constrained.  -1 for ROWID
        initonly SqliteIndexConstraintOperator Op; // Constraint operator
        initonly bool Usable; // True if this constraint is usable

        SqliteIndexConstraint(int column, SqliteIndexConstraintOperator op, bool usable);
    };

    public ref class SqliteOrderBy sealed {
        public:
        initonly int Column; // Column number
        initonly bool Desc; // True for DESC.  False for ASC.

        SqliteOrderBy(int column, bool desc);
    };

    public ref class SqliteIndexInfoInput sealed {
        public:
        initonly IReadOnlyList<SqliteIndexConstraint^>^ Constraints; // Table of WHERE clause constraints
        initonly IReadOnlyList<SqliteOrderBy^>^ Order; // The ORDER BY clause

        SqliteIndexInfoInput(IReadOnlyList<SqliteIndexConstraint^>^ constraints, IReadOnlyList<SqliteOrderBy^>^ order);
    };

    public ref class SqliteIndexConstraintUsage sealed {
        public:
        initonly int ArgIndex; // if >0, constraint is part of argv to xFilter
        initonly bool Omit; // Do not code a test for this constraint

        SqliteIndexConstraintUsage(int argIndex, bool omit);
    };

    public ref class SqliteIndexInfoOutput sealed {
        public:
        initonly IReadOnlyList<SqliteIndexConstraintUsage^>^ ConstraintUsage;
        initonly int IndexNumber; // Number used to identify the index
        initonly bool OrderByConsumed; // True if output is already ordered
        initonly double EstimatedCost; // Estimated cost of using this index
        initonly int64_t EstimatedRows; // Estimated number of rows returned

        SqliteIndexInfoOutput(IReadOnlyList<SqliteIndexConstraintUsage^>^ constraintUsage, int indexNumber,
            bool orderByConsumed, double estimatedCost, int64_t estimatedRows);
    };

    public interface class ISqliteVirtualTable : public IDisposable {
        void Disconnect();
        void Drop();
    };

    public interface class ISqliteVirtualTableCursor : public IDisposable {
        void Filter(int indexNumber, IReadOnlyList<Object^>^ args);
        void Next();
        bool Eof();
        Object Column(int columnIndex);
        int64_t Rowid();
    };

    public interface class ISqliteModule : public IDisposable {
        ISqliteVirtualTable^ Create(IReadOnlyList<String^>^ args);
        ISqliteVirtualTable^ Connect(IReadOnlyList<String^>^ args);
        SqliteIndexInfoOutput^ BestIndex(ISqliteVirtualTable^ vtab, SqliteIndexInfoInput^ input);
        ISqliteVirtualTableCursor^ Open(ISqliteVirtualTable^ vtab);
    };

    public ref class RegisteredModule sealed {
        public:
        RegisteredModule(ISqliteModule^ clientModule, NativeSqliteModule* sqliteModule);
        ~RegisteredModule();
        !RegisteredModule();

        private:
        bool _isDisposed;
        ISqliteModule^ _clientModule;
        NativeSqliteModule* _sqliteModule;
    };

    public ref class Notebook sealed {
        public:
        Notebook(String^ filePath);
        ~Notebook();
        !Notebook();
        void RegisterModule(String^ moduleName, ISqliteModule^ module);
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
        List<RegisteredModule^>^ _modules;

        DataTable^ QueryCore(String^ sql, IReadOnlyDictionary<String^, Object^>^ namedArgs, 
            IReadOnlyList<Object^>^ orderedArgs, bool returnResult);
        void SqliteCall(int result); // handle the errcode by throwing if non-SQLITE_OK
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
        SqliteException(String^ message);
    };
}

using namespace SqlNotebookCore;

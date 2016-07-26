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

using System.Collections.Generic;

namespace SqlNotebookScript {
    public interface INotebook {
        void Execute(string sql);
        void Execute(string sql, IReadOnlyList<object> args);
        void Execute(string sql, IReadOnlyDictionary<string, object> args);
        SimpleDataTable Query(string sql);
        SimpleDataTable Query(string sql, IReadOnlyList<object> args);
        SimpleDataTable Query(string sql, IReadOnlyDictionary<string, object> args);
        object QueryValue(string sql);
        object QueryValue(string sql, IReadOnlyList<object> args);
        object QueryValue(string sql, IReadOnlyDictionary<string, object> args);
        IReadOnlyList<Token> Tokenize(string input);
        NotebookUserData UserData { get; }
        bool IsTransactionActive();
    }

    public sealed class NotebookItemRecord {
        public string Name;
        public string Type;
        public string Data;
    };

    public sealed class ScriptParameterRecord {
        public string ScriptName;
        public List<string> ParamNames = new List<string>();
    };

    public sealed class LastErrorRecord {
        public object ErrorNumber;
        public object ErrorMessage;
        public object ErrorState;
    };

    public sealed class ConsoleHistoryRecord {
        public string Name;
        public List<string> History = new List<string>();
    };

    public sealed class NotebookUserData {
        public List<NotebookItemRecord> Items = new List<NotebookItemRecord>();
        public List<ScriptParameterRecord> ScriptParameters = new List<ScriptParameterRecord>();
        public LastErrorRecord LastError = new LastErrorRecord();
        public List<ConsoleHistoryRecord> ConsoleHistories = new List<ConsoleHistoryRecord>();
    };

    public sealed class Token {
        public TokenType Type;
        public string Text;
        public override string ToString() => $"{Type}: \"{Text}\"";
        public ulong Utf8Start;
        public ulong Utf8Length;
    };

    public enum TokenType {
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
    }
}

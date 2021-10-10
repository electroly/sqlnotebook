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
    }

    public sealed class ScriptParameterRecord {
        public string ScriptName;
        public List<string> ParamNames = new List<string>();
    }

    public sealed class LastErrorRecord {
        public object ErrorNumber;
        public object ErrorMessage;
        public object ErrorState;
    }

    public sealed class ConsoleStateRecord {
        public string ConsoleName;
        public List<string> VarNames;
        public string VarDataB64; // Base-64 encoded blob array
    }

    public sealed class NotebookUserData {
        public List<NotebookItemRecord> Items = new List<NotebookItemRecord>();
        public List<ScriptParameterRecord> ScriptParameters = new List<ScriptParameterRecord>();
        public LastErrorRecord LastError = new LastErrorRecord();
        public List<ConsoleStateRecord> ConsoleStates = new List<ConsoleStateRecord>();
    }

    public sealed class Token {
        public TokenType Type;
        public string Text;
        public override string ToString() => $"{Type}: \"{Text}\"";
        public ulong Utf8Start;
        public ulong Utf8Length;
    }

    // this enum list can be generated from sqlite3.c's #define TK_* list using the following formula in Excel:
    // =SUBSTITUTE(PROPER(SUBSTITUTE(TRIM(MID(A1,8,30)),"TK_","")),"_","")&" = "&TRIM(RIGHT(A1,5))&","
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
        Or = 27,
        And = 28,
        Is = 29,
        Match = 30,
        LikeKw = 31,
        Between = 32,
        In = 33,
        Isnull = 34,
        Notnull = 35,
        Ne = 36,
        Eq = 37,
        Gt = 38,
        Le = 39,
        Lt = 40,
        Ge = 41,
        Escape = 42,
        Bitand = 43,
        Bitor = 44,
        Lshift = 45,
        Rshift = 46,
        Plus = 47,
        Minus = 48,
        Star = 49,
        Slash = 50,
        Rem = 51,
        Concat = 52,
        Collate = 53,
        Bitnot = 54,
        Id = 55,
        Indexed = 56,
        Abort = 57,
        Action = 58,
        After = 59,
        Analyze = 60,
        Asc = 61,
        Attach = 62,
        Before = 63,
        By = 64,
        Cascade = 65,
        Cast = 66,
        Columnkw = 67,
        Conflict = 68,
        Database = 69,
        Desc = 70,
        Detach = 71,
        Each = 72,
        Fail = 73,
        For = 74,
        Ignore = 75,
        Initially = 76,
        Instead = 77,
        No = 78,
        Key = 79,
        Of = 80,
        Offset = 81,
        Pragma = 82,
        Raise = 83,
        Recursive = 84,
        Replace = 85,
        Restrict = 86,
        Row = 87,
        Trigger = 88,
        Vacuum = 89,
        View = 90,
        Virtual = 91,
        With = 92,
        Reindex = 93,
        Rename = 94,
        CtimeKw = 95,
        Any = 96,
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
        Isnot = 148,
        EndOfFile = 149,
        UnclosedString = 150,
        Function = 151,
        Column = 152,
        AggFunction = 153,
        AggColumn = 154,
        Uminus = 155,
        Uplus = 156,
        Register = 157,
        Asterisk = 158,
        Span = 159,
        Space = 160,
        Illegal = 161
    }
}

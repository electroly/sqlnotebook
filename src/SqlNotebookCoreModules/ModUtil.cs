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

using System;

namespace SqlNotebookCoreModules {
    public enum DatePart {
        Year, Quarter, Month, DayOfYear, Day, Week, DayOfWeek, Hour, Minute, Second, Millisecond, TzOffset
    }

    public static class ModUtil {
        public static long GetIntArg(object arg, string paramName, string functionName) {
            if (arg is int || arg is long) {
                return Convert.ToInt64(arg);
            } else {
                throw new Exception($"{functionName.ToUpper()}: The \"{paramName}\" argument must be an INTEGER " +
                    $" value, but type {GetTypeName(arg.GetType())} was provided.");
            }
        }

        public static int GetInt32Arg(object arg, string paramName, string functionName) {
            if (arg is int || arg is long) {
                var value64 = Convert.ToInt64(arg);
                if (value64 < int.MinValue || value64 > int.MaxValue) {
                    throw new Exception($"{functionName.ToUpper()}: The \"{paramName}\" argument must be between " +
                        $"{int.MinValue} and {int.MaxValue}.");
                } else {
                    return (int)value64;
                }
            } else {
                throw new Exception($"{functionName.ToUpper()}: The \"{paramName}\" argument must be an INTEGER " +
                    $" value, but type {GetTypeName(arg.GetType())} was provided.");
            }
        }

        public static string GetStrArg(object arg, string paramName, string functionName) {
            if (arg is string) {
                return (string)arg;
            } else {
                throw new Exception($"{functionName.ToUpper()}: The \"{paramName}\" argument must be a TEXT value, " +
                    $"but type {GetTypeName(arg.GetType())} was provided.");
            }
        }

        public static DateTimeOffset GetDateArg(object arg, string paramName, string functionName) {
            var text = GetStrArg(arg, paramName, functionName);
            DateTimeOffset date;
            if (DateTimeOffset.TryParse(text, out date)) {
                return date;
            } else {
                throw new Exception(
                    $"{functionName.ToUpper()}: The \"{paramName}\" argument must be a valid date string.");
            }
        }

        public static DatePart GetDatePartArg(object arg, string paramName, string functionName) {
            var text = GetStrArg(arg, paramName, functionName);
            switch (text.ToLower()) {
                case "year": case "yyyy": case "yy": return DatePart.Year;
                case "quarter": case "qq": case "q": return DatePart.Quarter;
                case "month": case "mm": case "m": return DatePart.Month;
                case "dayofyear": case "dy": case "y": return DatePart.DayOfYear;
                case "day": case "dd": case "d": return DatePart.Day;
                case "week": case "wk": case "ww": return DatePart.Week;
                case "weekday": case "dw": return DatePart.DayOfWeek;
                case "hour": case "hh": return DatePart.Hour;
                case "minute": case "mi": case "n": return DatePart.Minute;
                case "second": case "ss": case "s": return DatePart.Second;
                case "millisecond": case "ms": return DatePart.Millisecond;
                case "tzoffset": case "tz": return DatePart.TzOffset;
                default: throw new Exception(
                    $"{functionName.ToUpper()}: The \"{paramName}\" argument must be one of the recognized " +
                    "date part strings, such as 'year' or 'day'.");
            }
        }

        public static string GetTypeName(Type type) {
            if (type == typeof(int) || type == typeof(long)) {
                return "INTEGER";
            } else if (type == typeof(float) || type == typeof(double)) {
                return "FLOAT";
            } else if (type == typeof(string)) {
                return "TEXT";
            } else if (type == typeof(DBNull)) {
                return "NULL";
            } else if (type == typeof(byte[])) {
                return "BLOB";
            } else {
                return $"UNKNOWN ({type.Name})";
            }
        }
    }
}

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
using System.Globalization;

namespace SqlNotebookScript.Utils {
    public static class DateTimeUtil {
        public static string FormatDate(DateTime dt) => dt.ToString("yyyy-MM-dd");
        public static string FormatDateTime(DateTime dt) => dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
        public static string FormatDateTimeOffset(DateTime dt) => dt.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
        public static string FormatDateTimeOffset(DateTimeOffset dt) => dt.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
        public static int GetQuarter(DateTimeOffset dt) => GetQuarter(dt.Month);
        public static int GetQuarter(int month) =>
            month <= 3 ? 1 :
            month <= 6 ? 2 :
            month <= 9 ? 3 :
            4;

        public static int GetDatePart(DateTimeOffset date, DatePart datePart) {
            var cal = CultureInfo.CurrentCulture.Calendar;
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
            switch (datePart) {
                case DatePart.Year:
                    return cal.GetYear(date.DateTime);
                case DatePart.Quarter:
                    return GetQuarter(date);
                case DatePart.Month:
                    return cal.GetMonth(date.DateTime);
                case DatePart.DayOfYear:
                    return cal.GetDayOfYear(date.DateTime);
                case DatePart.Day:
                    return cal.GetDayOfMonth(date.DateTime);
                case DatePart.Week:
                    return cal.GetWeekOfYear(date.DateTime, dtf.CalendarWeekRule, dtf.FirstDayOfWeek);
                case DatePart.DayOfWeek:
                    return DayOfWeekToInt(cal.GetDayOfWeek(date.DateTime));
                case DatePart.Hour:
                    return cal.GetHour(date.DateTime);
                case DatePart.Minute:
                    return cal.GetMinute(date.DateTime);
                case DatePart.Second:
                    return cal.GetSecond(date.DateTime);
                case DatePart.Millisecond:
                    return (int)cal.GetMilliseconds(date.DateTime);
                case DatePart.TzOffset:
                    return (int)date.Offset.TotalMinutes;
                default:
                    throw new Exception("Internal error: unrecognized DatePart.");
            }
        }

        public static int DayOfWeekToInt(DayOfWeek dow) {
            switch (dow) {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
                default: throw new Exception("Internal error: unrecognized DayOfWeek.");
            }
        }

        public static DayOfWeek IntToDayOfWeek(int dow) {
            switch (dow) {
                case 1: return DayOfWeek.Monday;
                case 2: return DayOfWeek.Tuesday;
                case 3: return DayOfWeek.Wednesday;
                case 4: return DayOfWeek.Thursday;
                case 5: return DayOfWeek.Friday;
                case 6: return DayOfWeek.Saturday;
                case 7: return DayOfWeek.Sunday;
                default: throw new Exception("Internal error: unrecognized day of week number.");
            }
        }
    }
}

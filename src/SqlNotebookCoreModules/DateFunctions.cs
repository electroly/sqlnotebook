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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlNotebookCoreModules {
    public sealed class DateAddFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "dateadd";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ModUtil.GetDatePartArg(args[0], "date-part", Name);
            var number = ModUtil.GetInt32Arg(args[1], "number", Name);
            var date = ModUtil.GetDateArg(args[2], "date", Name);
            var newDate = date;

            switch (datePart) {
                case DatePart.Year: newDate = date.AddYears(number); break;
                case DatePart.Quarter: newDate = date.AddMonths(number * 3); break;
                case DatePart.Month: newDate = date.AddMonths(number); break;
                case DatePart.DayOfYear: case DatePart.Day: case DatePart.DayOfWeek:
                    newDate = date.AddDays(number); break;
                case DatePart.Week: newDate = date.AddDays(7 * number); break;
                case DatePart.Hour: newDate = date.AddHours(number); break;
                case DatePart.Minute: newDate = date.AddMinutes(number); break;
                case DatePart.Second: newDate = date.AddSeconds(number); break;
                case DatePart.Millisecond: newDate = date.AddMilliseconds(number); break;
                case DatePart.TzOffset: throw new Exception(
                    $"{Name.ToUpper()}: The \"date-part\" argument must not be 'TZOFFSET'.");
                default: throw new Exception($"Internal error: unrecognized DatePart.");
            }

            return DateUtil.FormatDateTimeOffset(newDate);
        }
    }

    public sealed class DateDiffFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datediff";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ModUtil.GetDatePartArg(args[0], "date-part", Name);
            var startDate = ModUtil.GetDateArg(args[1], "start-date", Name);
            var endDate = ModUtil.GetDateArg(args[2], "end-date", Name);

            // datediff() counts the number of calendar/clock boundaries crossed, which is different than
            // the amount of time between the two dates.  for instance, if startDate is one second before
            // midnight and endDate is one second after, that's one day boundary crossed.
            
            if (startDate == endDate) {
                return 0;
            }

            startDate = TruncateDate(datePart, startDate);
            endDate = TruncateDate(datePart, endDate);

            var span = endDate - startDate;
            var totalMonths = (endDate.Year * 12L + endDate.Month) - (startDate.Year * 12L + startDate.Month);

            switch (datePart) {
                case DatePart.Year: return (long)endDate.Year - startDate.Year;
                case DatePart.Quarter: return totalMonths / 3;
                case DatePart.Month: return totalMonths;
                case DatePart.DayOfYear: case DatePart.Day: case DatePart.DayOfWeek:
                    return (long)span.TotalDays;
                case DatePart.Week: return (long)span.TotalDays / 7;
                case DatePart.Hour: return (long)span.TotalHours;
                case DatePart.Minute: return (long)span.TotalMinutes;
                case DatePart.Second: return (long)span.TotalSeconds;
                case DatePart.Millisecond: return (long)span.TotalMilliseconds;
                case DatePart.TzOffset: throw new Exception(
                    $"{Name.ToUpper()}: The \"date-part\" argument must not be 'TZOFFSET'.");
                default: throw new Exception("Internal error: unrecognized DatePart.");
            }
        }

        private static DateTimeOffset TruncateDate(DatePart datePart, DateTimeOffset date) {
            switch (datePart) {
                case DatePart.Year: return new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, date.Offset);
                case DatePart.Quarter: return new DateTimeOffset(date.Year,
                    (DateUtil.GetQuarter(date) - 1) * 3 + 1, 1, 0, 0, 0, date.Offset);
                case DatePart.Month: return new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, date.Offset);
                case DatePart.DayOfYear: case DatePart.Day: case DatePart.DayOfWeek:
                    return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset);
                case DatePart.Week:
                    while (date.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) {
                        date = date.AddDays(-1);
                    }
                    return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset);
                case DatePart.Hour:
                    return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, 0, 0, date.Offset);
                case DatePart.Minute:
                    return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, date.Offset);
                case DatePart.Second:
                    return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second,
                        date.Offset);
                default:
                    return date;
            }
        }
    }

    public sealed class DateFromPartsFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datefromparts";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            if (args[0] is DBNull || args[1] is DBNull || args[2] is DBNull) {
                return DBNull.Value;
            }
            var year = ModUtil.GetInt32Arg(args[0], "year", Name);
            var month = ModUtil.GetInt32Arg(args[1], "month", Name);
            var day = ModUtil.GetInt32Arg(args[2], "day", Name);
            var date = new DateTime(year, month, day);
            return DateUtil.FormatDate(date);
        }
    }

    public sealed class DateNameFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datename";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ModUtil.GetDatePartArg(args[0], "date-part", Name);
            var date = ModUtil.GetDateArg(args[1], "date", Name);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            switch (datePart) {
                case DatePart.Month:
                    return dtf.GetMonthName(cal.GetMonth(date.DateTime));
                case DatePart.DayOfWeek:
                    return dtf.GetDayName(cal.GetDayOfWeek(date.DateTime));
                default:
                    return DateUtil.GetDatePart(date, datePart).ToString();
            }
        }
    }

    public sealed class DatePartFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datepart";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ModUtil.GetDatePartArg(args[0], "date-part", Name);
            var date = ModUtil.GetDateArg(args[1], "date", Name);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
            return DateUtil.GetDatePart(date, datePart);
        }
    }

    public sealed class DateTimeFromPartsFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datetimefromparts";
        public override int ParamCount => 7;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Take(ParamCount).OfType<DBNull>().Any()) {
                return DBNull.Value;
            }
            var year = ModUtil.GetInt32Arg(args[0], "year", Name);
            var month = ModUtil.GetInt32Arg(args[1], "month", Name);
            var day = ModUtil.GetInt32Arg(args[2], "day", Name);
            var hour = ModUtil.GetInt32Arg(args[3], "hour", Name);
            var minute = ModUtil.GetInt32Arg(args[4], "minute", Name);
            var second = ModUtil.GetInt32Arg(args[5], "second", Name);
            var msec = ModUtil.GetInt32Arg(args[6], "millisecond", Name);
            var date = new DateTime(year, month, day, hour, minute, second, msec);
            return DateUtil.FormatDateTime(date);
        }
    }

    public sealed class DateTimeOffsetFromPartsFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datetimeoffsetfromparts";
        public override int ParamCount => 9;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Take(ParamCount).OfType<DBNull>().Any()) {
                return DBNull.Value;
            }
            var year = ModUtil.GetInt32Arg(args[0], "year", Name);
            var month = ModUtil.GetInt32Arg(args[1], "month", Name);
            var day = ModUtil.GetInt32Arg(args[2], "day", Name);
            var hour = ModUtil.GetInt32Arg(args[3], "hour", Name);
            var minute = ModUtil.GetInt32Arg(args[4], "minute", Name);
            var second = ModUtil.GetInt32Arg(args[5], "second", Name);
            var msec = ModUtil.GetInt32Arg(args[6], "millisecond", Name);
            var hourOffset = ModUtil.GetInt32Arg(args[7], "hour-offset", Name);
            var minuteOffset = ModUtil.GetInt32Arg(args[8], "minute-offset", Name);
            var offset = new TimeSpan(hourOffset, minuteOffset, 0);
            var date = new DateTimeOffset(year, month, day, hour, minute, second, msec, offset);
            return DateUtil.FormatDateTimeOffset(date);
        }
    }

    public sealed class DayFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "day";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ModUtil.GetDateArg(args[0], "date", Name);
            return DateUtil.GetDatePart(date, DatePart.Day);
        }
    }

    public sealed class EoMonth1Function : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "eomonth";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ModUtil.GetDateArg(args[0], "start-date", Name);
            var lastDayOfMonth = new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month));
            return DateUtil.FormatDate(lastDayOfMonth);
        }
    }

    public sealed class EoMonth2Function : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "eomonth";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ModUtil.GetDateArg(args[0], "start-date", Name);
            var monthsToAdd = ModUtil.GetInt32Arg(args[1], "months-to-add", Name);
            date = date.AddMonths(monthsToAdd);
            var lastDayOfMonth = new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month));
            return DateUtil.FormatDate(lastDayOfMonth);
        }
    }

    public sealed class GetDateFunction : GenericSqliteFunction {
        public override bool IsDeterministic => false;
        public override string Name => "getdate";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args)
            => DateUtil.FormatDateTimeOffset(DateTimeOffset.Now);
    }

    public sealed class GetUtcDateFunction : GenericSqliteFunction {
        public override bool IsDeterministic => false;
        public override string Name => "getutcdate";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args)
            => DateUtil.FormatDateTimeOffset(DateTimeOffset.UtcNow);
    }

    public sealed class IsDateFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "isdate";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var arg = args[0];
            if (arg is DateTime || arg is DateTimeOffset) {
                return 1;
            } else if (arg is string) {
                DateTimeOffset dto;
                return DateTimeOffset.TryParse((string)arg, out dto) ? 1 : 0;
            } else {
                return 0;
            }
        }
    }

    public sealed class MonthFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "month";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ModUtil.GetDateArg(args[0], "date", Name);
            return DateUtil.GetDatePart(date, DatePart.Month);
        }
    }

    public sealed class SwitchOffsetFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "switchoffset";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ModUtil.GetDateArg(args[0], "date", Name);
            TimeSpan offset;
            if (args[1] is int || args[1] is long) {
                var minutes = ModUtil.GetInt32Arg(args[1], "time-zone", Name);
                offset = new TimeSpan(0, minutes, 0);
            } else {
                var offsetStr = ModUtil.GetStrArg(args[1], "time-zone", Name);
                var m = _tzOffsetRegex.Match(offsetStr);
                if (m.Success) {
                    var sign = m.Groups[1].Value == "+" ? 1 : -1;
                    var hour = int.Parse(m.Groups[2].Value);
                    var minute = int.Parse(m.Groups[3].Value);
                    offset = new TimeSpan(sign * hour, sign * minute, 0);
                } else {
                    throw new Exception($"{Name.ToUpper()}: \"{offsetStr}\" is not a valid time zone offset.");
                }
            }
            var newDate = new DateTimeOffset(date.DateTime, offset);
            return DateUtil.FormatDateTimeOffset(newDate);
        }
        private Regex _tzOffsetRegex = new Regex(@"^([+-]?)([0-9][0-9]?):([0-9][0-9]?)$");
    }

    public sealed class YearFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "year";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ModUtil.GetDateArg(args[0], "date", Name);
            return DateUtil.GetDatePart(date, DatePart.Year);
        }
    }

    public static class DateUtil {
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

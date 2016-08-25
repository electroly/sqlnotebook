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
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.ScalarFunctions {
    public sealed class DateAddFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "dateadd";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ArgUtil.GetDatePartArg(args[0], "date-part", Name);
            var number = ArgUtil.GetInt32Arg(args[1], "number", Name);
            var date = ArgUtil.GetDateArg(args[2], "date", Name);
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

            return DateTimeUtil.FormatDateTimeOffset(newDate);
        }
    }

    public sealed class DateDiffFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datediff";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ArgUtil.GetDatePartArg(args[0], "date-part", Name);
            var startDate = ArgUtil.GetDateArg(args[1], "start-date", Name);
            var endDate = ArgUtil.GetDateArg(args[2], "end-date", Name);

            // datediff() counts the number of calendar/clock boundaries crossed, which is different than
            // the amount of time between the two dates.  for instance, if startDate is one second before
            // midnight and endDate is one second after, that's one day boundary crossed.
            
            if (startDate == endDate) {
                return 0;
            }

            startDate = DateTimeUtil.TruncateDate(datePart, startDate);
            endDate = DateTimeUtil.TruncateDate(datePart, endDate);

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


    }

    public sealed class DateFromPartsFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datefromparts";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            if (args[0] is DBNull || args[1] is DBNull || args[2] is DBNull) {
                return DBNull.Value;
            }
            var year = ArgUtil.GetInt32Arg(args[0], "year", Name);
            var month = ArgUtil.GetInt32Arg(args[1], "month", Name);
            var day = ArgUtil.GetInt32Arg(args[2], "day", Name);
            var date = new DateTime(year, month, day);
            return DateTimeUtil.FormatDate(date);
        }
    }

    public sealed class DateNameFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datename";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ArgUtil.GetDatePartArg(args[0], "date-part", Name);
            var date = ArgUtil.GetDateArg(args[1], "date", Name);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            switch (datePart) {
                case DatePart.Month:
                    return dtf.GetMonthName(cal.GetMonth(date.DateTime));
                case DatePart.DayOfWeek:
                    return dtf.GetDayName(cal.GetDayOfWeek(date.DateTime));
                case DatePart.TzOffset:
                    return $"{date.Offset.Hours:+00;-00}:{Math.Abs(date.Offset.Minutes):00}";
                default:
                    return DateTimeUtil.GetDatePart(date, datePart).ToString();
            }
        }
    }

    public sealed class DatePartFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datepart";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ArgUtil.GetDatePartArg(args[0], "date-part", Name);
            var date = ArgUtil.GetDateArg(args[1], "date", Name);
            return DateTimeUtil.GetDatePart(date, datePart);
        }
    }

    public sealed class DateTruncFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "date_trunc";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var datePart = ArgUtil.GetDatePartArg(args[0], "date-part", Name);
            var date = ArgUtil.GetDateArg(args[1], "date", Name);
            return DateTimeUtil.FormatDateTimeOffset(DateTimeUtil.TruncateDate(datePart, date));
        }
    }

    public sealed class DateTimeFromPartsFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datetimefromparts";
        public override int ParamCount => 7;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Take(ParamCount).OfType<DBNull>().Any()) {
                return DBNull.Value;
            }
            var year = ArgUtil.GetInt32Arg(args[0], "year", Name);
            var month = ArgUtil.GetInt32Arg(args[1], "month", Name);
            var day = ArgUtil.GetInt32Arg(args[2], "day", Name);
            var hour = ArgUtil.GetInt32Arg(args[3], "hour", Name);
            var minute = ArgUtil.GetInt32Arg(args[4], "minute", Name);
            var second = ArgUtil.GetInt32Arg(args[5], "second", Name);
            var msec = ArgUtil.GetInt32Arg(args[6], "millisecond", Name);
            var date = new DateTime(year, month, day, hour, minute, second, msec);
            return DateTimeUtil.FormatDateTime(date);
        }
    }

    public sealed class DateTimeOffsetFromPartsFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "datetimeoffsetfromparts";
        public override int ParamCount => 9;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Take(ParamCount).OfType<DBNull>().Any()) {
                return DBNull.Value;
            }
            var year = ArgUtil.GetInt32Arg(args[0], "year", Name);
            var month = ArgUtil.GetInt32Arg(args[1], "month", Name);
            var day = ArgUtil.GetInt32Arg(args[2], "day", Name);
            var hour = ArgUtil.GetInt32Arg(args[3], "hour", Name);
            var minute = ArgUtil.GetInt32Arg(args[4], "minute", Name);
            var second = ArgUtil.GetInt32Arg(args[5], "second", Name);
            var msec = ArgUtil.GetInt32Arg(args[6], "millisecond", Name);
            var hourOffset = ArgUtil.GetInt32Arg(args[7], "hour-offset", Name);
            var minuteOffset = ArgUtil.GetInt32Arg(args[8], "minute-offset", Name);
            var offset = new TimeSpan(hourOffset, minuteOffset, 0);
            var date = new DateTimeOffset(year, month, day, hour, minute, second, msec, offset);
            return DateTimeUtil.FormatDateTimeOffset(date);
        }
    }

    public sealed class DayFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "day";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ArgUtil.GetDateArg(args[0], "date", Name);
            return DateTimeUtil.GetDatePart(date, DatePart.Day);
        }
    }

    public sealed class EoMonth1Function : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "eomonth";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ArgUtil.GetDateArg(args[0], "start-date", Name);
            var lastDayOfMonth = new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month));
            return DateTimeUtil.FormatDate(lastDayOfMonth);
        }
    }

    public sealed class EoMonth2Function : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "eomonth";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ArgUtil.GetDateArg(args[0], "start-date", Name);
            var monthsToAdd = ArgUtil.GetInt32Arg(args[1], "months-to-add", Name);
            date = date.AddMonths(monthsToAdd);
            var lastDayOfMonth = new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month));
            return DateTimeUtil.FormatDate(lastDayOfMonth);
        }
    }

    public sealed class GetDateFunction : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "getdate";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args)
            => DateTimeUtil.FormatDateTimeOffset(DateTimeOffset.Now);
    }

    public sealed class NowFunction : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "now";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args)
            => DateTimeUtil.FormatDateTimeOffset(DateTimeOffset.Now);
    }

    public sealed class GetUtcDateFunction : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "getutcdate";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args)
            => DateTimeUtil.FormatDateTimeOffset(DateTimeOffset.UtcNow);
    }

    public sealed class IsDateFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "isdate";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var arg = args[0].ToString();
            DateTimeOffset dto;
            return DateTimeOffset.TryParse(arg, out dto) ? 1 : 0;
        }
    }

    public sealed class MonthFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "month";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ArgUtil.GetDateArg(args[0], "date", Name);
            return DateTimeUtil.GetDatePart(date, DatePart.Month);
        }
    }

    public sealed class SwitchOffsetFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "switchoffset";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ArgUtil.GetDateArg(args[0], "date", Name);
            TimeSpan offset;
            if (args[1] is int || args[1] is long) {
                var minutes = ArgUtil.GetInt32Arg(args[1], "time-zone", Name);
                offset = new TimeSpan(0, minutes, 0);
            } else {
                var offsetStr = ArgUtil.GetStrArg(args[1], "time-zone", Name);
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
            return DateTimeUtil.FormatDateTimeOffset(newDate);
        }
        private Regex _tzOffsetRegex = new Regex(@"^([+-])([0-9][0-9]?):([0-9][0-9]?)$");
    }

    public sealed class YearFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "year";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var date = ArgUtil.GetDateArg(args[0], "date", Name);
            return DateTimeUtil.GetDatePart(date, DatePart.Year);
        }
    }

    public sealed class ToDateFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "to_date";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var input = ArgUtil.GetDateArg(args[0], "input", Name);
            return DateTimeUtil.FormatDate(input.DateTime);
        }
    }

    public sealed class ToDateTimeFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "to_datetime";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var input = ArgUtil.GetDateArg(args[0], "input", Name);
            return DateTimeUtil.FormatDateTime(input.DateTime);
        }
    }

    public sealed class ToDateTimeOffsetFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "to_datetimeoffset";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var input = ArgUtil.GetDateArg(args[0], "input", Name);
            return DateTimeUtil.FormatDateTimeOffset(input);
        }
    }
}

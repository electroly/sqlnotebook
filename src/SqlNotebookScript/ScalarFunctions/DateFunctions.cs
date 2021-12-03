using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.ScalarFunctions;

public sealed class DateAddFunction : CustomScalarFunction {
    public override bool IsDeterministic => true;
    public override string Name => "dateadd";
    public override int ParamCount => 3;
    public override object Execute(IReadOnlyList<object> args) {
        var datePart = ArgUtil.GetDatePartArg(args[0], "date-part", Name);
        var number = ArgUtil.GetInt32Arg(args[1], "number", Name);
        var date = ArgUtil.GetDateArg(args[2], "date", Name);
        var newDate = datePart switch {
            DatePart.Year => date.AddYears(number),
            DatePart.Quarter => date.AddMonths(number * 3),
            DatePart.Month => date.AddMonths(number),
            DatePart.DayOfYear or DatePart.Day or DatePart.DayOfWeek => date.AddDays(number),
            DatePart.Week => date.AddDays(7 * number),
            DatePart.Hour => date.AddHours(number),
            DatePart.Minute => date.AddMinutes(number),
            DatePart.Second => date.AddSeconds(number),
            DatePart.Millisecond => date.AddMilliseconds(number),
            _ => throw new Exception($"Internal error: unrecognized DatePart."),
        };
        return DateTimeUtil.FormatDateTime(newDate);
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
                return dtf.GetMonthName(cal.GetMonth(date));
            case DatePart.DayOfWeek:
                return dtf.GetDayName(cal.GetDayOfWeek(date));
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
        return DateTimeUtil.FormatDateTime(DateTimeUtil.TruncateDate(datePart, date));
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
        => DateTimeUtil.FormatDateTime(DateTime.Now);
}

public sealed class NowFunction : CustomScalarFunction {
    public override bool IsDeterministic => false;
    public override string Name => "now";
    public override int ParamCount => 0;
    public override object Execute(IReadOnlyList<object> args)
        => DateTimeUtil.FormatDateTime(DateTime.Now);
}

public sealed class GetUtcDateFunction : CustomScalarFunction {
    public override bool IsDeterministic => false;
    public override string Name => "getutcdate";
    public override int ParamCount => 0;
    public override object Execute(IReadOnlyList<object> args)
        => DateTimeUtil.FormatDateTime(DateTime.UtcNow);
}

public sealed class IsDateFunction : CustomScalarFunction {
    public override bool IsDeterministic => true;
    public override string Name => "isdate";
    public override int ParamCount => 1;
    public override object Execute(IReadOnlyList<object> args) {
        var arg = args[0].ToString();
        return DateTime.TryParse(arg, out _) ? 1 : 0;
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
        return DateTimeUtil.FormatDate(input);
    }
}

public sealed class ToDateTimeFunction : CustomScalarFunction {
    public override bool IsDeterministic => true;
    public override string Name => "to_datetime";
    public override int ParamCount => 1;
    public override object Execute(IReadOnlyList<object> args) {
        var input = ArgUtil.GetDateArg(args[0], "input", Name);
        return DateTimeUtil.FormatDateTime(input);
    }
}

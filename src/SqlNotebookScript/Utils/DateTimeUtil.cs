using System;
using System.Globalization;

namespace SqlNotebookScript.Utils;

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

    public static DateTimeOffset TruncateDate(DatePart datePart, DateTimeOffset date) {
        switch (datePart) {
            case DatePart.Year: return new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, date.Offset);
            case DatePart.Quarter: return new DateTimeOffset(date.Year,
                (DateTimeUtil.GetQuarter(date) - 1) * 3 + 1, 1, 0, 0, 0, date.Offset);
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

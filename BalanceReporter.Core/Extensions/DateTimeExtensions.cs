using System;

namespace BalanceReporter.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime TrimDateTimeToYearMonthDay(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day);
        }
    }
}

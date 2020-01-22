using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using BalanceReporter.Core.Extensions;
using BalanceReporter.Core.Models;

namespace BalanceReporter.Data
{
    public class BalanceReport : Report
    {
        public class EndOfTheDayBalance
        {
            public DateTime Date { get; set; }
            public decimal Balance { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as EndOfTheDayBalance);
            }

            public bool Equals(EndOfTheDayBalance endOfTheDayBalance)
            {
                if (endOfTheDayBalance == null) return false;
                return Date.TrimDateTimeToYearMonthDay() == endOfTheDayBalance.Date.TrimDateTimeToYearMonthDay();
            }

            public override int GetHashCode()
            {
                return Date.TrimDateTimeToYearMonthDay().GetHashCode();
            }
            public override string ToString()
            {
                return $"{Date} : {Balance}";
            }
        }
        [JsonProperty("TotalCredits")]
        public decimal TotalCredits { get; set; }
        [JsonProperty("TotalDebits")]
        public decimal TotalDebits { get; set; }
        [JsonProperty("EndOfDayBalances")]
        public IList<EndOfTheDayBalance> EndOfDayBalances { get; set; }
    }
}

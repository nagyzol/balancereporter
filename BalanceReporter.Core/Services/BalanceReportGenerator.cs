﻿using BalanceReporter.Core.Extensions;
using BalanceReporter.Data;
using System;
using System.Linq;

namespace BalanceReporter.Core.Services
{
    public class BalanceReportGenerator : IBalanceReportGenerator
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public BalanceReport Generate(Account account, DateTime requestDate)
        {
            if (account == null)
            {
                Logger.Error("Account can't be null");
                throw new ArgumentNullException(nameof(account));
            }

            var tempBalance = account.Current.Amount;
            decimal totalCredit = 0;
            decimal totalDebit = 0;

            var balances = account.Transactions
                .Where(t => t.Status == Transaction.State.Booked && t.BookingDate.TrimDateTimeToYearMonthDay() <= requestDate)
                .GroupBy(t => t.BookingDate.TrimDateTimeToYearMonthDay())
                .OrderByDescending(t => t.Key)
                .Select(t =>
                    {
                        var balance = new BalanceReport.EndOfTheDayBalance
                        {
                            Date = t.Key,
                            Balance = (decimal)t.Sum(a =>
                                        {
                                            switch (a.CreditDebitIndicator)
                                            {
                                                case CreditDebitIndicator.Credit:
                                                    if (t.Key < requestDate.TrimDateTimeToYearMonthDay()) totalCredit += (decimal)a.Amount;
                                                    return -a.Amount;
                                                case CreditDebitIndicator.Debit:
                                                    if (t.Key < requestDate.TrimDateTimeToYearMonthDay()) totalDebit += (decimal)a.Amount;
                                                    return a.Amount;
                                                default:
                                                    Logger.Error($"Invalid CreditDebitIndicator: {a.CreditDebitIndicator} at {a}");
                                                    throw new InvalidOperationException($"Invalid CreditDebitIndicator: {a.CreditDebitIndicator} at {a}");
                                            }
                                        })
                        };
                        (balance.Balance, tempBalance) = (tempBalance, tempBalance + balance.Balance);
                        return balance;
                    }
                )
                .Where(b => b.Date != requestDate.TrimDateTimeToYearMonthDay())
                .ToList();
                
            return new BalanceReport { TotalCredits = totalCredit, TotalDebits = totalDebit, EndOfDayBalances = balances };
        }
    }
}
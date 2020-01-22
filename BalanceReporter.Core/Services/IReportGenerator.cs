using BalanceReporter.Core.Models;
using BalanceReporter.Data;
using System;

namespace BalanceReporter.Core.Services
{
    public interface IReportGenerator< out TResult>
        where TResult : Report
    {
        TResult Generate(Account account, DateTime requestDate);

    }
}
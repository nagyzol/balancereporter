using BalanceReporter.Core.Models;
using BalanceReporter.Data;

namespace BalanceReporter.Core.Services
{
    public interface IReporterService<TRequest, TResult>
        where TRequest : ReportRequest
        where TResult : Report
    {
        TResult GetFromFile(string filePath);
        TResult GetFromJson(string jsonInput);
    }
}

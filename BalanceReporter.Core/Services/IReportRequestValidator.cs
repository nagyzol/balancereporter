using BalanceReporter.Data;

namespace BalanceReporter.Core.Services
{
    public interface IReportRequestValidator<TRequest>
        where TRequest : ReportRequest
    {
        bool IsValid(TRequest request, out string reason);
    }
}

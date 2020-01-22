using BalanceReporter.Data;
using System.Linq;

namespace BalanceReporter.Core.Services
{
    public class AccountBalanceRequestValidator : IReportRequestValidator<AccountBalanceRequest>
    {
        public bool IsValid(AccountBalanceRequest request, out string reason)
        {
            if (request == null )
            {
                reason = "Request can't be null";
                return false;
            }
            if (request.Accounts.Select(a => a.CurrencyCode).Count() > 1)
            {
                reason = "Accounts using different currency.";
                return false;
            }

            reason = null;
            return true;
        }
    }
}

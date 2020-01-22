using System;
using System.Linq;

using BalanceReporter.Core.Parsers;
using BalanceReporter.Data;

namespace BalanceReporter.Core.Services
{
    public class BalanceReporterService : IReporterService<AccountBalanceRequest, BalanceReport>
    {
        private readonly IAccountRequestParser accountRequestParser;
        private readonly IReportGenerator<BalanceReport> reportGenerator;
        private readonly IReportRequestValidator<AccountBalanceRequest> validator;

        public BalanceReporterService(IAccountRequestParser accountRequestParser, IReportGenerator<BalanceReport> reportGenerator,
            IReportRequestValidator<AccountBalanceRequest> validator)
        {
            this.accountRequestParser = accountRequestParser;
            this.reportGenerator = reportGenerator;
            this.validator = validator;
        }

        public BalanceReport GetFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                //todo: log
                throw new ArgumentNullException(nameof(filePath));
            }
            var request = accountRequestParser.GetAccountBalanceRequestFromFile(filePath);
            return GetFromRequest(request);
        }

        public BalanceReport GetFromJson(string jsonInput)
        {
            if (string.IsNullOrWhiteSpace(jsonInput))
            {
                //todo: log
                throw new ArgumentNullException(nameof(jsonInput));
            }

            var request = accountRequestParser.GetAccountBalanceRequestFromString(jsonInput);
            return GetFromRequest(request);
        }

        private BalanceReport GetFromRequest(AccountBalanceRequest request)
        {
            if (!validator.IsValid(request, out string reason))
            {
                //todo log
                throw new InvalidOperationException($"Can't generate report from {request} because of the following reason: {reason}");
            }

            return reportGenerator.Generate(request.Accounts.SingleOrDefault(), request.RequestDate);
        }
    }
}

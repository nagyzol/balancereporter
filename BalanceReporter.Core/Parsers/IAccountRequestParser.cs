using BalanceReporter.Data;

namespace BalanceReporter.Core.Parsers
{
    public interface IAccountRequestParser
    {
        AccountBalanceRequest GetAccountBalanceRequestFromFile(string filePath);
        AccountBalanceRequest GetAccountBalanceRequestFromString(string inputJson);
    }
}
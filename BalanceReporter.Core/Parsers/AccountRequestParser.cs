using System;
using System.IO;

using BalanceReporter.Data;
using Newtonsoft.Json;

namespace BalanceReporter.Core.Parsers
{
    public class AccountRequestParser : IAccountRequestParser
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public AccountBalanceRequest GetAccountBalanceRequestFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Logger.Error("File path is null");
                throw new ArgumentNullException(nameof(filePath));
            }
            if (!File.Exists(filePath))
            {
                Logger.Error($"File doesn't exists {filePath}");
                throw new ArgumentException($"File doesn't exists {filePath}");
            }

            try
            {
                using (StreamReader reader = File.OpenText(filePath))
                {
                    var serializer = new JsonSerializer();
                    return (AccountBalanceRequest)serializer.Deserialize(reader, typeof(AccountBalanceRequest));
                }
            }
            catch(Exception e)
            {
                Logger.Error(e, $"Can't read file {filePath}");
                throw new InvalidOperationException($"Can't read file {filePath}", e);
            }
        }

        public AccountBalanceRequest GetAccountBalanceRequestFromString(string inputJson)
        {
            if (string.IsNullOrWhiteSpace(inputJson)) throw new ArgumentNullException(nameof(inputJson));
            try
            {
                return JsonConvert.DeserializeObject<AccountBalanceRequest>(inputJson);
            }
            catch(Exception e)
            {
                Logger.Error(e, $"Can't parse json: {inputJson}");
                throw new InvalidOperationException($"Can't parse json: {inputJson}", e);
            }
        }
    }
}

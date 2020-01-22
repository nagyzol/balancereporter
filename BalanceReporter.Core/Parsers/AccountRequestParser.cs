using BalanceReporter.Data;
using Newtonsoft.Json;
using System;
using System.IO;

namespace BalanceReporter.Core.Parsers
{
    public class AccountRequestParser : IAccountRequestParser
    {
        public AccountBalanceRequest GetAccountBalanceRequestFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                //todo: log
                throw new ArgumentNullException(nameof(filePath));
            }
            if (!File.Exists(filePath))
            {
                //todo: log
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
                //todo: log
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
                //todo: log exception
                throw new InvalidOperationException($"Can't parse json: {inputJson}", e);
            }
        }
    }
}

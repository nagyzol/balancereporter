using BalanceReporter.Data.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BalanceReporter.Data
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Account
    {
        [JsonProperty("accountId")]
        public string Id { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("balances.current")]
        public Balance Current { get; set; }
        [JsonProperty("balances.available")]
        public Balance Available { get; set; }
        [JsonProperty("transactions")]
        public IList<Transaction> Transactions { get; set; }
    }
}

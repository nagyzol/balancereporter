using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BalanceReporter.Data
{
    public class AccountBalanceRequest : ReportRequest
    {
        [JsonProperty("brandName")]
        public string BrandName { get; set; }
        [JsonProperty("dataSourceName")]
        public string DataSourceName { get; set; }
        [JsonProperty("requestDateTime")]
        public DateTime RequestDate { get; set; }
        [JsonProperty("accounts")]
        public IList<Account> Accounts { get; set; }
    }
}

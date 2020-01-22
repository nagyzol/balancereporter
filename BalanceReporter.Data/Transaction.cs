using Newtonsoft.Json;
using System;

namespace BalanceReporter.Data
{
    public class Transaction
    {
        public enum State
        {
            Booked,
            Other //I assume there are more than 1 states 
        }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("amount")]
        public float Amount { get; set; } //I guess to amount can't be negative
        [JsonProperty("creditDebitIndicator")]
        public CreditDebitIndicator CreditDebitIndicator { get; set; }
        [JsonProperty("status")]
        public State Status { get; set; }
        [JsonProperty("bookingDate")]
        public DateTime BookingDate { get; set; }
    }
}

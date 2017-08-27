using Newtonsoft.Json;

namespace JPesa.NET.Models
{
    public class Transaction
    {
        /// <summary>
        /// Internal JPesa id of transaction.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The total amount requested in the transaction. NB: This is the 
        /// total sum without regard to fees. To get the actual sum that
        /// is received or deposited, make sure to subtract the fees.
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The fee charged by JPesa for this transaction.
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        /// <summary>
        /// The three letter currency code for the amount and fees in this
        /// transaction.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The status of this transaction.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The identifier for the mobile network from/to which the mobile
        /// money was withdrawn/deposited.
        /// </summary>
        [JsonProperty("pp")]
        public string MobileNetwork { get; set; }

        /// <summary>
        /// The phone number of the mobile wallet from/to which the mobile
        /// money was deposited.
        /// </summary>
        [JsonProperty("number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The public id of the transaction. Can be used to track the status
        /// of a transaction via Instant Payment Notification.
        /// </summary>
        [JsonProperty("tid")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Unix timestamp in seconds of when the transaction was initiated.
        /// </summary>
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }
    }
}

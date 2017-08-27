using Flurl;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using JPesa.NET.Models;
using Newtonsoft.Json;

namespace JPesa.NET
{
    public class JpesaClient
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public JpesaClient()
        {
            
        }

        /// <summary>
        /// Initialize a new Jpesa Client with the specified user name and
        /// password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public JpesaClient(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(userName, "The user name cannot be empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(password, "The password cannot be empty");
            }
            Password = password;
            UserName = userName;
        }

        public string Password { get; set; }
        public string UserName { get; set; }
        protected string JpesaUri { get; set; } = "https://secure.jpesa.com/api.php";

        /// <summary>
        /// Check the status of a previous pull or push transaction with the
        /// specified transaction id.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<Transaction> QueryTransaction(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                throw new ArgumentNullException(transactionId, "The transaction ID cannot be null");
            }

            var url = JpesaUri.SetQueryParams(new
            {
                action="info",
                command="jpesa",
                IS_GET = "3",
                password=Password,
                tid=transactionId,
                username = UserName,
            });
            
            Console.WriteLine(url.ToString());
            string response;
            using (var httpClient = new HttpClient())
            {
                response = await httpClient.GetStringAsync(url.ToString());
            }
            if (response.Contains("SUCCESS"))
            {
                var result =
                    JsonConvert.DeserializeObject<Transaction>(
                        response.Substring(response.IndexOf("{", StringComparison.Ordinal)));
                return result;
            }
            else
            {
                var result = response.Substring(response.IndexOf(" ", StringComparison.Ordinal) + 1);
                throw new Exception(result);
            }
        }

        /// <summary>
        /// This method informs JPesa to pull the specified amount of mobile 
        /// money from the specified phone number into the account 
        /// associated with this JpesaClient. Check the status of the 
        /// transaction with the QueryTransaction method.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>The id of the created trasanction</returns>
        public async Task<string> PullMobileMoney(decimal amount, string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentNullException(phoneNumber, "The transaction ID cannot be null");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    amount.ToString(CultureInfo.InvariantCulture), "The amount must be greater than zero");
            }

            var url = JpesaUri.SetQueryParams(new
            {
                action = "deposit",
                amount,
                command = "jpesa",
                IS_GET = "3",
                number = phoneNumber,
                password = Password,
                username = UserName,
            });

            string response;
            using (var httpClient = new HttpClient())
            {
                response = await httpClient.GetStringAsync(url.ToString());
            }

            if (response.Contains("[SUCCESS]"))
            {
                var txnId = response.Substring(response.IndexOf("|", StringComparison.Ordinal) + 1);
                return txnId;
            }
            throw new Exception(response.Substring(response.IndexOf(" ", StringComparison.Ordinal) + 1));
        }

        ///// <summary>
        ///// Attempts to send the specified amount of mobile moeny from the
        ///// account associated with this JpesaClient to the specified phone
        ///// number.
        ///// </summary>
        ///// <param name="amount"></param>
        ///// <param name="phoneNumber"></param>
        //public void SendMobileMoney(decimal amount, string phoneNumber)
        //{
        //}
    }
}

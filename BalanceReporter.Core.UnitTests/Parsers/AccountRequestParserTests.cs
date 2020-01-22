using BalanceReporter.Core.Parsers;
using BalanceReporter.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BalanceReporter.Core.UnitTests.Parsers
{
    [TestFixture]
    public class AccountRequestParserTests
    {
        IAccountRequestParser parser;

        [SetUp]
        public void Setup()
        {
            parser = new AccountRequestParser();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [TestCase(@"x:\doesntexistfile.json")]
        public void GetAccountBalanceRequestFromFile_WhenFileDoesntExist_ThrowsArgumentException(string filePath)
        {
            TestDelegate act = () => parser.GetAccountBalanceRequestFromFile(filePath);
            
            Assert.That(act, Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"File doesn't exists {filePath}"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void GetAccountBalanceRequestFromFile_WhenInputIsNullOrWhiteSpace_ThrowsArgumentNullException(string filePath)
        {
            TestDelegate act = () => parser.GetAccountBalanceRequestFromFile(filePath);

            Assert.That(act, Throws.Exception.InstanceOf<ArgumentNullException>());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void GetAccountRequestFromString_WhenInputIsNullOrWhiteSpace_ThrowsArgumentNullException(string inputJson)
        {
            TestDelegate act = () => parser.GetAccountBalanceRequestFromString(inputJson);

            Assert.That(act, Throws.Exception.InstanceOf<ArgumentNullException>());
        }

        [TestCase("- brandName: TestBank - accounts: - accountId: 10473770 currencyCode: GBP")]
        [TestCase("<balanceRequest><brandName>TestBank</brandName><accounts><account><accountId>10473770<accountId><currencyCode>currencyCode</currencyCode></account></accounts></balanceRequest> ")]
        public void GetAccountRequestFromString_WhenInputIsNullOrInvalidJson_ReturnsNull(string inputJson)
        {
            TestDelegate act = () => parser.GetAccountBalanceRequestFromString(inputJson);

            Assert.That(act, Throws.Exception.With.Message.Contains($"Can't parse json: {inputJson}"));
        }

        [Test, TestCaseSource(nameof(AccountRequestJsonCases))]
        public void GetAccountRequestFromString_WhenInputIsGood_ReturnsAccountRequest(string input, AccountBalanceRequest expected)
        {
            var result = parser.GetAccountBalanceRequestFromString(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.BrandName, Is.EqualTo(expected.BrandName));
            Assert.That(result.DataSourceName, Is.EqualTo(expected.DataSourceName));
            Assert.That(result.RequestDate, Is.EqualTo(expected.RequestDate));
            Assert.That(result.Accounts.Count, Is.EqualTo(expected.Accounts.Count));
            //...
        }

        #region TestCaseSources

        static object[] AccountRequestJsonCases =
        {
            new object[] {"{" +
                "\"brandName\": \"TestBank\"," +
                "\"dataSourceName\": \"TestDataSource\"," +
                "\"dataSourceType\": \"CredentialSharing\"," +
                "\"requestDateTime\": \"2019-04-13T14:08:12Z\"," +
                "\"accounts\": [" +
                "{" +
                "\"accountId\": \"10473770\"," +
                "\"currencyCode\": \"GBP\"," +
                "\"displayName\": \"Current Account\"," +
                "\"accountType\": \"Personal\"," +
                "\"accountSubType\": \"CurrentAccount\"," +
                "\"identifiers\": {" +
                "\"sortCode\": \"122101706\"," +
                "\"accountNumber\": \"xxxx1484\"," +
                "\"secondaryIdentification\": null" +
                "}," +
                "\"parties\": []," +
                "\"standingOrders\": []," +
                "\"directDebits\": []," +
                "\"balances\": {" +
                "\"current\": {" +
                "\"amount\": 100," +
                "\"creditDebitIndicator\": \"Credit\"," +
                "\"creditLines\": null" +
                "}," +
                "\"available\": {" +
                "\"amount\": 100," +
                "\"creditDebitIndicator\": \"Credit\"," +
                "\"creditLines\": null" +
                "}" +
                "}," +
                "\"transactions\": [" +
                "{" +
                "\"description\": \"MIICARD LTD , MIICARD LTD , FP 31/07/14 1409 , XXXXXXXXXXXXXXXX01 - BAC\"," +
                "\"amount\": 57," +
                "\"creditDebitIndicator\": \"Credit\"," +
                "\"status\": \"Booked\"," +
                "\"bookingDate\": \"2019-04-12T00:00:00Z\"," +
                "\"merchantDetails\": null" +
                "}" +
                "]" + //End of transactions
                "}" +
                "]" + //End of accounts
                "}",
            new AccountBalanceRequest
            {
                BrandName = "TestBank",
                DataSourceName = "TestDataSource",
                RequestDate = new DateTime(2019,4,13,14,8,12), //2019-04-13T14:08:12Z\
                Accounts = new List<Account>
                {
                    new Account
                    {
                        DisplayName = "Current Account",
                        Current = new Balance
                        {
                            Amount = 100,
                            CreditDebitIndicator = CreditDebitIndicator.Credit
                        },
                        Available = new Balance
                        {
                            Amount = 100,
                            CreditDebitIndicator = CreditDebitIndicator.Debit
                        },
                        Transactions = new List<Transaction>
                        {
                            new Transaction
                            {
                                Description = "MIICARD LTD , MIICARD LTD , FP 31/07/14 1409 , XXXXXXXXXXXXXXXX01 - BAC",
                                Amount = 57,
                                CreditDebitIndicator = CreditDebitIndicator.Credit,
                                Status = Transaction.State.Booked,
                                BookingDate = new DateTime(2019, 4, 12)
                            }
                        }
                    }
                }
            }},
        };

        #endregion
    }
}

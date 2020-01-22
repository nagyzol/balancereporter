using System.Collections.Generic;

using BalanceReporter.Core.Services;
using BalanceReporter.Data;
using NUnit.Framework;

namespace BalanceReporter.Core.UnitTests.Services
{
    [TestFixture]
    public class AccountBalanceRequestValidatorTests
    {
        private IReportRequestValidator<AccountBalanceRequest> validator;

        [SetUp]
        public void Setup()
        {
            validator = new AccountBalanceRequestValidator();
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void IsValid_WhenRequestIsNull_ReturnsFalse()
        {
            var result = validator.IsValid(null, out string reason);

            Assert.That(result, Is.False);
            Assert.That(reason, Is.EqualTo("Request can't be null"));
        }

        [Test]
        public void IsValid_WhenRequestContainsMultipleCurrencies_ReturnsFalse()
        {
            var request = new AccountBalanceRequest
            {
                Accounts = new List<Account>
                {
                    new Account {CurrencyCode ="GBP"},
                    new Account {CurrencyCode ="HUF"},
                }
            };

            var result = validator.IsValid(request, out string reason);

            Assert.That(result, Is.False);
            Assert.That(reason, Is.EqualTo("Accounts using different currency."));
        }

        [Test]
        public void IsValid_WhenRequestIsValid_ReturnsTrue()
        {
            var request = new AccountBalanceRequest
            {
                Accounts = new List<Account>
                {
                    new Account {CurrencyCode ="GBP"},
                }
            };

            var result = validator.IsValid(request, out string reason);

            Assert.That(result, Is.True);
            Assert.That(reason, Is.Null);
        }
    }
}

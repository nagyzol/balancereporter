using System;
using System.IO;
using BalanceReporter.Core.Parsers;
using BalanceReporter.Core.Services;
using BalanceReporter.Data;
using NUnit.Framework;

namespace BalanceReporter.Core.IntegrationTests.Services
{
    [TestFixture]
    public class BalanceReporterServiceTests
    {
        private IReporterService<AccountBalanceRequest, BalanceReport> balanceReporterService;
        private IAccountRequestParser accountRequestParser;
        private IReportGenerator<BalanceReport> reportGenerator;
        private IReportRequestValidator<AccountBalanceRequest> validator;

        [SetUp]
        public void Setup()
        {
            validator = new AccountBalanceRequestValidator();
            reportGenerator = new BalanceReportGenerator();
            accountRequestParser = new AccountRequestParser();
            balanceReporterService = new BalanceReporterService(accountRequestParser, reportGenerator, validator);
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void GetFromFile_ShouldReturnEndOfDayBalanceReport()
        {
            var result = balanceReporterService.GetFromFile("Account.json");

            Assert.That(result.EndOfDayBalances.Count, Is.EqualTo(6));
            Assert.That(result.EndOfDayBalances[0].Balance, Is.EqualTo(100));
            Assert.That(result.EndOfDayBalances[0].Date, Is.EqualTo(new DateTime(2019,4,12)));
            Assert.That(result.EndOfDayBalances[1].Balance, Is.EqualTo(95));
            Assert.That(result.EndOfDayBalances[1].Date, Is.EqualTo(new DateTime(2019, 4, 11)));
            Assert.That(result.EndOfDayBalances[2].Balance, Is.EqualTo(176));
            Assert.That(result.EndOfDayBalances[2].Date, Is.EqualTo(new DateTime(2019, 4, 10)));
            Assert.That(result.EndOfDayBalances[3].Balance, Is.EqualTo(348));
            Assert.That(result.EndOfDayBalances[3].Date, Is.EqualTo(new DateTime(2019, 4, 9)));
            Assert.That(result.EndOfDayBalances[4].Balance, Is.EqualTo(398));
            Assert.That(result.EndOfDayBalances[4].Date, Is.EqualTo(new DateTime(2019, 4, 8)));
            Assert.That(result.EndOfDayBalances[5].Balance, Is.EqualTo(421));
            Assert.That(result.EndOfDayBalances[5].Date, Is.EqualTo(new DateTime(2019, 4, 7)));

            Assert.That(result.TotalCredits, Is.EqualTo(228));
            Assert.That(result.TotalDebits, Is.EqualTo(456));
        }

        [Test]
        public void GetFromString_ShouldReturnEndOfDayBalanceReport()
        {
            var jsonString = File.ReadAllText("Account.json");
            var result = balanceReporterService.GetFromJson(jsonString);

            Assert.That(result.EndOfDayBalances.Count, Is.EqualTo(6));
            Assert.That(result.EndOfDayBalances[0].Balance, Is.EqualTo(100));
            Assert.That(result.EndOfDayBalances[0].Date, Is.EqualTo(new DateTime(2019, 4, 12)));
            Assert.That(result.EndOfDayBalances[1].Balance, Is.EqualTo(95));
            Assert.That(result.EndOfDayBalances[1].Date, Is.EqualTo(new DateTime(2019, 4, 11)));
            Assert.That(result.EndOfDayBalances[2].Balance, Is.EqualTo(176));
            Assert.That(result.EndOfDayBalances[2].Date, Is.EqualTo(new DateTime(2019, 4, 10)));
            Assert.That(result.EndOfDayBalances[3].Balance, Is.EqualTo(348));
            Assert.That(result.EndOfDayBalances[3].Date, Is.EqualTo(new DateTime(2019, 4, 9)));
            Assert.That(result.EndOfDayBalances[4].Balance, Is.EqualTo(398));
            Assert.That(result.EndOfDayBalances[4].Date, Is.EqualTo(new DateTime(2019, 4, 8)));
            Assert.That(result.EndOfDayBalances[5].Balance, Is.EqualTo(421));
            Assert.That(result.EndOfDayBalances[5].Date, Is.EqualTo(new DateTime(2019, 4, 7)));

            Assert.That(result.TotalCredits, Is.EqualTo(228));
            Assert.That(result.TotalDebits, Is.EqualTo(456));
        }
    }
}

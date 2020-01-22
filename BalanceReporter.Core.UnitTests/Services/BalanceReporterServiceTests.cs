using System;
using System.Collections.Generic;

using BalanceReporter.Core.Parsers;
using BalanceReporter.Core.Services;
using BalanceReporter.Data;
using Moq;
using NUnit.Framework;
namespace BalanceReporter.Core.UnitTests.Services
{
    [TestFixture]
    public class BalanceReporterServiceTests
    {
        private IReporterService<AccountBalanceRequest, BalanceReport> service;
        private Mock<IAccountRequestParser> accountRequestParser;
        private Mock<IReportGenerator<BalanceReport>> reportGenerator;
        private Mock<IReportRequestValidator<AccountBalanceRequest>> validator;

        [SetUp]
        public void Setup()
        {
            accountRequestParser = new Mock<IAccountRequestParser>();
            reportGenerator = new Mock<IReportGenerator<BalanceReport>>();
            validator = new Mock<IReportRequestValidator<AccountBalanceRequest>>();

            service = new BalanceReporterService(accountRequestParser.Object,
                reportGenerator.Object, validator.Object);
        }

        [TearDown]
        public void Teardown()
        {

        }

        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void GetFromFile_WhenFilePathIsNull_ThrowsArgumentNullException(string filePath)
        {
            TestDelegate act = () => service.GetFromFile(filePath);

            Assert.That(act, Throws.ArgumentNullException);
        }

        [Test]
        public void GetFromFile_WhenRequestIsNotValid_ThrowsInvalidOperationException()
        {
            var filePath = "validFilePath";
            var accountBalanceRequest = new AccountBalanceRequest();
            string reason;
            accountRequestParser.Setup(p => p.GetAccountBalanceRequestFromFile(filePath))
                .Returns(accountBalanceRequest);
            validator.Setup(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason)).Returns(false);
            
            TestDelegate act = () => service.GetFromFile(filePath);

            Assert.That(act, Throws.InvalidOperationException);
            accountRequestParser.Verify(p => p.GetAccountBalanceRequestFromFile(filePath), Times.Once);
            validator.Verify(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason), Times.Once);
            reportGenerator.Verify(r => r.Generate(It.IsAny<Account>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void GetFromFile_ReturnsReport()
        {
            var filePath = "validFilePath";
            var accountBalanceRequest = new AccountBalanceRequest { Accounts = new List<Account> { new Account() } };
            string reason;
            accountRequestParser.Setup(p => p.GetAccountBalanceRequestFromFile(filePath))
                .Returns(accountBalanceRequest);
            validator.Setup(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason)).Returns(true);

            TestDelegate act = () => service.GetFromFile(filePath);

            Assert.That(act, Throws.Nothing);
            accountRequestParser.Verify(p => p.GetAccountBalanceRequestFromFile(It.IsAny<string>()), Times.Once);
            validator.Verify(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason), Times.Once);
            reportGenerator.Verify(r => r.Generate(It.IsAny<Account>(), It.IsAny<DateTime>()), Times.Once);
        }

        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void GetFromJson_WhenJsonIsNull_ThrowsArgumentNullException(string json)
        {
            TestDelegate act = () => service.GetFromJson(json);

            Assert.That(act, Throws.ArgumentNullException);
        }

        [Test]
        public void GetFromJson_WhenRequestIsNotValid_ThrowsInvalidOperationException()
        {
            var json = "validJson";
            var accountBalanceRequest = new AccountBalanceRequest();
            string reason;
            accountRequestParser.Setup(p => p.GetAccountBalanceRequestFromString(json))
                .Returns(accountBalanceRequest);
            validator.Setup(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason)).Returns(false);

            TestDelegate act = () => service.GetFromJson(json);

            Assert.That(act, Throws.InvalidOperationException);
            accountRequestParser.Verify(p => p.GetAccountBalanceRequestFromString(json), Times.Once);
            validator.Verify(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason), Times.Once);
            reportGenerator.Verify(r => r.Generate(It.IsAny<Account>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void GetFromJson_ReturnsReport()
        {
            var json = "validJson";
            var accountBalanceRequest = new AccountBalanceRequest { Accounts = new List<Account> { new Account() } };
            string reason;
            accountRequestParser.Setup(p => p.GetAccountBalanceRequestFromString(json))
                .Returns(accountBalanceRequest);
            validator.Setup(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason)).Returns(true);

            TestDelegate act = () => service.GetFromJson(json);

            Assert.That(act, Throws.Nothing);
            accountRequestParser.Verify(p => p.GetAccountBalanceRequestFromString(It.IsAny<string>()), Times.Once);
            validator.Verify(v => v.IsValid(It.IsAny<AccountBalanceRequest>(), out reason), Times.Once);
            reportGenerator.Verify(r => r.Generate(It.IsAny<Account>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}

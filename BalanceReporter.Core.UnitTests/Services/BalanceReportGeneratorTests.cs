using System;
using System.Collections.Generic;
using System.Linq;

using BalanceReporter.Core.Services;
using BalanceReporter.Data;
using NUnit.Framework;

namespace BalanceReporter.Core.UnitTests.Services
{
    [TestFixture]
    public class BalanceReportGeneratorTests
    {
        private IBalanceReportGenerator service;

        [SetUp]
        public void Setup()
        {
            service = new BalanceReportGenerator();
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void GetBalanceReport_ThrowArgumentNullException_WhenAccountIsNull()
        {
            TestDelegate act = () => service.Generate(null, DateTime.Now);

            Assert.That(act, Throws.ArgumentNullException);
        }

        [TestCaseSource(nameof(BalanceReportBeforeRequestDateAndBookedTransactionsTestCases))]
        public void GetBalanceReport_WhenOnlyBookedTransactionsExists_BeforeTheRequestDate(Account account, BalanceReport report)
        {
            var now = DateTime.Now;

            var result = service.Generate(account, now);

            Assert.That(result.TotalCredits, Is.EqualTo(report.TotalCredits));
            Assert.That(result.TotalDebits, Is.EqualTo(report.TotalDebits));
            Assert.That(result.EndOfDayBalances.SequenceEqual(report.EndOfDayBalances));
        }

        [TestCaseSource(nameof(BalanceReportBeforeRequestDateTransactionsTestCases))]
        public void GetBalanceReport_WhenBookedAndOtherTransactionsExists_BeforeTheRequestDate(Account account, BalanceReport report)
        {
            var now = DateTime.Now;

            var result = service.Generate(account, now);

            Assert.That(result.TotalCredits, Is.EqualTo(report.TotalCredits));
            Assert.That(result.TotalDebits, Is.EqualTo(report.TotalDebits));
            Assert.That(result.EndOfDayBalances.SequenceEqual(report.EndOfDayBalances));
        }

        [TestCaseSource(nameof(BalanceReportWithRequestDateAndBookedTransactionsTestCases))]
        public void GetBalanceReport_WhenOnlyBookedTransactionsExists_IncludingTheRequestDate(Account account, BalanceReport report)
        {
            var now = DateTime.Now;

            var result = service.Generate(account, now);

            Assert.That(result.TotalCredits, Is.EqualTo(report.TotalCredits));
            Assert.That(result.TotalDebits, Is.EqualTo(report.TotalDebits));
            Assert.That(result.EndOfDayBalances.SequenceEqual(report.EndOfDayBalances));
        }

        [TestCaseSource(nameof(BalanceReportWithRequestDateTransactionsTestCases))]
        public void GetBalanceReport_WhenBookedAndOtherTransactionsExists_IncludingTheRequestDate(Account account, BalanceReport report)
        {
            var now = DateTime.Now;

            var result = service.Generate(account, now);

            Assert.That(result.TotalCredits, Is.EqualTo(report.TotalCredits));
            Assert.That(result.TotalDebits, Is.EqualTo(report.TotalDebits));
            Assert.That(result.EndOfDayBalances.SequenceEqual(report.EndOfDayBalances));
        }

        [TestCaseSource(nameof(BalanceReportAfterRequestDateAndBookedTransactionsTestCases))]
        public void GetBalanceReport_WhenBooked_AfterRequestDate(Account account, BalanceReport report)
        {
            var now = DateTime.Now;

            var result = service.Generate(account, now);

            Assert.That(result.TotalCredits, Is.EqualTo(report.TotalCredits));
            Assert.That(result.TotalDebits, Is.EqualTo(report.TotalDebits));
            Assert.That(result.EndOfDayBalances.SequenceEqual(report.EndOfDayBalances));
        }

        [TestCaseSource(nameof(BalanceReportAfterRequestDateTransactionsTestCases))]
        public void GetBalanceReport_WhenBookedAndOther_AfterRequestDate(Account account, BalanceReport report)
        {
            var now = DateTime.Now;

            var result = service.Generate(account, now);

            Assert.That(result.TotalCredits, Is.EqualTo(report.TotalCredits));
            Assert.That(result.TotalDebits, Is.EqualTo(report.TotalDebits));
            Assert.That(result.EndOfDayBalances.SequenceEqual(report.EndOfDayBalances));
        }

        #region TestCaseSources
        static object[] BalanceReportBeforeRequestDateAndBookedTransactionsTestCases =
        {
            //Only credit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 45,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 200
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 170
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 165
                        }
                    }
                }
            },
            //Only dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 45,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 200
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 230
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 235
                        }
                    }
                }
            },
            //Credit and dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 40,
                    TotalDebits = 30,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 200
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 230
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 215
                        }
                    }
                }
            }
        };

        static object[] BalanceReportBeforeRequestDateTransactionsTestCases =
        {
            //Only credit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 45,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 200
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 170
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 165
                        }
                    }
                }
            },
            //Only dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-8) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 45,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 200
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 230
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 235
                        }
                    }
                }
            },
            //Credit and dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 40,
                    TotalDebits = 30,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 200
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 230
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 215
                        }
                    }
                }
            }
        };

        static object[] BalanceReportWithRequestDateAndBookedTransactionsTestCases =
        {
            //Only credit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 45,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 190
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 160
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 155
                        }
                    }
                }
            },
            //Only dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 45,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 210
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 240
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 245
                        }
                    }
                }
            },
            //Credit and dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 40,
                    TotalDebits = 30,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 195
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 225
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 210
                        }
                    }
                }
            }
        };

        static object[] BalanceReportWithRequestDateTransactionsTestCases =
        {
            //Only credit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 45,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 190
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 160
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 155
                        }
                    }
                }
            },
            //Only dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-8) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 45,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 210
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 240
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 245
                        }
                    }
                }
            },
            //Credit and dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(-1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(-8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 40,
                    TotalDebits = 30,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>
                    {
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                            Balance = 195
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2),
                            Balance = 225
                        },
                        new BalanceReport.EndOfTheDayBalance
                        {
                            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8),
                            Balance = 210
                        }
                    }
                }
            }
        };

        static object[] BalanceReportAfterRequestDateAndBookedTransactionsTestCases =
        {
            //Only credit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance> ()
                }
            },
            //Only dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance> ()
                }
            },
            //Credit and dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>()
                }
            }
        };

        static object[] BalanceReportAfterRequestDateTransactionsTestCases =
        {
            //Only credit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance> ()
                }
            },
            //Only dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 5, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(8) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>()
                }
            },
            //Credit and dedit transations
            new object[]
            {
                new Account
                {
                    Available = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Current = new Balance {Amount = 200, CreditDebitIndicator = CreditDebitIndicator.Credit},
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Status = Transaction.State.Booked, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Booked, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(8) },
                        new Transaction { Status = Transaction.State.Other, Amount = 10, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 15, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(2) },
                        new Transaction { Status = Transaction.State.Other, Amount = 20, CreditDebitIndicator = CreditDebitIndicator.Debit, BookingDate = DateTime.Now.AddDays(1) },
                        new Transaction { Status = Transaction.State.Other, Amount = 25, CreditDebitIndicator = CreditDebitIndicator.Credit, BookingDate = DateTime.Now.AddDays(8) },
                    }
                },
                new BalanceReport
                {
                    TotalCredits = 0,
                    TotalDebits = 0,
                    EndOfDayBalances = new List<BalanceReport.EndOfTheDayBalance>()
                }
            }
        };

        #endregion TestCaseSources
    }
}

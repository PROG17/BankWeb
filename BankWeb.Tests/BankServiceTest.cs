using BankWeb.Configs;
using BankWeb.Models;
using BankWeb.Repositories;
using BankWeb.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BankWeb.Tests
{
    [TestClass]
    public class BankServiceTest
    {
        private static IBankService _service;

        [TestInitialize]
        public void TestInit()
        {
            var accounts = new Dictionary<int, Account>
            {
                {
                    13019,
                    new Account()
                    {
                        AccountId = 13019,
                        Balance = 1488.80M,
                        CustomerId = 1005
                    }
                },
                {
                    13020,
                    new Account()
                    {
                        AccountId = 13020,
                        Balance = 613.20M,
                        CustomerId = 1005
                    }
                },
                {
                    13093,
                    new Account()
                    {
                        AccountId = 13093,
                        Balance = 695.62M,
                        CustomerId = 1024
                    }
                },
                {
                    13128,
                    new Account()
                    {
                        AccountId = 13128,
                        Balance = 392.20M,
                        CustomerId = 1032
                    }
                },
                {
                    13130,
                    new Account()
                    {
                        AccountId = 13130,
                        Balance = 4807.00M,
                        CustomerId = 1032
                    }
                }
            };

            var mockRepository = new Mock<IBankRepository>();
            mockRepository.Setup(x => x.GetAllAccounts()).Returns(accounts);

            _service = new BankService(mockRepository.Object);
        }

        [TestMethod]
        public void DepositTest()
        {
            int accountNumber = 13020;
            decimal amount = 888.8M, expectedAmount = 1502M;

            var expectedResponse = BankResponse.Success;
            var actualResponse = _service.Deposit(accountNumber, amount);

            Assert.AreEqual(expectedAmount, _service.GetAccount(accountNumber).Balance);
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void WithdrawTest()
        {
            int accountNumber = 13019;
            decimal amount = 222, expectedAmount = 1266.8M;

            var expectedResponse = BankResponse.Success;
            var actualResponse = _service.Withdraw(accountNumber, amount);

            Assert.AreEqual(expectedAmount, _service.GetAccount(accountNumber).Balance);
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void WithdrawTooMuch()
        {
            int accountNumber = 13093;
            decimal amount = 700, expectedAmount = 695.62M;

            var expectedResponse = BankResponse.NoFunds;
            var actualResponse = _service.Withdraw(accountNumber, amount);

            Assert.AreEqual(expectedAmount, _service.GetAccount(accountNumber).Balance);
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TransferTest()
        {

            int accountNumberFrom = 13019;
            int accountNumberTo = 13093;
            decimal amount = 222, expectedAmount = 917.62M;

            var expectedResponse = BankResponse.Success;
            var actualResponse = _service.Transfer(accountNumberFrom, accountNumberTo, amount);

            Assert.AreEqual(expectedAmount, _service.GetAccount(accountNumberTo).Balance);
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TransferTestTooMuch()
        {

            int accountNumberFrom = 13019;
            int accountNumberTo = 13093;
            decimal amount = 1500, expectedAmount = 695.62M;

            var expectedResponse = BankResponse.NoFunds;
            var actualResponse = _service.Transfer(accountNumberFrom, accountNumberTo, amount);

            Assert.AreEqual(expectedAmount, _service.GetAccount(accountNumberTo).Balance);
            Assert.AreEqual(expectedResponse, actualResponse);
        }
    }
}
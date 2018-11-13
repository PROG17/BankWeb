using BankWeb.Configs;
using BankWeb.Models;
using BankWeb.Repositories;
using BankWeb.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BankWeb.Tests
{
    [TestClass]
    public class BankServiceTest
    {
        private static IBankRepository _repository;
        private static IBankService _service;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var bankConfig = new ApplicationSettingsConfig()
            {
                Filename = "..\\..\\..\\..\\BankWeb\\repositories\\bankdata-small.txt"
            };

            var mock = new Mock<IOptionsSnapshot<ApplicationSettingsConfig>>();
            mock.Setup(m => m.Value).Returns(bankConfig);

            _repository = new BankRepository(mock.Object);
            _service = new BankService(_repository);
        }

        [TestMethod]
        public void DepositTest()
        {
            int accountNumber = 13020, customerNumber = 1005;
            decimal amount = 888.8M, expectedAmount = 1502M;

            var expectedResponse = BankResponse.Success;
            var actualResponse = _service.Deposit(accountNumber, amount);

            Assert.AreEqual(expectedAmount, GetAccountBalance(customerNumber, accountNumber));
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void WithdrawTest()
        {
            int accountNumber = 13019, customerNumber = 1005;
            decimal amount = 222, expectedAmount = 1266.8M;

            var expectedResponse = BankResponse.Success;
            var actualResponse = _service.Withdraw(accountNumber, amount);

            Assert.AreEqual(expectedAmount, GetAccountBalance(customerNumber, accountNumber));
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void WithdrawTooMuch()
        {
            int accountNumber = 13093, customerNumber = 1024;
            decimal amount = 700, expectedAmount = 695.62M;

            var expectedResponse = BankResponse.NoFunds;
            var actualResponse = _service.Withdraw(accountNumber, amount);

            Assert.AreEqual(expectedAmount, GetAccountBalance(customerNumber, accountNumber));
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TransferTest()
        {

            int accountNumberFrom = 13019;
            int accountNumberTo = 13093, customerNumberTo = 1024;
            decimal amount = 222, expectedAmount = 917.62M;

            var expectedResponse = BankResponse.Success;
            var actualResponse = _service.Transfer(accountNumberFrom, accountNumberTo, amount);

            Assert.AreEqual(expectedAmount, GetAccountBalance(customerNumberTo, accountNumberTo));
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TransferTestTooMuch()
        {

            int accountNumberFrom = 13019;
            int accountNumberTo = 13093, customerNumberTo = 1024;
            decimal amount = 1500, expectedAmount = 695.62M;

            var expectedResponse = BankResponse.NoFunds;
            var actualResponse = _service.Transfer(accountNumberFrom, accountNumberTo, amount);

            Assert.AreEqual(expectedAmount, GetAccountBalance(customerNumberTo, accountNumberTo));
            Assert.AreEqual(expectedResponse, actualResponse);
        }

        private decimal GetAccountBalance(int customerNumber, int accountNumber)
        {
            return _repository.GetAllAccounts()[accountNumber].Balance;
        }


    }
}
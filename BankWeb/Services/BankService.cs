using BankWeb.Configs;
using BankWeb.Models;
using BankWeb.Repositories;
using System.Collections.Generic;

namespace BankWeb.Services
{
    public interface IBankService
    {
        Account GetAccount(int accountId);
        List<Account> GetTransferDetails(int fromAccount, int toAccount);
        decimal? GetAccountBalance(int accountNumber);
        IEnumerable<Customer> GetCustomers();

        BankResponse Deposit(int account, decimal amount);
        BankResponse Withdraw(int account, decimal amount);
        BankResponse Transfer(int fromAccoun, int toAccount, decimal amount);
    }

    public class BankService : IBankService
    {
        private readonly IBankRepository _repository;

        public BankService(IBankRepository repository)
        {
            _repository = repository;
        }

        public Account GetAccount(int accountId)
        {
            return _repository.GetAllAccounts().TryGetValue(accountId, out Account result) ? result : null;
        }

        public decimal? GetAccountBalance(int accountId)
        {
            return GetAccount(accountId)?.Balance;
        }

        public List<Account> GetTransferDetails(int fromAccount, int toAccount)
        {
            List<Account> accountList = new List<Account>();
            accountList.Add(GetAccount(fromAccount));
            accountList.Add(GetAccount(toAccount));

            return accountList;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _repository.GetAllCustomers().Values;
        }

        public BankResponse Deposit(int accountNumber, decimal amount)
        {
            var response = BankResponse.Success;
            var account = GetAccount(accountNumber);

            if (account != null)
                account.Balance += amount;
            else
                response = BankResponse.NoAccount;

            return response;
        }

        public BankResponse Withdraw(int accountNumber, decimal amount)
        {
            var response = BankResponse.Success;
            var account = GetAccount(accountNumber);

            if (account != null)
            {
                if (account.Balance >= amount)
                    account.Balance -= amount;
                else
                    response = BankResponse.NoFunds;
            }
            else
                response = BankResponse.NoAccount;

            return response;
        }

        public BankResponse Transfer(int fromAccountNumber, int toAccountNumber, decimal amount)
        {

            var response = BankResponse.Success;
            var fromAccount = GetAccount(fromAccountNumber);
            var toAccount = GetAccount(toAccountNumber);

            if (fromAccount != null && toAccount != null)
            {
                if (fromAccount.Balance >= amount)
                {
                    fromAccount.Balance -= amount;
                    toAccount.Balance += amount;
                }
                else
                    response = BankResponse.NoFunds;                
            }
            else
                response = BankResponse.NoAccount;

            return response;
        }
    }
}
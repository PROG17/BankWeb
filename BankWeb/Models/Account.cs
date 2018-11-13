using BankWeb.Configs;
using BankWeb.Repositories;
using BankWeb.Services;

namespace BankWeb.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
       
    }
}

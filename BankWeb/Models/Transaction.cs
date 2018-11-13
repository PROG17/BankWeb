using System.ComponentModel.DataAnnotations;

namespace BankWeb.Models
{
    public class Transaction
    {
        [Range(0, int.MaxValue)]
        public int AccountNumber { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, int.MaxValue)]
        public int ToAccountNumber { get; set; }
    }
}
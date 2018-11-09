using System.Collections.Generic;

namespace BankWeb.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Personnummer { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        [Ignore]
        public virtual IEnumerable<Account> Accounts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Finane_Transaction_Manager
{

    public class FinanceTransaction
    {
       
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string AccountEmail { get; set; }
        public string Notes { get; set; }
        public bool IsRecurring { get; set; }

        public FinanceTransaction( string description, decimal amount, TransactionType type, TransactionStatus status, string date, string category, string accountEmail, string notes)
        {
            Id = Guid.NewGuid();
            Description = description;
            Amount = amount;
            Type = type;
            Status = status;
            Date = date;
            Category = category;
            AccountEmail = accountEmail;
            Notes = notes;
           
        }



    }
}

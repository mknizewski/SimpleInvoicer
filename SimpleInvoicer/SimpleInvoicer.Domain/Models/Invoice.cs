using System;
using System.Collections.Generic;

namespace SimpleInvoicer.Domain.Models
{
    public class Invoice
    {
        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public Subject Seller { get; set; }
        public Subject Reciver { get; set; }
        public Subject Buyer { get; set; }
        public Bank Bank { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public List<Item> Items { get; set; }
        public decimal AmmountGross { get; set; }
    }
}

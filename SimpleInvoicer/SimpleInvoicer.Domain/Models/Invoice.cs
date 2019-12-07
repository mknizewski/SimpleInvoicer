using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleInvoicer.Domain.Models
{
    [Serializable]
    public class Invoice : INotifyPropertyChanged
    {
        private decimal _amountGross;

        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public Subject Seller { get; set; }
        public Subject Reciver { get; set; }
        public Subject Buyer { get; set; }
        public Bank Bank { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public List<Item> Items { get; set; }
        public decimal AmmountGross 
        {
            get => _amountGross;
            set
            {
                _amountGross = value;
                OnPropertyChanged(nameof(AmmountGross));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

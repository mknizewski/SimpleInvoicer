using System;
using System.ComponentModel;

namespace SimpleInvoicer.Domain.Models
{
    [Serializable]
    public class Item : INotifyPropertyChanged
    {
        private decimal _taxAmmount;
        private decimal _valueGross;
        private decimal _value;

        public int Order { get; set; }
        public string Name { get; set; }
        public Units Unit { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Value 
        {
            get => _value; 
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        public ItemTax Tax { get; set; }
        public decimal TaxAmmount 
        {
            get => _taxAmmount; 
            set
            {
                _taxAmmount = value;
                OnPropertyChanged(nameof(TaxAmmount));
            }
        }
        public decimal ValueGross 
        {
            get => _valueGross; 
            set
            {
                _valueGross = value;
                OnPropertyChanged(nameof(ValueGross));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

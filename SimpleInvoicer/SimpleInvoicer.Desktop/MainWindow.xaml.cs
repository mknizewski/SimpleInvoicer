using SimpleInvoicer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace SimpleInvoicer.Desktop
{
    public partial class MainWindow : Window
    {
        public Invoice Invoice { get; set; }

        public MainWindow()
        {
            Language = XmlLanguage.GetLanguage("pl-PL");
            InitializeComponent();

            Invoice = new Invoice
            {
                Seller = new Subject(),
                Reciver = new Subject(),
                Buyer = new Subject(),
                Bank = new Bank(),
                Items = new List<Item>
                {
                    new Item { Name = "Test", Unit = Units.Kilograms, Order = 1, Tax = ItemTax.TwentyThree },
                     new Item { Name = "Test", Unit = Units.Piece, Order = 1 }
                },
                PaymentDate = DateTime.Now.AddDays(14.0),
                IssueDate = DateTime.Now,
            };

            DataContext = Invoice;
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var data = InvoiceGrid.SelectedItem as Item;
            Invoice.Items.Remove(data);
            InvoiceGrid.Items.Refresh();
        }

        private void AddNewRow_Click(object sender, RoutedEventArgs e)
        {
            var item = new Item();
            Invoice.Items.Add(item);
            InvoiceGrid.Items.Refresh();
        }
    }
}

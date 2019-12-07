using Microsoft.Win32;
using SimpleInvoicer.Application.Factory;
using SimpleInvoicer.Application.Services;
using SimpleInvoicer.Domain.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace SimpleInvoicer.Desktop
{
    public partial class MainWindow : Window
    {
        private const string PolishCulture = "pl-PL";
        private const string SaveDialogExtensionFilter = "Plik PDF (*.pdf)|*.pdf";

        private readonly IInvoiceService _invoiceService;
        private readonly IItemService _itemService;

        public Invoice Invoice { get; set; }

        public MainWindow()
        {
            _invoiceService = ServiceFactory.CreateInstance<InvoiceService, IInvoiceService>();
            _itemService = ServiceFactory.CreateInstance<ItemService, IItemService>();

            InitializeComponent();
            SetConfig();
        }

        public void SetConfig()
        {
            Language = XmlLanguage.GetLanguage(PolishCulture);
            Invoice = _invoiceService.GetEmptyInvoice();
            DataContext = Invoice;
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var data = InvoiceGrid.SelectedItem as Item;
            Invoice.Items.Remove(data);
            _itemService.SortOrder(Invoice.Items);
            InvoiceGrid.Items.Refresh();
        }

        private void AddNewRow_Click(object sender, RoutedEventArgs e)
        {
            Invoice.Items.Add(_itemService.GetEmptyItem(Invoice.Items));
            InvoiceGrid.Items.Refresh();
        }

        private async void GenerateInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = SaveDialogExtensionFilter
                };

                if (saveFileDialog.ShowDialog().GetValueOrDefault())
                    await SaveInvoiceToFile(saveFileDialog.FileName);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                MakeBusy(false);
            }
        }

        private async Task SaveInvoiceToFile(string fileName)
        {
            MakeBusy(true);
            await _invoiceService.SaveInvoiceToPdf(Invoice, fileName);
        }

        private void MakeBusy(bool isBusy)
        {
            if (isBusy)
            {
                BusyIndi.IsBusy = true;
                BusyIndi.Visibility = Visibility.Visible;
                GenerateInvoice.Visibility = Visibility.Collapsed;
            }
            else
            {
                BusyIndi.IsBusy = false;
                BusyIndi.Visibility = Visibility.Collapsed;
                GenerateInvoice.Visibility = Visibility.Visible;
            }
        }
    }
}

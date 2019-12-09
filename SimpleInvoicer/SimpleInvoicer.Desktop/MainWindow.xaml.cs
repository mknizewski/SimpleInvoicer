using Microsoft.Win32;
using SimpleInvoicer.Application.Factory;
using SimpleInvoicer.Application.Services;
using SimpleInvoicer.Domain.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace SimpleInvoicer.Desktop
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string PolishCulture = "pl-PL";
        private const string DialogPdfExtensionFilter = "Plik PDF (*.pdf)|*.pdf";
        private const string DialogJsonExtensionFilter = "Plik JSON (*.json)|*.json";

        private readonly IInvoiceService _invoiceService;
        private readonly IItemService _itemService;

        public event PropertyChangedEventHandler PropertyChanged;

        private Invoice _invoice;
        public Invoice Invoice 
        {
            get => _invoice; 
            set
            {
                _invoice = value;
                OnPropertyChanged(nameof(Invoice));
            }
        }

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
                    Filter = DialogPdfExtensionFilter,
                    FileName = _invoiceService.GetDefaultFilePath(Invoice),
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

        private void InvoiceGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (!(InvoiceGrid.SelectedItem is Item item))
                return;

            _itemService.CalulateItem(item);
            _invoiceService.CalulateTotalPrice(Invoice);        
        }

        private void ExportInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = DialogJsonExtensionFilter
                };

                if (saveFileDialog.ShowDialog().GetValueOrDefault())
                    _invoiceService.SaveInvoiceToFile(Invoice, saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ImportInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = DialogJsonExtensionFilter
                };

                if (openFileDialog.ShowDialog().GetValueOrDefault())
                {
                    Invoice = _invoiceService.LoadInvoiceFromFile(openFileDialog.FileName);
                    DataContext = Invoice;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        protected void OnPropertyChanged(string name) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

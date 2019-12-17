using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SimpleInvoicer.Application.Factory;
using SimpleInvoicer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleInvoicer.Application.Services
{
    public interface IInvoiceService
    {
        Invoice GetEmptyInvoice();
        Task SaveInvoiceToPdf(Invoice invoice, string filePath);
        void CalulateTotalPrice(Invoice invoice);
        void SaveInvoiceToFile(Invoice invoice, string filePath);
        Invoice LoadInvoiceFromFile(string filePath);
        string GetDefaultFilePath(Invoice invoice);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IFileService _fileService;

        public InvoiceService() => 
            _fileService = ServiceFactory.CreateInstance<FileService, IFileService>();

        public void CalulateTotalPrice(Invoice invoice) => 
            invoice.AmmountGross = Math.Round(invoice.Items.Sum(x => x.ValueGross), 2, MidpointRounding.AwayFromZero);

        public string GetDefaultFilePath(Invoice invoice)
        {
            string invoiceNumber = invoice.Number;
            if (string.IsNullOrEmpty(invoiceNumber))
                return string.Empty;

            return $"FV-{invoiceNumber.Replace("/", "-")}.pdf";
        }

        public Invoice GetEmptyInvoice()
        {
            return new Invoice
            {
                Seller = new Subject(),
                Reciver = new Subject(),
                Buyer = new Subject(),
                Bank = new Bank(),
                Items = new List<Item>(),
                PaymentDate = DateTime.Now.AddDays(14.0),
                IssueDate = DateTime.Now,
                PaymentForm = PaymentForm.Bank,
                AmmountGross = decimal.Zero,
                Number = string.Empty
            };
        }

        public Invoice LoadInvoiceFromFile(string filePath) => 
            _fileService.ImportInvoiceFromFile(filePath);

        public void SaveInvoiceToFile(Invoice invoice, string filePath) => 
            _fileService.ExportInvoiceToFile(invoice, filePath);

        public async Task SaveInvoiceToPdf(Invoice invoice, string filePath)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var lanuchOptions = new LaunchOptions
            {
                Headless = true,
                WebSocketFactory = new PuppeteerSharp.Transport.WebSocketFactory((uri, options, token) =>
                {
                    return System.Net.WebSockets.SystemClientWebSocket.ConnectAsync(uri, token);
                })
            };

            using (var browser = await Puppeteer.LaunchAsync(lanuchOptions))
            {
                using (var page = await browser.NewPageAsync()) 
                {
                    var template = _fileService.GetInvoiceTemplate();
                    var function = _fileService.GetJsFunction();
                    var serializableInvoice = JsonConvert.SerializeObject(invoice, Formatting.Indented);

                    await page.SetContentAsync(template);
                    await page.EvaluateFunctionAsync(function, serializableInvoice);
                    var result = await page.GetContentAsync();
                    await page.PdfAsync(filePath, new PdfOptions
                    {
                        Format = PaperFormat.A4,
                        MarginOptions = new MarginOptions
                        {
                            Bottom = "50",
                            Top = "50",
                            Right = "50",
                            Left = "50"
                        }
                    });
                }
            }
        }
    }
}

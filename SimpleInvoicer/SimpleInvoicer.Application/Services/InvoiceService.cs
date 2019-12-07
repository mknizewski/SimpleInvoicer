using PuppeteerSharp;
using PuppeteerSharp.Media;
using SimpleInvoicer.Application.Factory;
using SimpleInvoicer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleInvoicer.Application.Services
{
    public interface IInvoiceService
    {
        Invoice GetEmptyInvoice();
        Task SaveInvoiceToPdf(Invoice invoice, string filePath);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IFileService _fileService;

        public InvoiceService() => 
            _fileService = ServiceFactory.CreateInstance<FileService, IFileService>();

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
            };
        }

        public async Task SaveInvoiceToPdf(Invoice invoice, string filePath)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    var template = _fileService.GetInvoiceTemplate();
                    await page.SetContentAsync(template);
                    var result = await page.GetContentAsync();
                    await page.PdfAsync(filePath, new PdfOptions
                    {
                        Format = PaperFormat.A4
                    });
                }
            }
        }
    }
}

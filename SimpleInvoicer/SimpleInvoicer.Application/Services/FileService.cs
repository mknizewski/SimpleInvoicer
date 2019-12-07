using Newtonsoft.Json;
using SimpleInvoicer.Domain.Models;
using System.IO;

namespace SimpleInvoicer.Application.Services
{
    public interface IFileService
    {
        string GetInvoiceTemplate();
        string GetJsFunction();
        void ExportInvoiceToFile(Invoice invoice, string filePath);
        Invoice ImportInvoiceFromFile(string filePath);
    }

    public class FileService : IFileService
    {
        private const string InvoiceTemplatePath = "Resources/invoice-template.html";
        private const string JsFunctionPath = "Resources/invoice-function.js";

        public void ExportInvoiceToFile(Invoice invoice, string filePath)
        {
            var serializableInvoice = JsonConvert.SerializeObject(invoice, Formatting.Indented);
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(serializableInvoice);
            }
        }

        public string GetInvoiceTemplate()
        {
            using (var fileStream = new FileStream(InvoiceTemplatePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                    return streamReader.ReadToEnd();
            }
        }

        public string GetJsFunction()
        {
            using (var fileStream = new FileStream(JsFunctionPath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                    return streamReader.ReadToEnd();
            }
        }

        public Invoice ImportInvoiceFromFile(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                   return JsonConvert.DeserializeObject<Invoice>(streamReader.ReadToEnd());
            }
        }
    }
}

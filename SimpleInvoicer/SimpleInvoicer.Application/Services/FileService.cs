using System.IO;

namespace SimpleInvoicer.Application.Services
{
    public interface IFileService
    {
        string GetInvoiceTemplate();
    }

    public class FileService : IFileService
    {
        private const string InvoiceTemplatePath = "Resources/invoice-template.html";

        public string GetInvoiceTemplate()
        {
            using (var fileStream = new FileStream(InvoiceTemplatePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                    return streamReader.ReadToEnd();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    public class FaxDocumentBuilder
    {
        public FaxDocument FromFile(string filePath)
        {
            return new FaxDocument()
            {
                FaxDocumentName = System.IO.Path.GetFileName(filePath),
                FaxDocumentBinary = System.IO.File.ReadAllBytes(filePath)
            };
        }

        public FaxDocument FromText(string faxDocumentName, string faxDocumentText)
        {
            var bytes = Encoding.UTF8.GetBytes(faxDocumentText);
            return new FaxDocument()
            {
                FaxDocumentName = faxDocumentName,
                FaxDocumentBinary = bytes
            };
        }

        public FaxDocument FromByteArray(string faxDocumentName, byte[] faxDocumentData)
        {
            return new FaxDocument()
            {
                FaxDocumentName = faxDocumentName,
                FaxDocumentBinary = faxDocumentData
            };
        }

        public FaxDocument FromBase64String(string faxDocumentName, string encodedFaxDocumentData)
        {
            var bytes = Convert.FromBase64String(encodedFaxDocumentData);
            return new FaxDocument()
            {
                FaxDocumentName = faxDocumentName,
                FaxDocumentBinary = bytes
            };
        }

        public FaxDocument FromID(int faxDocumentID)
        {
            return new FaxDocument()
            {
                FaxDocumentID = faxDocumentID
            };
        }
    }
}

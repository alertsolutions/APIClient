using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class FaxDocument
    {
        public int FaxDocumentID { get; internal set; }
        public string FaxDocumentName { get; internal set; }
        public byte[] FaxDocumentBinary { get; internal set; }

        internal XElement ToXml()
        {
            return new DocumentElementBuilder().ToXml("Document", FaxDocumentID, FaxDocumentName, FaxDocumentBinary);
        }
    }
}

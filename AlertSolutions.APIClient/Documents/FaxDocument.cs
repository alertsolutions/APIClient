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
        internal int FaxDocumentID { get; set; }
        internal string FaxDocumentName { get; set; }
        internal byte[] FaxDocumentBinary { get; set; }

        internal XElement ToXml()
        {
            return new DocumentElementBuilder().ToXml("Document", FaxDocumentID, FaxDocumentName, FaxDocumentBinary);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    // enforces fields so orders using it will have these fields accessible to the user
    internal interface IContainDocuments
    {
        List<Document> Documents { get; set; }
    }

    [Serializable]
    // common code for any orders that contain a list of documents
    internal class DocumentInfo
    {
        private List<Document> _documents;
        public List<Document> Documents
        {
            get { return _documents ?? (_documents = new List<Document>()); }
            set { _documents = value; }
        }

        public DocumentInfo()
        {
            Documents = new List<Document>();            
        }

        public XElement GetDocuments()
        {
            var xDocs = new XElement("Documents");
            if (Documents.Count < 1)
            {
                throw new FormatException("Must have at least one document.");
            }
            else
            {
                Documents.ForEach(doc =>
                {
                    xDocs.Add(doc.ToXml());
                });
            }
            return xDocs;
        }
    }
}

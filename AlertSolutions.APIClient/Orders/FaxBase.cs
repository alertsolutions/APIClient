using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Documents;

namespace AlertSolutions.API.Orders
{
    // handle code common to TL and WL
    public abstract class FaxBase : Order, IContainDocuments
    {
        // accessors for document info ( goes into each class that inherits IContainDocuments)
        private DocumentInfo _documentInfo = new DocumentInfo();
        public List<Document> Documents { get { return _documentInfo.Documents; } set { _documentInfo.Documents = value; } }

        public int NumberOfRedials { get; set; }
        public string FaxFrom { get; set; }
        public string ToHeader { get; set; }
        public string DocumentStyle { get; set; }

        internal FaxBase()
        {
            // defaults
        }

        protected override XDocument BuildXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("NumberOfRedials", NumberOfRedials));
            orderTag.Add(new XElement("FaxFrom", FaxFrom));
            orderTag.Add(new XElement("DocumentStyle", DocumentStyle));

            orderTag.Add(_documentInfo.GetDocuments());

            return xmlDoc;
        }
    }
}

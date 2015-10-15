using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Documents;

namespace AlertSolutions.API.Orders
{
    // handle code common to TL and WL
    public enum DocumentStyle { Letter, Legal, A4 }

    public abstract class FaxBase : Order
    {
        public List<FaxDocument> Documents { get; set; }

        public int NumberOfRedials { get; set; }
        public string FaxFrom { get; set; }
        public string ToHeader { get; set; }
        public DocumentStyle DocumentStyle { get; set; }

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

            if (Documents == null)
                Documents = new List<FaxDocument>();

            if (Documents.Count < 1)
                throw new FormatException("Must have at least one fax document.");

            var xDocs = new XElement("Documents");
            Documents.ForEach(doc => xDocs.Add(doc.ToXml()));
            orderTag.Add(xDocs);

            return xmlDoc;
        }
    }
}

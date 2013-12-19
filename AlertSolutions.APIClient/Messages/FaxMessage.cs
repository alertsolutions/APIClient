using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Messages
{
    // fax transactional
    public class FaxMessage : FaxBase, IMessage
    {
        public string FaxNumber { get; set; }

        public FaxMessage()
        {
            this.TypeOfOrder = OrderType.FaxMessage;
        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("ToHeader", ToHeader));
            orderTag.Add(new XElement("FaxNumber", FaxNumber));
            
            return xmlDoc.ToString();
        }
    }
}

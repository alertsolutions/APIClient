using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AlertSolutions.API
{
    public class CancelResponse
    {
        public CancelStatusCode StatusCode { get; private set; }
        public string StatusMessage { get; private set; }
        public int OrderId { get; private set; }
        public OrderType OrderType { get; private set; }

        public CancelResponse(string responseXml)
        {
            var root = XDocument.Parse(responseXml).Root;
            var xe = root.XPathSelectElement("/PostAPIResponse/CancelOrderResult");

            StatusCode = (CancelStatusCode)Convert.ToInt32(xe.Element("StatusCode").Value);
            StatusMessage = xe.Element("Status").Value;
            OrderId = Convert.ToInt32(xe.Attribute("Id").Value);
            OrderType = OrderTypeUtil.ParseCode(xe.Attribute("Type").Value);
        }
    }
}
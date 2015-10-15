using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AlertSolutions.API
{
    public interface ICancelResponse
    {
        CancelStatusCode StatusCode { get; }
        string StatusMessage { get; }
        int OrderId { get; }
        OrderType OrderType { get; }
        void ParseXml(string responseXml);
    }

    public class CancelResponse : ICancelResponse
    {
        public CancelStatusCode StatusCode { get; private set; }
        public string StatusMessage { get; private set; }
        public int OrderId { get; private set; }
        public OrderType OrderType { get; private set; }

        public CancelResponse() { }

        public CancelResponse(string responseXml)
        {
            ParseXml(responseXml);
        }

        public void ParseXml(string responseXml)
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
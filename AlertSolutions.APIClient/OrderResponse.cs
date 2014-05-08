using System;
using System.Xml;

namespace AlertSolutions.API
{
    [Serializable]
    public class OrderResponse
    {
        public OrderResponse(OrderType type)
        {
            OrderType = type;
            OrderID = -1;
            Unqid = "";
            RequestErrorMessage = "unknown";
            RequestResult = RequestResultType.Error;
            ResponseTime = -1;
        }

        public OrderResponse(string type) : this(OrderTypeUtil.ParseCode(type)) { }

        public int OrderID { get; set; }
        public string Unqid { get; set; } // unique transactional id (empty if broadcast types)
        public OrderType OrderType { get; set; }

        public int ResponseTime { get; set; }

        public RequestResultType RequestResult { get; set; }
        public string RequestErrorMessage { get; set; }

        public void InitializeTestMode()
        {
            OrderID = 0;
            Unqid = "0";
            ResponseTime = 0;
            RequestResult = RequestResultType.TestMode;
            RequestErrorMessage = "none";
        }

        public void ParseXml(XmlNode xmlResult)
        {
            RequestResult = RequestResultType.Success; // we have xml and it parsed correctly, so here we know request was made successfully
            RequestErrorMessage = "none";

            if (xmlResult["OrderID"] == null)
            {
                RequestResult = RequestResultType.Error;
                RequestErrorMessage = xmlResult["Exception"].InnerText;
            }
            else
            {
                OrderID = Convert.ToInt32(xmlResult["OrderID"].InnerText);
                Unqid = GetTransactionId(xmlResult);
            }
        }

        private string GetTransactionId(XmlNode xn)
        {
            string transactionid = null;
            if (OrderType == OrderType.EmailMessage || OrderType == OrderType.SMSMessage ||
                OrderType == OrderType.VoiceMessage || OrderType == OrderType.FaxMessage)
                transactionid = xn["transactionID"].InnerText;
            return transactionid;
        }
    }
}

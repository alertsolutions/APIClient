using System;
using System.Xml;

namespace AlertSolutions.API
{
    public interface IOrderResponse
    {
        int OrderID { get; set; }
        string Unqid { get; set; }
        OrderType OrderType { get; set; }
        int ResponseTime { get; set; }
        RequestResultType RequestResult { get; set; }
        string RequestErrorMessage { get; set; }
        void InitializeTestMode();
        void ParseXml(XmlNode xmlResult);
    }

    [Serializable]
    public class OrderResponse : IOrderResponse
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
            if (xmlResult.Name == "Exception")
            {
                RequestResult = RequestResultType.Error;
                RequestErrorMessage = xmlResult.InnerText;
            }
            else if (xmlResult.Name == "SaveTransactionalOrderResult")
            {
                RequestResult = RequestResultType.Success;
                RequestErrorMessage = "none";
                OrderID = Convert.ToInt32(xmlResult["OrderID"].InnerText);
                Unqid = GetTransactionId(xmlResult);
            }
            else
            {
                throw new InvalidOperationException("invalid xml response: " + xmlResult);
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

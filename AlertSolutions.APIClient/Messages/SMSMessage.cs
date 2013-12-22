using System;
using System.Collections.Generic;
using System.Xml.Linq;

using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Messages
{
    [Serializable]
    public class SMSMessage : SMSBase, IMessage
    {
        // accessors for transactional info ( goes into each class that inherits ITransactionalOrder)
        private TransactionalInfo _transactionalInfo = new TransactionalInfo();

        //fields
        public string Number { get; set; }

        public SMSMessage()
        {
            this.TypeOfOrder = OrderType.SMSMessage;
            Number = "0";
        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("CellPhoneNumber", Number));

            return xmlDoc.ToString();
        }
    }
}
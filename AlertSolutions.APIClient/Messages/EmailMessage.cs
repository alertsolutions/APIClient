using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Messages
{
    public class EmailMessage : EmailBase, IMessage
    {
        // accessors for transactional info ( goes into each class that inherits ITransactionalOrder)
        private TransactionalInfo _transactionalInfo = new TransactionalInfo();

        //fields
        public string EmailTo { get; set; }

        public EmailMessage() : base()
        {
            this.TypeOfOrder = OrderType.EmailMessage;
            this.EmailTo = "";
        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = (xmlDoc.Element("Orders").Element("Order"));

            orderTag.Add(new XElement("EmailTo", this.EmailTo));

            //the rest of these are actually shared with EB, but the names of the elements
            // slightly differ in postAPI, hence the separate implementations
            orderTag.Add(new XElement("EmailFrom", base.EmailFrom));
            orderTag.Add(new XElement("EmailReplyTo", base.EmailReplyTo));
            orderTag.Add(new XElement("EmailSubject", base.EmailSubject));
            orderTag.Add(new XElement("IsForward", base.IsForward ? "Yes" : "No"));
            orderTag.Add(new XElement("ReplaceLink", base.IsReplaceLink ? "Yes" : "No"));
            orderTag.Add(new XElement("IsUnsubscribe", base.IsUnsubscribe ? "Yes" : "No"));

            Utilities.StripNodeFromElement(orderTag, "Date");
            Utilities.StripNodeFromElement(orderTag, "Time");

            return xmlDoc.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Messages
{
    [Serializable]
    public class VoiceMessage : VoiceBase, IMessage
    {
        // accessors for transactional info ( goes into each class that inherits ITransactionalOrder)
        private TransactionalInfo _transactionalInfo = new TransactionalInfo();

        //fields
        public string Phone { get; set; }

        public VoiceMessage()
        {
            this.TypeOfOrder = OrderType.VoiceMessage;
            Phone = "0";
            var sendTimeLocal = DateTime.Now;
            var sendTimeUTC = sendTimeLocal.ToUniversalTime();
            StopTimeUTC = new DateTime(sendTimeUTC.Year, sendTimeUTC.Month, sendTimeUTC.Day, 23, 59, 59);
            RestartTimeUTC = StopTimeUTC.AddMinutes(481); // 8am the next day
        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("Phone", Phone));

            // custom modification for this tag since voicemessage has stoptime, not stopdatetime tag
            Utilities.StripNodeFromElement(orderTag, "StopDateTime");
            orderTag.Add(new XElement("StopTime", StopTimeUTC.ToString("HH:mm")));

            // custom modification to remove date and time tags
            Utilities.StripNodeFromElement(orderTag, "Date");
            Utilities.StripNodeFromElement(orderTag, "Time");

            return xmlDoc.ToString();
        }
    }
}

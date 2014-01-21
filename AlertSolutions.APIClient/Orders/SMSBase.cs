using System;
using System.Collections.Generic;
using System.Xml.Linq;

using AlertSolutions.API.Documents;

namespace AlertSolutions.API.Orders
{
    // handle code common to MT and M
    [Serializable]
    public abstract class SMSBase : Order
    {
        public string ShortCode { get; set; }
        public DateTime StopTimeUTC { get; set; }
        public DateTime RestartTimeUTC { get; set; }
        public string OverType { get; set; }
        public TextMessage TextMessage { get; set; }

        internal SMSBase()
        {
            ShortCode = "0";
            OverType = "truncate";

            // TODO : remove these default values and force user to send them in future
            var sendTimeLocal = DateTime.Now;
            SendTimeUTC = sendTimeLocal.ToUniversalTime();
            StopTimeUTC = new DateTime(sendTimeLocal.Year, sendTimeLocal.Month, sendTimeLocal.Day, 23, 59, 59).ToUniversalTime();
            RestartTimeUTC = StopTimeUTC.AddMinutes(481); // 8am the next day
        }

        protected override XDocument BuildXml()
        {
            if (TextMessage == null)
            {
                throw new FormatException("Must set a TextMessage.");
            }

            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("ShortCode", ShortCode));
            orderTag.Add(new XElement("StopDateTime", StopTimeUTC.ToString("yyyy-MM-dd HH:mm")));
            orderTag.Add(new XElement("RestartTime", RestartTimeUTC.ToString("HH:mm")));
            orderTag.Add(new XElement("OverType", OverType));



            var messageElements = TextMessage.ToXml();
            foreach (var el in messageElements)
            {
                orderTag.Add(el);
            }

            return xmlDoc;
        }
    }
}

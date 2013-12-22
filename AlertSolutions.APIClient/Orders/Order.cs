using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlertSolutions.API.Orders
{
    public interface IOrder
    {
        OrderType TypeOfOrder { get; set; }
        DateTime SendTimeUTC { set; get; }
        string ToXml();
    }

    [Serializable]
    public abstract class Order : IOrder
    {
        public DateTime SendTimeUTC { get; set; }
        public OrderType TypeOfOrder { get; set; } // only inheriting order classes set this value

        protected Order()
        {
            this.SendTimeUTC = DateTime.UtcNow;
        }

        // implemented in inheriting classes, but still accessible from base
        // so that a user can take any order and get it's xml without downcasting to an ordertype
        public abstract string ToXml();

        // generates the basic xml common to all orders
        protected virtual XDocument BuildXml()
        {
            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Orders",
                    // adding the type attribute is necessary, but needs to be done in the individual order types themselves,
                    // first so TypeOfOrder has the correct information, and second so that it 
                    new XElement("Order", new XAttribute("Type", OrderTypeUtil.GetCode(TypeOfOrder)),
                        new XElement("Date", SendTimeUTC.ToString("yyyy-MM-dd")),
                        new XElement("Time", SendTimeUTC.ToString("HH:mm"))
                )));

            return xmlDoc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Documents;
using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Broadcasts
{
    // fax broadcast
    public class FaxBroadcast : FaxBase, IBroadcast
    {
        // accessors for broadcast info ( goes into each class that inherits IBroadcastOrder)
        private BroadcastInfo _broadcastInfo = new BroadcastInfo();
        public string BillCode { get { return _broadcastInfo.BillCode; } set { _broadcastInfo.BillCode = value; } }
        public string ProjectCode { get { return _broadcastInfo.ProjectCode; } set { _broadcastInfo.ProjectCode = value; } }
        public ContactList List { get { return _broadcastInfo.Contacts; } set { _broadcastInfo.Contacts = value; } }
        public bool AutoLaunch { get { return _broadcastInfo.AutoLaunch; } set { _broadcastInfo.AutoLaunch = value; } }

        //fields
        public bool Restart { get; set; }
        public int NumberOfResends { get; set; }
        public int ResendInterval { get; set; }
        public DateTime StopTimeUTC { get; set; }
        public DateTime RestartTimeUTC { get; set; }
        public string FaxHeader { get; set; }
        public string ToHeader1 { get; set; }
        public string ToHeader2 { get; set; }
        public string ToHeader3 { get; set; }
        public string ToHeader4 { get; set; }
        public bool Dedup { get; set; }
        //public string DedupField { get; set; } //TODO : Find where this came from ?? ! NOT IN DOCUMENTATION

        public FaxBroadcast()
        {
            this.TypeOfOrder = OrderType.FaxBroadcast;

            Restart = false;
            StopTimeUTC = this.SendTimeUTC.Date.AddHours(23).AddMinutes(59);//midnight the day it's sent
            RestartTimeUTC = this.SendTimeUTC.Date.AddDays(1).AddHours(8).AddMinutes(01); // 8 am the next day
        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("Restart", Restart ? "Yes" : "No"));
            orderTag.Add(new XElement("NumberOfResends", NumberOfResends));
            orderTag.Add(new XElement("ResendInterval"));

            orderTag.Add(new XElement("ToHeader1", ToHeader1));
            orderTag.Add(new XElement("ToHeader2", ToHeader2));
            orderTag.Add(new XElement("ToHeader3", ToHeader3));
            orderTag.Add(new XElement("ToHeader4", ToHeader4));

            orderTag.Add(new XElement("StopTime", StopTimeUTC.ToString("HH:mm")));
            orderTag.Add(new XElement("RestartTime", RestartTimeUTC.ToString("HH:mm")));
            orderTag.Add(new XElement("FaxHeader", FaxHeader));
            orderTag.Add(new XElement("Dedup", Dedup ? "Yes" : "No"));
            //orderTag.Add(new XElement("DedupField");

            var broadcastElements = _broadcastInfo.GetBroadcastElements();
            foreach (var element in broadcastElements)
            {
                orderTag.Add(element);
            }

            return xmlDoc.ToString();
        }
    }
}

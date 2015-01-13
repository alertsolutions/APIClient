using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Documents;
using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Broadcasts
{
    [Serializable]
    public class VoiceBroadcast : VoiceBase, IBroadcast
    {
        public enum VoiceThrottleType
        {
            None,
            CallsPerHour,
            MaximumCalls
        }

        // accessors for broadcast info ( goes into each class that inherits IBroadcastOrder)
        private BroadcastInfo _broadcastInfo = new BroadcastInfo();
        public string BillCode { get { return _broadcastInfo.BillCode; } set { _broadcastInfo.BillCode = value; } }
        public string ProjectCode { get { return _broadcastInfo.ProjectCode; } set { _broadcastInfo.ProjectCode = value; } }
        public ContactList List { get { return _broadcastInfo.Contacts; } set { _broadcastInfo.Contacts = value; } }
        public bool AutoLaunch { get { return _broadcastInfo.AutoLaunch; } set { _broadcastInfo.AutoLaunch = value; } }

        // fields
        public bool Dedup { get; set; }
        public string DedupField { get; set; }
        public bool Restart { get; set; }
        public int NumberOfResends { get; set; }
        public int NumberOfRedials { get; set; }
        public string VoiceHeader { get; set; }
        public string LanguageHeader { get; set; }
        public int ThrottleNumber { get; set; }
        public VoiceThrottleType ThrottleType { get; set; }

        public VoiceBroadcast()
        {
            this.Dedup = true;
            this.DedupField = "";
            this.TypeOfOrder = OrderType.VoiceBroadcast;
            this.Restart = false;
            this.NumberOfResends = 0;
            this.NumberOfRedials = 0;
            this.VoiceHeader = "";
            this.LanguageHeader = "";
            this.ThrottleType = VoiceThrottleType.None;
            this.ThrottleNumber = 1;
            this.Documents = new List<VoiceDocument>();
        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("Dedup", Dedup ? "Yes" : "No"));
            if (!string.IsNullOrEmpty(DedupField))
                orderTag.Add(new XElement("DedupField", DedupField));

            orderTag.Add(new XElement("Restart", Restart ? "Yes" : "No"));
            orderTag.Add(new XElement("NumberOfResends", NumberOfResends));
            orderTag.Add(new XElement("NumberOfRedials", NumberOfRedials));
            orderTag.Add(new XElement("VoiceHeader", VoiceHeader));

            if (!string.IsNullOrEmpty(LanguageHeader)) { orderTag.Add(new XElement("LanguageHeader", LanguageHeader)); }

            //throttling tags
            switch (ThrottleType)
            {
                case VoiceThrottleType.None:
                    orderTag.Add(new XElement("ThrType"));
                    orderTag.Add(new XElement("ThrNum"));
                    break;
                case VoiceThrottleType.CallsPerHour:
                    orderTag.Add(new XElement("ThrType", "calls per hour"));
                    orderTag.Add(new XElement("ThrNum", ThrottleNumber));
                    break;
                case VoiceThrottleType.MaximumCalls:
                    orderTag.Add(new XElement("ThrType", "maximum calls"));
                    orderTag.Add(new XElement("ThrNum", ThrottleNumber));
                    break;
                default:
                    throw new FormatException("Invalid Throttle Type.");
            }

            var broadcastElements = _broadcastInfo.GetBroadcastElements();
            foreach (var element in broadcastElements)
            {
                orderTag.Add(element);
            }

            return xmlDoc.ToString();
        }
    }
}

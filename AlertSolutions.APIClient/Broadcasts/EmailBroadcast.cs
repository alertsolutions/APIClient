using System;
using System.Collections.Generic;
using System.Xml.Linq;

using AlertSolutions.API.Documents;
using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Broadcasts
{
    [Serializable]
    public class EmailBroadcast : EmailBase, IBroadcast, IMultiLanguageBroadcast
    {
        // accessors for broadcast info ( goes into each class that inherits IBroadcastOrder)
        private BroadcastInfo _broadcastInfo = new BroadcastInfo();
        public string BillCode { get { return _broadcastInfo.BillCode; } set { _broadcastInfo.BillCode = value; } }
        public string ProjectCode { get { return _broadcastInfo.ProjectCode; } set { _broadcastInfo.ProjectCode = value; } }
        public ContactList List { get { return _broadcastInfo.Contacts; } set { _broadcastInfo.Contacts = value; } }
        public bool AutoLaunch { get { return _broadcastInfo.AutoLaunch; } set { _broadcastInfo.AutoLaunch = value; } }

        // fields
        public int NumberOfRedials { get; set; }
        public int NumberOfResends { get; set; }
        public string ResendInterval { get; set; }
        public string EmailHeader { get; set; }
        public string LanguageHeader { get; set; }
        public bool Dedup { get; set; }
        public string DedupField { get; set; }

        private List<string> _proofs;
        public List<string> Proofs
        {
            get { return _proofs ?? (_proofs = new List<string>()); }
            set { _proofs = value; }
        }

        public EmailBroadcast() : base()
        {
            this.TypeOfOrder = OrderType.EmailBroadcast;
            this.NumberOfRedials = 0;
            this.NumberOfResends = 0;
            this.EmailHeader = "";
            this.LanguageHeader = "";
            this.ResendInterval = "";
        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("NumberOfRedials", NumberOfRedials));
            orderTag.Add(new XElement("NumberOfResends", NumberOfResends));
            orderTag.Add(new XElement("ResendInterval", ResendInterval));
            orderTag.Add(new XElement("EmailField", EmailHeader)); // bad choice name in postAPI, expose it with a proper name in this library

            if (!string.IsNullOrEmpty(LanguageHeader)) { orderTag.Add(new XElement("LanguageHeader", LanguageHeader)); }

            //the rest of these are actually shared with ET, but the names of the elements
            // slightly differ in postAPI, hence the separate implementations
            orderTag.Add(new XElement("From", EmailFrom));
            orderTag.Add(new XElement("ReplyTo", EmailReplyTo));
            orderTag.Add(new XElement("Subject", EmailSubject));
            orderTag.Add(new XElement("Forward", IsForward ? "Yes" : "No"));
            orderTag.Add(new XElement("ReplaceLink", IsReplaceLink ? "Yes" : "No"));
            orderTag.Add(new XElement("Unsubscribe", IsUnsubscribe ? "Yes" : "No"));
            orderTag.Add(new XElement("Dedup", Dedup ? "Yes" : "No"));
            if (!string.IsNullOrEmpty(DedupField))
                orderTag.Add(new XElement("DedupField", DedupField));

            orderTag.Add(GetProofs());

            var broadcastElements = _broadcastInfo.GetBroadcastElements();
            foreach (var element in broadcastElements)
            {
                orderTag.Add(element);
            }

            return xmlDoc.ToString();
        }

        private XElement GetProofs()
        {
            var xProofs = new XElement("Proofs");
            if (Proofs.Count > 0)
            {
                Proofs.ForEach(p => xProofs.Add(new XElement("Proof", p)));
            }
            else
            {
                xProofs.Add(new XElement("Proof"));
            }
            return xProofs;
        }
    }
}

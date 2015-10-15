using System;
using System.Collections.Generic;
using System.Xml.Linq;

using AlertSolutions.API.Documents;
using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Broadcasts
{
    [Serializable]
    public class SMSBroadcast : SMSBase, IBroadcast, IMultiLanguageBroadcast
    {
        // accessors for broadcast info ( goes into each class that inherits IBroadcastOrder)
        private BroadcastInfo _broadcastInfo = new BroadcastInfo();
        public string BillCode { get { return _broadcastInfo.BillCode; } set { _broadcastInfo.BillCode = value; } }
        public string ProjectCode { get { return _broadcastInfo.ProjectCode; } set { _broadcastInfo.ProjectCode = value; } }
        public ContactList List { get { return _broadcastInfo.Contacts; } set { _broadcastInfo.Contacts = value; } }
        public bool AutoLaunch { get { return _broadcastInfo.AutoLaunch; } set { _broadcastInfo.AutoLaunch = value; } }

        //fields
        public bool Dedup { get; set; }
        public string DedupField { get; set; }
        public bool Restart { get; set; } 
        public string SMSHeader { get; set; }
        public string LanguageHeader { get; set; }
        private List<string> _proofs;
        public List<string> Proofs
        {
            get { if (_proofs == null) { _proofs = new List<string>(); } return _proofs; }
            set { _proofs = value; }
        }

        public SMSBroadcast()
        {
            this.Dedup = true;
            this.DedupField = "";
            this.TypeOfOrder = OrderType.SMSBroadcast;
            this.Restart = false;        
            this.SMSHeader = "";
            this.LanguageHeader = "";

        }

        public override string ToXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("Dedup", Dedup ? "Yes" : "No"));
            if(!string.IsNullOrEmpty(DedupField))
                orderTag.Add(new XElement("DedupField", DedupField));

            orderTag.Add(new XElement("Restart", Restart ? "Yes" : "No"));
            orderTag.Add(new XElement("SMSHeader", SMSHeader));
            orderTag.Add(GetProofs());

            if (!string.IsNullOrEmpty(LanguageHeader)) { orderTag.Add(new XElement("LanguageHeader", LanguageHeader)); }

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

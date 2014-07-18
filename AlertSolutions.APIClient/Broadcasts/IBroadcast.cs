using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Documents;
using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Broadcasts
{
    public interface IMultiLanguageBroadcast : IBroadcast
    {
        string LanguageHeader { get; set; }
    }
    // enforces fields so orders using it will have these fields accessible to the user
    public interface IBroadcast : IOrder
    {
        string BillCode { get; set; }
        string ProjectCode { get; set; }
        ContactList List { get; set; }
        bool AutoLaunch { get; set; }
        bool Dedup { get; set; }
    }

    // common code for broadcast orders
    internal class BroadcastInfo
    {
        public string BillCode { get; set; }
        public string ProjectCode { get; set; }
        public ContactList Contacts { get; set; }
        public bool AutoLaunch { get; set; }

        public BroadcastInfo()
        {
            Contacts = null; // default to null, users responsiblity to set
            BillCode = "";
            ProjectCode = "";
            AutoLaunch = true;
        }

        public List<XElement> GetBroadcastElements()
        {
            if (Contacts == null)
            {
                throw new FormatException("Must set a contact list for a broadcast.");
            }

            var xList = new List<XElement>
                {
                    new XElement("BillCode", BillCode),
                    new XElement("Project", ProjectCode),
                    new XElement("AutoLaunch", AutoLaunch ? "Yes" : "No")
                };

            var listElements = Contacts.ToXml();
            xList.AddRange(listElements);

            return xList;
        }
    }
}

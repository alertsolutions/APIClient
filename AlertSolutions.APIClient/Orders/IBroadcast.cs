using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlertSolutions.API.Orders
{
    // enforces fields so orders using it will have these fields accessible to the user
    public interface IBroadcast : IOrder
    {
        string BillCode { get; set; }
        string ProjectCode { get; set; }
        DateTime Date { get; set; }
        ContactList List { get; set; }
    }

    // common code for broadcast orders
    internal class BroadcastInfo
    {
        public string BillCode { get; set; }
        public string ProjectCode { get; set; }
        public DateTime Date { get; set; }
        public ContactList Contacts { get; set; }
        public bool AutoLaunch { get; set; }

        public BroadcastInfo()
        {
            Contacts = null; // default to null, users responsiblity to set
            BillCode = "";
            ProjectCode = "";
            Date = DateTime.Now;
            AutoLaunch = true;
        }

        public List<XElement> GetBroadcastElements()
        {
            if (Contacts == null)
            {
                throw new Exception("Must set a contact list for a broadcast.");
            }

            var xList = new List<XElement>();

            xList.Add(new XElement("BillCode", BillCode));
            xList.Add(new XElement("Project", ProjectCode));
            xList.Add(new XElement("Date", Date.ToString("yyyy-MM-dd")));
            xList.Add(new XElement("Time", Date.ToString("HH:mm")));
            xList.Add(new XElement("AutoLaunch", AutoLaunch ? "Yes" : "No"));

            var listElements = Contacts.ToXml();
            foreach (var el in listElements)
            {
                xList.Add(el);
            }

            return xList;
        }
    }
}

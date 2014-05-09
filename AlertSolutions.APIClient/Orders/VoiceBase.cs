using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Documents;

namespace AlertSolutions.API.Orders
{
    // handle code common to VT and VL
    [Serializable]
    public abstract class VoiceBase : Order
    {
        public List<VoiceDocument> Documents { get; set; }

        public string CallerID { get; set; }
        public DateTime StopTimeUTC { get; set; }
        public DateTime RestartTimeUTC { get; set; }
        public int DurationHours { get; set; }

        public Dictionary<string, string> HotKeys { get; set; }

        public CallScript CallScript { get; set; }

        protected VoiceBase()
        {
            CallScript = null;
            HotKeys = new Dictionary<string, string>();
            CallerID = "0";
            Documents = new List<VoiceDocument>();
        }

        protected override XDocument BuildXml()
        {
            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("CallerID", CallerID));
            
            if (DurationHours != 0)
            {
                var ts = new TimeSpan(0, DurationHours, 0,0);
                orderTag.Add(new XElement("Duration", ts.Days +  "." + ts.Hours.ToString("0#") + ":00"));
            }
            else
            {
                if (StopTimeUTC == DateTime.MinValue)
                {
                    throw new ArgumentException("StopTimeUTC is a required field.");
                }
                orderTag.Add(new XElement("StopDateTime", StopTimeUTC.ToString("yyyy-MM-dd HH:mm")));
            }
            orderTag.Add(new XElement("RestartTime", RestartTimeUTC.ToString("HH:mm")));

            if (CallScript != null)
                orderTag.Add(CallScript.ToXml());
            else
                orderTag.Add(new XElement("CallScript"));

            var hotKeyElements = GetHotKeyElements();
            foreach (var el in hotKeyElements)
            {
                orderTag.Add(el);
            }


            if(Documents == null)
                Documents = new List<VoiceDocument>();

            if (Documents.Count < 1)
                throw new FormatException("Must have at least one voice document.");

            var xDocs = new XElement("Documents");
            Documents.ForEach(doc => xDocs.Add(doc.ToXml()));
            orderTag.Add(xDocs);

            return xmlDoc;
        }

        private List<XElement> GetHotKeyElements()
        {
            var xList = new List<XElement>();
            var hotkeyNames = new Dictionary<string, string>()
            { 
                { "HotZero", "0" }, 
                { "HotOne", "1" },
                { "HotTwo", "2" },
                { "HotThree", "3" },
                { "HotFour", "4" },
                { "HotFive", "5" },
                { "HotSix", "6" },
                { "HotSeven", "7" },
                { "HotEight", "8" },
                { "HotNine", "9" },
                { "HotStar", "*" },
                { "HotPound", "#" } 
            };
            
            foreach(var name in hotkeyNames)
            {
                if (HotKeys.ContainsKey(name.Value))
                {
                    xList.Add(new XElement(name.Key, HotKeys[name.Value]));
                }
                else
                {
                    xList.Add(new XElement(name.Key));
                }
            }

            return xList;
        }
    }
}

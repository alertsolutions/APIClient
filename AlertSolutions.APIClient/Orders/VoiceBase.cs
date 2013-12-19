﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using AlertSolutions.API.Documents;

namespace AlertSolutions.API.Orders
{
    // handle code common to VT and VL
    public abstract class VoiceBase : Order, IContainDocuments
    {
        // accessors for document info ( goes into each class that inherits IContainDocuments)
        private DocumentInfo _documentInfo = new DocumentInfo();
        public List<Document> Documents {
            get { return _documentInfo.Documents; } 
            set { _documentInfo.Documents = value; } 
        }

        public string CallerID { get; set; }
        public DateTime StopTimeUTC { get; set; }
        public DateTime RestartTimeUTC { get; set; }
        public int DurationHours { get; set; }

        public Dictionary<string, string> HotKeys { get; set; }

        public CallScript CallScript { get; set; }

        internal VoiceBase()
        {
            CallScript = null;
            HotKeys = new Dictionary<string, string>();

            CallerID = "0";
            
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

            var voiceDocs = _documentInfo.Documents.FindAll(s => s.GetType() == typeof(VoiceDocument));

            if(_documentInfo.Documents.Count != voiceDocs.Count)
            {
                throw new FormatException("Must only use a voice document for a voice order.");
            }

            orderTag.Add(_documentInfo.GetDocuments());

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

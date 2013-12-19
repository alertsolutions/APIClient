using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace AlertSolutions.API
{
    public class OrderReportBuilder
    {
        public List<T> FromXml<T>(OrderType orderType, string xml) where T : new()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            string xPath = GetXPathForOrderType(orderType);
            var xnList = xmlDoc.SelectNodes(xPath); 
            List<T> reportItems = new List<T>();
            foreach (XmlNode xn in xnList)
            {
                T reportItem = new T();
                try
                {
                    var itemType = typeof(T);
                    var properties = itemType.GetProperties();

                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(Int32))
                        {
                            property.SetValue(reportItem, int.Parse(xn[property.Name].InnerText), null);
                        }
                        else if (property.PropertyType == typeof(Decimal))
                        {
                            property.SetValue(reportItem, Decimal.Parse(xn[property.Name].InnerText), null);
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            property.SetValue(reportItem, DateTime.Parse(xn[property.Name].InnerText), null);
                        }
                        else if (property.PropertyType == typeof(Guid))
                        {
                            property.SetValue(reportItem, Guid.Parse(xn[property.Name].InnerText), null);
                        }
                        else
                        {
                            property.SetValue(reportItem, xn[property.Name].InnerText, null);
                        }
                    }
                    reportItems.Add(reportItem);
                }
                catch (Exception)
                {
                }
            }

            return reportItems;
        }
  
        private string GetXPathForOrderType(OrderType orderType)
        {
            if (orderType == OrderType.EmailBroadcast)
            {
                return "/Report/EB";
            }
            if (orderType == OrderType.VoiceBroadcast)
            {
                return "/Report/VL";
            }
            if (orderType == OrderType.SMSBroadcast)
            {
                return "/Report/ML";
            }
            throw new NotImplementedException("Only EmailBroadcast, VoiceBroadcast, and SMSBroadcast are supported.");
        }
    }
}
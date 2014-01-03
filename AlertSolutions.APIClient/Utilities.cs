using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AlertSolutions.API.Orders;

namespace AlertSolutions.API
{
    public static class Utilities
    {

        internal static void StripNodeFromElement(XElement element, string nodeName)
        {
            (from node in element.Descendants(nodeName) select node).First().Remove();
        }

        internal static DateTime ConvertToEasternTime(DateTime utcTime)
        {
            TimeZoneInfo utcZone;
            try
            {
                utcZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            }
            catch (Exception)
            {
                utcZone = TimeZoneInfo.FromSerializedString("UTC;0;UTC;UTC;UTC;;");
            }
            TimeZoneInfo etZone;
            try
            {
                etZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch (Exception)
            {
                etZone = TimeZoneInfo.FromSerializedString("Eastern Standard Time;-300;(UTC-05:00) Eastern Time (US & Canada);" +
                    "Eastern Standard Time;Eastern Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];" +
                    "[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];");
            }
            var offsetTime = TimeZoneInfo.ConvertTime(utcTime, utcZone, etZone);

            return offsetTime;
        }

        public static IOrder OffsetOrderTimeFieldsToEasternStandard(IOrder order)
        {
            // convert sendtime to eastern
            // (since postAPI currently uses that instead of UTC like it should)
            order.SendTimeUTC = ConvertToEasternTime(order.SendTimeUTC);

            // likewise convert stop and start time to eastern
            if (order.HasProperty("StopTimeUTC"))
            {
                var prop = order.GetType().GetProperty("StopTimeUTC");
                var offsetTime = ConvertToEasternTime(Convert.ToDateTime(prop.GetValue(order, null)));
                prop.SetValue(order, offsetTime, null);
            }

            if (order.HasProperty("RestartTimeUTC"))
            {
                var prop = order.GetType().GetProperty("RestartTimeUTC");
                var offsetTime = ConvertToEasternTime(Convert.ToDateTime(prop.GetValue(order, null)));
                prop.SetValue(order, offsetTime, null);
            }

            // only on voice for now
            if (order.HasProperty("StopDateTimeUTC"))
            {
                var prop = order.GetType().GetProperty("StopDateTimeUTC");
                var offsetTime = ConvertToEasternTime(Convert.ToDateTime(prop.GetValue(order, null)));
                prop.SetValue(order, offsetTime, null);
            }
            
            return order;
        }

        // useful internal extension for checking orders since they might have inconsistent properties (stoptime, etc)
        internal static bool HasProperty(this object objectToCheck, string propertyName)
        {
            var type = objectToCheck.GetType();
            return type.GetProperty(propertyName) != null;
        }
    }
}

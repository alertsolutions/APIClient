using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AlertSolutions.API
{
    public enum OrderType
    {
        VoiceBroadcast,
        SMSBroadcast,
        EmailBroadcast,
        FaxBroadcast,

        VoiceMessage,
        SMSMessage,
        EmailMessage,
        FaxMessage,
    }

    public static class OrderTypeUtil
    {
        private static readonly Dictionary<OrderType, string> _enumDetails =
            new Dictionary<OrderType, string>()
                {
                    {OrderType.VoiceBroadcast, "VL"},
                    {OrderType.SMSBroadcast, "ML"},
                    {OrderType.EmailBroadcast, "EB"},
                    {OrderType.FaxBroadcast, "WL"},

                    {OrderType.VoiceMessage, "VT"},
                    {OrderType.SMSMessage, "MT"},
                    {OrderType.EmailMessage, "ET"},
                    {OrderType.FaxMessage, "TL"},
                };

        public static string GetCode(OrderType type)
        {
            return _enumDetails[type];
        }

        public static OrderType ParseCode(string orderTypeCode)
        {
            if (_enumDetails.ContainsValue(orderTypeCode.ToUpper()))
            {
                return _enumDetails.FirstOrDefault(x => x.Value == orderTypeCode).Key;
            }

            throw new InvalidEnumArgumentException(orderTypeCode + " is not a valid OrderType.");
        }
    }
}

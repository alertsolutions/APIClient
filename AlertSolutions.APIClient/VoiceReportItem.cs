using System;

namespace AlertSolutions.API
{
    public class VoiceReportItem
    {
        public string JobID { get; set; }

        public string PhoneNumber { get; set; }

        public int Duration { get; set; }

        public Decimal Rate { get; set; }

        public Decimal Cost { get; set; }

        public string Status { get; set; }

        public string Error { get; set; }

        public string DeliveryMethod { get; set; }

        public string KeyPress { get; set; }

        public DateTime Timestamp { get; set; }

        public string ProjectCode { get; set; }

        public string Name { get; set; }

        public string VoicePhone { get; set; }
    }
}
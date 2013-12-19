using System;

namespace AlertSolutions.API
{
    public class SmsReportItem
    {
        public string FormID { get; set; }

        public Guid UNQID { get; set; }

        public string OrderID { get; set; }

        public string Project { get; set; }

        public string CellPhoneNumber { get; set; }

        public int Sent { get; set; }

        public int SendFailed { get; set; }
    }
}
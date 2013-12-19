using System;

namespace AlertSolutions.API
{
    public class EmailReportItem
    {
        public string OrderId { get; set; }

        public string Project { get; set; }

        public string EmailAddress { get; set; }

        public int OpenCount { get; set; }

        public DateTime LastOpened { get; set; }

        public string JobStatus { get; set; }

        public string Result { get; set; }

        public string Error { get; set; }

        public DateTime Timestamp { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
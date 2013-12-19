using System;

namespace AlertSolutions.API
{
    public class OrderReport
    {
        public int OrderID { get; set; }
        public OrderType OrderType { get; set; }

        public RequestResultType RequestResult { get; set; }
        public string RequestErrorMessage { get; set; }

        public string OrderStatus { get; set; }
        public string ReportData { get; set; }
    }
}

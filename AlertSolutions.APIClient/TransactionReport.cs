using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Orders
{
    //probably should have inherited from OrderReport
    public class TransactionReport
    {
        public int OrderID { get; set; }
        public string Unqid { get; set; } // unique transactional id
        public OrderType OrderType { get; set; }

        public RequestResultType RequestResult { get; set; }
        public string RequestErrorMessage { get; set; }

        public string OrderStatus { get; set; }
        public string ReportData { get; set; }
    }
}

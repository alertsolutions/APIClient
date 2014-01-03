using System;

namespace AlertSolutions.API
{
    [Serializable]
    public class OrderResponse
    {
        public int OrderID { get; set; }
        public string Unqid { get; set; } // unique transactional id (empty if broadcast types)
        public OrderType OrderType { get; set; }

        public int ResponseTime { get; set; }

        public RequestResultType RequestResult { get; set; }
        public string RequestErrorMessage { get; set; }
    }
}

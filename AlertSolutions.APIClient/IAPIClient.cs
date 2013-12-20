using System.Xml.Linq;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using AlertSolutions.API.Templates;

namespace AlertSolutions.API
{
    public interface IAPIClient
    {
        /// <summary>
        /// If activated the client pretends it's sending broadcasts or messages.
        /// The client still works the same, but never actually sends messages to the API.
        /// This is useful for testing, when you don't really want to send broadcasts or messages to contacts. 
        /// </summary>
        bool TestMode { get; set; }

        /// <summary>
        /// Sends only broadcasts.
        /// </summary>
        OrderResponse LaunchBroadcast(IBroadcast broadcast);

        /// <summary>
        /// Sends only messages.
        /// </summary>
        OrderResponse LaunchMessage(IMessage message);

        /// <summary>
        /// Sends an order. All broadcasts and messages are orders.
        /// </summary>
        OrderResponse SendOrder(IOrder order);

        /// <summary>
        /// STRONGLY DISCOURAGE USE OF THIS OVERLOAD UNLESS YOU ABSOLUTELY NEED IT!
        /// Word Of Warning: the api works on eastern time, this client works on utc
        /// so you give it a utc date, the client will do the conversion (until such time that the
        /// api does use utc, rendering converting redundant).
        /// HOWEVER, this method directly consumes the given xml
        /// IT DOES NOT CONVERT FROM UTC TO EASTERN TIME!
        /// so anyone using this method will have to convert the times in their orders manually!
        /// </summary>
        OrderResponse SendOrder(string xml);

        OrderResponse SendOrder(XDocument xml);

        /// <summary>
        /// Gets a report for the associated broadcast or message
        /// </summary>
        OrderReport GetOrderReport(int OrderID, OrderType type, ReportReturnType returnType);

        OrderReport GetOrderReport(OrderResponse response, ReportReturnType returnType);

        /// <summary>
        /// Gets a report for the associated message using the message's transactionID
        /// </summary>
        TransactionReport GetTransactionReport(string transactionID, OrderType type, ReportReturnType returnType);

        TransactionReport GetTransactionReport(OrderResponse response, ReportReturnType returnType);

        /// <summary>
        /// Indicates if the credentials given to the client are valid.
        /// </summary>
        bool Authenticated();

        /// <summary>
        /// Cancels an order. All broadcasts and messages are orders.
        /// </summary>
        string CancelOrder(OrderResponse response);

        string CancelOrder(int orderid, OrderType type);

        /// <summary>
        /// Returns a collection of contact lists associated with the user.
        /// </summary>
        string GetLists(ReportReturnType returnType);

        TemplateResponse SendTemplates(Template template);
    }
}
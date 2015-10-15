using System;
using System.IO;
using System.Text;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.Xml.Linq;
using AlertSolutions.API.Orders;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Templates;

namespace AlertSolutions.API
{
    public enum RequestResultType
    {
        TestMode,
        Error,
        Success,
    }

    public enum ReportReturnType
    {
        XML,
        CSV,
    }

    public enum CancelStatusCode
    {
        Success = 1,
        DoesNotExist = -1,
        NotScheduled = -2,
        UnknownError = 0
    }

    public class ApiClient : IAPIClient
    {
        private string _url;
        private string _user;
        private string _password;
        private string _sessionId;
        private bool isInitialized = false;
        IWebClientProxy webClientProxy = ApiClientResolver.Instance.Container.Resolve<IWebClientProxy>();

        /// <summary>
        /// If activated the client pretends it's sending broadcasts or messages.
        /// The client still works the same, but never actually sends messages to the API.
        /// This is useful for testing, when you don't really want to send broadcasts or messages to contacts. 
        /// </summary>
        public bool TestMode { get; set; }

        public void InitializeWithUser(string url, string user, string password)
        {
            _url = url;
            _user = user;
            _password = password;
            TestMode = false;
            this.isInitialized = true;
        }

        public void InitializeWithUserAndSession(string url, string user, string password, string sessionId)
        {
            _url = url;
            _user = user;
            _password = password;
            _sessionId = sessionId;
            TestMode = false;
            this.isInitialized = true;
        }

        public void InitializeWithSession(string url, string sessionId)
        {
            _url = url;
            _sessionId = sessionId;
            TestMode = false;
            this.isInitialized = true;
        }

        public void CheckInitialized()
        {
            if (!this.isInitialized) throw new ApplicationException("APIClient.Initialize must be called prior to this method");
        }

        public ApiClient() { }

        public ApiClient(string url, string user, string password)
        {
            InitializeWithUser(url, user, password);
        }

        /// <summary>
        /// Sends only broadcasts.
        /// </summary>
        public OrderResponse LaunchBroadcast(IBroadcast broadcast)
        {
            return SendOrder(broadcast);
        }

        /// <summary>
        /// Sends only messages.
        /// </summary>
        public OrderResponse LaunchMessage(IMessage message)
        {
            return SendOrder(message);
        }

        /// <summary>
        /// Sends an order. All broadcasts and messages are orders.
        /// </summary>
        public OrderResponse SendOrder(IOrder order)
        {
            order = Utilities.OffsetOrderTimeFieldsToEasternStandard(order);
            var xml = order.ToXml();
            return SendOrder(xml);
        }

        /// <summary>
        /// STRONGLY DISCOURAGE USE OF THIS OVERLOAD UNLESS YOU ABSOLUTELY NEED IT!
        /// Word Of Warning: the api works on eastern time, this client works on utc
        /// so you give it a utc date, the client will do the conversion (until such time that the
        /// api does use utc, rendering converting redundant).
        /// HOWEVER, this method directly consumes the given xml
        /// IT DOES NOT CONVERT FROM UTC TO EASTERN TIME!
        /// so anyone using this method will have to convert the times in their orders manually!
        /// </summary>
        public OrderResponse SendOrder(string xml)
        {
            return SendOrder(XDocument.Parse(xml));
        }

        public OrderResponse SendOrder(XDocument xml)
        {
            CheckInitialized();
            var type = xml.Element("Orders").Element("Order")
                .Attribute("Type").Value;
            var response = new OrderResponse(type);

            if (TestMode)
            {
                response.InitializeTestMode();
                return response;
            }

            try
            {
                response = SendXml(type, xml);
            }
            catch (Exception ex)
            {
                response.RequestResult = RequestResultType.Error;
                response.RequestErrorMessage = GetErrorMessage(ex);
            }

            return response;
        }

        private OrderResponse SendXml(string type, XDocument xml)
        {
            //send for real
            var result = new OrderResponse(type);
            var sw = Stopwatch.StartNew();
            try
            {
                var url = BuildUrl(OrderTypeUtil.GetCode(result.OrderType));
                var response = webClientProxy.UploadString(url, xml.ToString());
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                var xmlResult = xmlDoc.SelectSingleNode("/PostAPIResponse/SaveTransactionalOrderResult") ??
                                xmlDoc.SelectSingleNode("/PostAPIResponse/Exception");
                result.ParseXml(xmlResult);
            }
            finally
            {
                sw.Stop();
                result.ResponseTime = sw.Elapsed.Seconds;
            }
            return result;
        }

        private string GetErrorMessage(Exception error)
        {
            var errorType = error.GetType();
            if (errorType == typeof(WebException) || errorType == typeof(TimeoutException))
                return "Server Error: " + error;
            return "Error: " + error;
        }

        private string BuildUrl(string type)
        {
            StringBuilder location = new StringBuilder("");
            location.Append(_url);
            location.Append("/xml/");
            location.Append(type + "new.aspx?");
            location.Append("UserName=" + _user);
            location.Append("&UserPassword=" + _password);
            location.Append("&PostWay=sync");
            location.Append("&CSVFile=");
            if (!string.IsNullOrEmpty(_sessionId))
                location.Append("&SessionID=" + _sessionId);
            return location.ToString();
        }

        /// <summary>
        /// Gets a report for the associated broadcast or message
        /// </summary>
        public OrderReport GetOrderReport(int OrderID, OrderType type, ReportReturnType returnType)
        {
            CheckInitialized();
            return GetOrderReport(new OrderResponse(type) { OrderID = OrderID }, returnType);
        }

        public OrderReport GetOrderReport(OrderResponse response, ReportReturnType returnType)
        {
            CheckInitialized();
            var trans = GetTransactionReport(response, returnType);
            return new OrderReport
                {
                    OrderID = trans.OrderID,
                    OrderType = trans.OrderType,
                    RequestResult = trans.RequestResult,
                    RequestErrorMessage = trans.RequestErrorMessage,
                    OrderStatus = trans.OrderStatus,
                    ReportData = trans.ReportData
                };
        }

        /// <summary>
        /// Gets a report for the associated message using the message's transactionID
        /// </summary>
        public TransactionReport GetTransactionReport(string transactionID, OrderType type, ReportReturnType returnType)
        {
            CheckInitialized();
            return GetTransactionReport(new OrderResponse(type) { Unqid = transactionID }, returnType);
        }

        public TransactionReport GetTransactionReport(OrderResponse response, ReportReturnType returnType)
        {
            CheckInitialized();
            var error = "none";
            var requestResult = RequestResultType.Success;
            var reportData = "";
            var orderStatus = "";

            OrderType type = response.OrderType;

            if (TestMode)
            {
                reportData = "This is a test order report. Order ID: ( " + response.OrderID + " ) Type: ( " + type.ToString() + " )";
                requestResult = RequestResultType.TestMode;
                orderStatus = "Test Mode.";
                return new TransactionReport(response, requestResult, error, orderStatus, reportData);
            }

            try
            {
                var location = BuildReportUrl(response, returnType, type);
                reportData = webClientProxy.UploadString(location, "");

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(reportData);
                //assumes there is only one of this kind of tag
                var xmlResult = xmlDoc.SelectSingleNode("/PostAPIResponse/SaveTransactionalOrderResult");
                if (xmlResult["status"] != null)
                    orderStatus = xmlResult["status"].InnerText;
                else
                {
                    requestResult = RequestResultType.Error;
                    error = xmlResult["Exception"].InnerText;
                }
            }
            catch (Exception ex)
            {
                requestResult = RequestResultType.Error;
                error = "An error occurred while requesting the order report. " + ex;
            }

            return new TransactionReport(response, requestResult, error, orderStatus, reportData);
        }

        private string BuildReportUrl(OrderResponse response, ReportReturnType returnType, OrderType type)
        {
            var location = new StringBuilder(_url);

            location.Append(@"/" + OrderTypeUtil.GetCode(type));
            location.Append(type == OrderType.FaxMessage ? "ReportByUnqid.aspx?" : "report.aspx?");

            location.Append("UserName=" + _user);
            location.Append("&UserPassword=" + _password);
            location.Append("&ReturnType=" + returnType);

            if (IsTransaction(type))
                location.Append("&Unqid=" + response.Unqid);

            location.Append("&OrderID=" + response.OrderID);
            return location.ToString();
        }

        private bool IsTransaction(OrderType type)
        {
            return type == OrderType.EmailMessage || type == OrderType.SMSMessage ||
                   type == OrderType.VoiceMessage || type == OrderType.FaxMessage;
        }

        /// <summary>
        /// Indicates if the credentials given to the client are valid.
        /// </summary>
        public bool Authenticated()
        {
            CheckInitialized();
            try
            {
                var response = CancelOrder(0, OrderType.EmailBroadcast);

                // -1 is error, 0 is bad user/password
                return ((int)response.StatusCode) != 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to authenticate user. " + ex);
            }
        }

        /// <summary>
        /// Cancels an order. All broadcasts and messages are orders.
        /// </summary>
        public ICancelResponse CancelOrder(OrderResponse response)
        {
            CheckInitialized();
            return CancelOrder(response.OrderID, response.OrderType);
        }

        public ICancelResponse CancelOrder(int orderid, OrderType type)
        {
            CheckInitialized();
            var location = new StringBuilder();
            location.Append(_url);
            location.Append("/xml/CancelOrder.aspx?");
            location.Append("UserName=" + _user);
            location.Append("&UserPassword=" + _password);
            if (!string.IsNullOrEmpty(_sessionId))
                location.Append("&SessionID=" + _sessionId);

            var xdoc = new XElement("Cancels",
                new XElement("Cancel", new XAttribute("Type", OrderTypeUtil.GetCode(type)), orderid));
            
            var responseXml =  webClientProxy.UploadString(location.ToString(), xdoc.ToString(SaveOptions.DisableFormatting));
            var cancelResponse = new CancelResponse(responseXml);
            return cancelResponse;
        }

        /// <summary>
        /// Returns a collection of contact lists associated with the user.
        /// </summary>
        public string GetLists(ReportReturnType returnType)
        {
            CheckInitialized();
            var location = new StringBuilder();
            location.Append(_url);
            location.Append("/xml/lists.aspx?");
            location.Append("UserName=" + _user);
            location.Append("&UserPassword=" + _password);
            location.Append("&ReturnType=" + returnType);

            return webClientProxy.DownloadString(location.ToString());
        }

        public TemplateResponse SendTemplates(ITemplate template)
        {
            var templateId = -1;
            var error = "unknown";
            var status = RequestResultType.Error;

            var xml = template.ToXml();

            StringBuilder location = new StringBuilder("");
            location.Append(_url);
            location.Append("/xml/TemplateSubmit.aspx?");
            location.Append("UserName=");
            location.Append(_user);
            location.Append("&UserPassword=");
            location.Append(_password);

            string response = "";

            try
            {
                response = webClientProxy.UploadString(location.ToString(), xml.ToString());
            }
            catch (Exception ex)
            {
                status = RequestResultType.Error;
                error = GetErrorMessage(ex);
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);

            status = RequestResultType.Success; // we have xml and it parsed correctly, so here we know request was made successfully

            var xnList = xmlDoc.SelectNodes("Templates");

            if (xnList.Count == 0)
            {
                error = xmlDoc.SelectNodes("PostAPIResponse/SaveTemplateResult")[0]["Exception"].InnerText;
            }
            else
            {
                error = "none";

                foreach (XmlNode xn in xnList)
                {
                    try
                    {
                        templateId = Convert.ToInt32(xn["TemplateID"].InnerText);
                    }
                    catch (Exception)
                    {
                        status = RequestResultType.Error;
                        error = xn["Exception"].InnerText;
                    }
                }
            }

            return new TemplateResponse { TemplateId = templateId, Status = status, Error = error };
        }
    }
}
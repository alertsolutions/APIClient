using System;
using System.Text;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.Xml.Linq;
using AlertSolutions.API.Orders;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Templates;

//worthless change

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

    public class ApiClient : IAPIClient
    {
        private string _url;
        private string _user;
        private string _password;
        private bool isInitialized = false;
        IWebClientProxy webClientProxy = ApiClientResolver.Instance.Container.Resolve<IWebClientProxy>();


        /// <summary>
        /// If activated the client pretends it's sending broadcasts or messages.
        /// The client still works the same, but never actually sends messages to the API.
        /// This is useful for testing, when you don't really want to send broadcasts or messages to contacts. 
        /// </summary>
        public bool TestMode { get; set; }

        public void Initialize(string url, string user, string password)
        {
            _url = url;
            _user = user;
            _password = password;
            TestMode = false;
            this.isInitialized = true;
        }

        public void CheckInitialized()
        {
            if (!this.isInitialized) throw new ApplicationException("APIClient.Initialized must be called prior to this method");
        }

        public ApiClient()
        {
        }

        public ApiClient(string url, string user, string password)
        {
            Initialize(url,user,password);
        }


        /// <summary>
        /// Sends only broadcasts.
        /// </summary>
        public OrderResponse LaunchBroadcast(IBroadcast broadcast)
        {
            CheckInitialized();
            return SendOrder(broadcast);
        }

        /// <summary>
        /// Sends only messages.
        /// </summary>
        public OrderResponse LaunchMessage(IMessage message)
        {
            CheckInitialized();
            return SendOrder(message);
        }


        /// <summary>
        /// Sends an order. All broadcasts and messages are orders.
        /// </summary>
        public OrderResponse SendOrder(IOrder order)
        {
            CheckInitialized();
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
            CheckInitialized();
            return SendOrder(XDocument.Parse(xml));
        }
        public OrderResponse SendOrder(XDocument xml)
        {
            CheckInitialized();
            var type = "";
            var orderid = -1;
            var transactionid = "";
            var error = "unknown";
            var status = RequestResultType.Error;
            var responseTime = -1;
            Stopwatch sw =  new Stopwatch();

            var element = xml.Element("Orders").Element("Order");
            type = element.Attribute("Type").Value;
                  
            try
            {
                if (TestMode)
                {
                    // give response indicating testmode
                    orderid = 0;
                    transactionid = "0";
                    responseTime = 0;
                    status = RequestResultType.TestMode;
                    error = "none";
                }
                else
                {
                    //send for real
                    StringBuilder location = new StringBuilder("");
                    location.Append(_url);
                    location.Append("/xml/");
                    location.Append(type);
                    location.Append("new.aspx?");
                    location.Append("UserName=");
                    location.Append(_user);
                    location.Append("&UserPassword=");
                    location.Append(_password);
                    location.Append("&PostWay=sync");
                    location.Append("&CSVFile=");

                    string response;
                    sw.Start();
                    response = webClientProxy.UploadString(location.ToString(), xml.ToString());
                    sw.Stop();

                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);
                    //assumes there is only one of this kind of tag
                    var xnList = xmlDoc.SelectNodes("/PostAPIResponse/SaveTransactionalOrderResult");

                    status = RequestResultType.Success; // we have xml and it parsed correctly, so here we know request was made successfully
                    error = "none";

                    foreach (XmlNode xn in xnList)
                    {
                        try
                        {
                            orderid = Convert.ToInt32(xn["OrderID"].InnerText);

                            if (type == OrderTypeUtil.GetCode(OrderType.EmailMessage) || type == OrderTypeUtil.GetCode(OrderType.SMSMessage) ||
                                type == OrderTypeUtil.GetCode(OrderType.VoiceMessage) || type == OrderTypeUtil.GetCode(OrderType.FaxMessage))
                                transactionid = xn["transactionID"].InnerText;
                        }
                        catch (NullReferenceException)
                        {
                            status = RequestResultType.Error;
                            error = xn["Exception"].InnerText;
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                status = RequestResultType.Error;
                error = "Server Error: " + wex;
            }
            catch (TimeoutException tex)
            {
                status = RequestResultType.Error;
                error = "Server Error: " + tex;
            }
            catch (Exception ex)
            {
                status = RequestResultType.Error;
                error = "Error: " + ex; 
            }
            finally
            {
                sw.Stop();
                responseTime = sw.Elapsed.Seconds;
            }
            
            return new OrderResponse
            {
                OrderID = orderid,
                Unqid = transactionid,
                OrderType = OrderTypeUtil.ParseCode(type),
                ResponseTime = responseTime,
                RequestResult = status,
                RequestErrorMessage = error,
            };
        }

        /// <summary>
        /// Gets a report for the associated broadcast or message
        /// </summary>
        public OrderReport GetOrderReport(int OrderID, OrderType type, ReportReturnType returnType)
        {
            CheckInitialized();
            return GetOrderReport(new OrderResponse() { OrderID = OrderID, OrderType = type }, returnType);
        }

        public OrderReport GetOrderReport(OrderResponse response, ReportReturnType returnType)
        {
            CheckInitialized();
            var trans = GetTransactionReport(response, returnType);
            return new OrderReport()
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
            return GetTransactionReport(new OrderResponse() { Unqid = transactionID, OrderType = type }, returnType);
        }

        public TransactionReport GetTransactionReport(OrderResponse response, ReportReturnType returnType)
        {
            CheckInitialized();
            var error = "unknown";
            var requestResult = RequestResultType.Error;
            var reportData = "";
            var orderStatus = "";

            OrderType type = response.OrderType;

            if (TestMode)
            {
                reportData = "This is a test order report. Order ID: ( " + response.OrderID + " ) Type: ( " + type.ToString() + " )";
                requestResult = RequestResultType.TestMode;
                orderStatus = "Test Mode.";
                error = "none";
            }
            else
            {
                try
                {
                    var location = new StringBuilder(_url);

                    if (type == OrderType.FaxMessage) // test for FaxMessage/TL since it's url doesn't follow the convention
                    {
                        location.Append(@"/");
                        location.Append(OrderTypeUtil.GetCode(type));
                        location.Append("ReportByUnqid.aspx?");
                    }
                    else
                    {
                        location.Append(@"/");
                        location.Append(OrderTypeUtil.GetCode(type));
                        location.Append("report.aspx?");
                    }

                    location.Append("UserName=");
                    location.Append(_user);
                    location.Append("&UserPassword=");
                    location.Append(_password);
                    location.Append("&ReturnType=");
                    location.Append(returnType.ToString());

                    if (type == OrderType.EmailMessage || type == OrderType.SMSMessage ||
                        type == OrderType.VoiceMessage || type == OrderType.FaxMessage)
                    {
                        location.Append("&Unqid=");
                        location.Append(response.Unqid);
                    }


                    location.Append("&OrderID=");
                    location.Append(response.OrderID);

                    reportData = webClientProxy.UploadString(location.ToString(), "");

                    requestResult = RequestResultType.Success;
                    error = "none";

                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(reportData);
                    //assumes there is only one of this kind of tag
                    var xnList = xmlDoc.SelectNodes("/PostAPIResponse/SaveTransactionalOrderResult");
                    foreach (XmlNode xn in xnList)
                    {
                        try
                        {
                            orderStatus = xn["status"].InnerText;
                        }
                        catch (NullReferenceException)
                        {
                            requestResult = RequestResultType.Error;
                            error = xn["Exception"].InnerText;
                        }
                    }
                }
                catch (Exception ex)
                {
                    requestResult = RequestResultType.Error;
                    error = "An error occurred while requesting the order report. " + ex.ToString();
                }
            }

            return new TransactionReport()
            {
                OrderID = response.OrderID,
                Unqid = response.Unqid,
                OrderType = response.OrderType,
                RequestResult = requestResult,
                RequestErrorMessage = error,
                OrderStatus = orderStatus,
                ReportData = reportData,
            };
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

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                var xnList = xmlDoc.SelectNodes("/PostAPIResponse/CancelOrderResult");
                int statusCode = 0;
                foreach (XmlNode xn in xnList)
                {
                    statusCode = Convert.ToInt32(xn["StatusCode"].InnerText);
                }
                if (statusCode == 0) // -1 is error, 0 is bad user/password
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to authenticate user. " + ex);
            }
        }

        /// <summary>
        /// Cancels an order. All broadcasts and messages are orders.
        /// </summary>
        public string CancelOrder(OrderResponse response)
        {
            CheckInitialized();
            return CancelOrder(response.OrderID, response.OrderType);
        }

        public string CancelOrder(int orderid, OrderType type)
        {
            CheckInitialized();
            var location = new StringBuilder();
            location.Append(_url);
            location.Append("/xml/CancelOrder.aspx?");
            location.Append("UserName=");
            location.Append(_user);
            location.Append("&UserPassword=");
            location.Append(_password);

            var xml = "<Cancels><Cancel Type=\"" + OrderTypeUtil.GetCode(type) + "\">" + orderid + "</Cancel></Cancels>";
            
            return webClientProxy.UploadString(location.ToString(), xml);
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
            location.Append("UserName=");
            location.Append(_user);
            location.Append("&UserPassword=");
            location.Append(_password);
            location.Append("&ReturnType=");
            location.Append(returnType.ToString());

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
            catch (WebException wex)
            {
                status = RequestResultType.Error;
                error = "Server Error: " + wex;
            }
            catch (TimeoutException tex)
            {
                status = RequestResultType.Error;
                error = "Server Error: " + tex;
            }
            catch (Exception ex)
            {
                status = RequestResultType.Error;
                error = "Error: " + ex;
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
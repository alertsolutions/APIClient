using System;
using System.Reflection;
using System.Xml.Linq;
using AlertSolutions.API;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using AlertSolutions.API.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AlertSolutions.APIClient.Test
{
    [TestClass]
    public class APIClientTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            ApiClient apiClient = new ApiClient();
            Assert.IsNotNull(apiClient);

            apiClient = new ApiClient("url", "user", "password");
            Assert.IsNotNull(apiClient);
        }

        [TestMethod]
        public void CallsFailWithoutInit01Test()
        {
            ApiClient apiClient = new ApiClient();
            try
            {
                apiClient.GetLists(ReportReturnType.CSV);
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("APIClient.Initialize must be called prior to this method",ae.Message);
            }
        }

        [TestMethod]
        public void CallsFailWithoutInit02Test()
        {
            ApiClient apiClient = new ApiClient();
            try
            {
                apiClient.GetOrderReport(1, OrderType.VoiceBroadcast, ReportReturnType.XML);
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("APIClient.Initialize must be called prior to this method", ae.Message);
            }
        }

        [TestMethod]
        public void GetListsTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.DownloadString(It.IsAny<string>(), It.IsAny<string>())).Returns(() =>
            {
                return "downloaded string";
            });
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "some result"; });
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(delegate { return mockWebClientProxy.Object; });
            ApiClient apiClient = new ApiClient();
            apiClient.InitializeWithUser("blah", "user", "password");
            string lists = apiClient.GetLists(ReportReturnType.XML);
            Assert.IsNotNull("downloaded string");
        }

        [TestMethod]
        public void CancelOrderTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "some result"; });
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(delegate { return mockWebClientProxy.Object; });
            ApiClient apiClient = new ApiClient();
            apiClient.InitializeWithUser("blah", "user", "password");
            string retval = apiClient.CancelOrder(new OrderResponse(OrderType.EmailBroadcast) { OrderID = 1 });
            Assert.AreEqual("some result", retval);
        }

        [TestMethod]
        public void LaunchBroadcastTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => "<PostAPIResponse><SaveTransactionalOrderResult><OrderID>12345</OrderID></SaveTransactionalOrderResult></PostAPIResponse>");
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(delegate { return mockWebClientProxy.Object; });

            var mockBroadcast = new Mock<IBroadcast>();
            mockBroadcast.Setup(x => x.ToXml()).Returns("<Orders><Order Type='EB'></Order></Orders>");

            ApiClient apiClient = new ApiClient("url", "user", "password");
            OrderResponse response = apiClient.LaunchBroadcast(mockBroadcast.Object);
            Assert.AreEqual(RequestResultType.Success, response.RequestResult);
            Assert.AreEqual(OrderType.EmailBroadcast, response.OrderType);
            Assert.AreEqual(12345, response.OrderID);
        }

        [TestMethod]
        public void LaunchMessageTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "<PostAPIResponse><SaveTransactionalOrderResult><OrderID>12345</OrderID><transactionID>transactionId</transactionID></SaveTransactionalOrderResult></PostAPIResponse>"; });
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(delegate { return mockWebClientProxy.Object; });

            var mockMessage = new Mock<IMessage>();
            mockMessage.Setup(x => x.ToXml()).Returns("<Orders><Order Type='ET'></Order></Orders>");

            ApiClient apiClient = new ApiClient("url", "user", "password");
            OrderResponse response = apiClient.LaunchMessage(mockMessage.Object);
            Assert.AreEqual(RequestResultType.Success, response.RequestResult);
            Assert.AreEqual(OrderType.EmailMessage, response.OrderType);
            Assert.AreEqual(12345, response.OrderID);
            Assert.AreEqual("transactionId", response.Unqid);
        }

        [TestMethod]
        public void SendTemplateTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "<Templates><TemplateID>123</TemplateID></Templates>"; });
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(delegate { return mockWebClientProxy.Object; });

            var mockMessage = new Mock<ITemplate>();
            mockMessage.Setup(x => x.ToXml()).Returns(XElement.Parse("<Templates><Template><FileName>template.html</FileName><FileBinary>fakefilebinary</FileBinary></Template></Templates>"));

            ApiClient apiClient = new ApiClient("url", "user", "password");
            TemplateResponse response = apiClient.SendTemplates(mockMessage.Object);
            Assert.AreEqual(RequestResultType.Success, response.Status);
            Assert.AreEqual(123, response.TemplateId);
        }

        [TestMethod]
        public void GetOrderReportTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "<PostAPIResponse><SaveTransactionalOrderResult><status>status</status></SaveTransactionalOrderResult></PostAPIResponse>"; });
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(delegate { return mockWebClientProxy.Object; });

            ApiClient apiClient = new ApiClient("url", "user", "password");
            OrderReport response = apiClient.GetOrderReport(12345, OrderType.EmailBroadcast, ReportReturnType.XML);
            Assert.AreEqual(RequestResultType.Success, response.RequestResult);
            Assert.AreEqual(OrderType.EmailBroadcast, response.OrderType);
            Assert.AreEqual(12345, response.OrderID);
            Assert.AreEqual("status", response.OrderStatus);
            Assert.AreEqual("<PostAPIResponse><SaveTransactionalOrderResult><status>status</status></SaveTransactionalOrderResult></PostAPIResponse>", response.ReportData);         
        }

        [TestMethod]
        public void GetTransactionReport()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "<PostAPIResponse><SaveTransactionalOrderResult><status>status</status></SaveTransactionalOrderResult></PostAPIResponse>"; });
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(delegate { return mockWebClientProxy.Object; });

            ApiClient apiClient = new ApiClient("url", "user", "password");
            TransactionReport response = apiClient.GetTransactionReport("transactionid", OrderType.EmailMessage, ReportReturnType.XML);
            Assert.AreEqual(RequestResultType.Success, response.RequestResult);
            Assert.AreEqual(OrderType.EmailMessage, response.OrderType);
            Assert.AreEqual("transactionid", response.Unqid);
            Assert.AreEqual("status", response.OrderStatus);
            Assert.AreEqual("<PostAPIResponse><SaveTransactionalOrderResult><status>status</status></SaveTransactionalOrderResult></PostAPIResponse>", response.ReportData);    
        }

        [TestMethod]
        public void InitializeSessionTest()
        {
            var ac = new ApiClient();
            var id = Guid.NewGuid().ToString();
            ac.InitializeWithSession("http://example.com/postapi", id);
            var method = typeof(ApiClient).GetMethod("BuildUrl", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method.Invoke(ac, new object [] { OrderTypeUtil.GetCode(OrderType.FaxMessage) });
            var gold = "http://example.com/postapi/xml/TLnew.aspx?UserName=&UserPassword=&PostWay=sync&CSVFile=&SessionID=" + id;
            Assert.AreEqual(gold, result);

            ac.InitializeWithUserAndSession("http://example.com/postapi", "jdoe", "p@ssw3rd", id);
            var result2 = method.Invoke(ac, new object [] { OrderTypeUtil.GetCode(OrderType.FaxBroadcast) });
            var gold2 = "http://example.com/postapi/xml/WLnew.aspx?UserName=jdoe&UserPassword=p@ssw3rd&PostWay=sync&CSVFile=&SessionID=" + id;
            Assert.AreEqual(gold2, result2);
        }

        [TestMethod]
        public void SendOrderTestModeTest()
        {
            var ac = new ApiClient();
            ac.InitializeWithUser("", "", "");
            ac.TestMode = true;
            var xmlStub = XDocument.Parse(@"<Orders><Order Type=""TL""></Order></Orders>");
            var response = ac.SendOrder(xmlStub);
            Assert.AreEqual(OrderType.FaxMessage, response.OrderType);
            Assert.AreEqual("none", response.RequestErrorMessage);
            Assert.AreEqual(RequestResultType.TestMode, response.RequestResult);
        }

        [TestMethod]
        public void SendOrderErrorTest()
        {
            var xmlStub = XDocument.Parse(@"<Orders><Order Type=""TL""></Order></Orders>");
            var xmlText = xmlStub.ToString();
            var wc = new Mock<IWebClientProxy>();
            var error = new TimeoutException("test exception");
            wc.Setup(w => w.UploadString(It.IsAny<string>(), xmlText, It.IsAny<string>()))
                .Throws(error);
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(r => wc.Object);
            var ac = new ApiClient();
            ac.InitializeWithUser("http://example.com/postapi", "jdoe", "password");
            var result = ac.SendOrder(xmlStub);
            Assert.AreEqual(RequestResultType.Error, result.RequestResult);
            Assert.AreEqual("Server Error: " + error, result.RequestErrorMessage);
        }

        [TestMethod]
        public void SendOrderBadXmlTest()
        {
            var xmlStub = XDocument.Parse(@"<Orders><Order Type=""ML""></Order></Orders>");

            var wc = new Mock<IWebClientProxy>();
            wc.Setup(w => w.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("<PostAPIResponse><Exception>missing BillCode tag</Exception></PostAPIResponse>");
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(r => wc.Object);

            var ac = new ApiClient();
            ac.InitializeWithUser("", "", "");
            var response = ac.SendOrder(xmlStub);
            Assert.AreEqual(OrderType.SMSBroadcast, response.OrderType);
            Assert.AreEqual("missing BillCode tag", response.RequestErrorMessage);
            Assert.AreEqual(RequestResultType.Error, response.RequestResult);
        }

        [TestMethod]
        public void GetDefaultErrorTest()
        {
            var ac = new ApiClient();
            ac.InitializeWithUser("", "", "");
            var method = typeof(ApiClient).GetMethod("GetErrorMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            var error = new Exception("test error");
            var message = method.Invoke(ac, new object [] { error });
            Assert.AreEqual("Error: " + error, message);
        }

        [TestMethod]
        public void GetTransactionReportTestModeTest()
        {
            var ac = new ApiClient();
            ac.InitializeWithSession("", "");
            ac.TestMode = true;
            var response = new OrderResponse(OrderType.FaxMessage);
            var report = ac.GetTransactionReport(response, ReportReturnType.XML);
            Assert.IsTrue(report.ReportData.StartsWith("This is a test order report."));
            Assert.AreEqual(RequestResultType.TestMode, report.RequestResult);
            Assert.AreEqual("Test Mode.", report.OrderStatus);
        }

        [TestMethod]
        public void GetTransactionReportServerErrorTest()
        {
            var wc = new Mock<IWebClientProxy>();
            wc.Setup(w => w.UploadString(It.IsAny<string>(), "", It.IsAny<string>()))
                .Returns("<PostAPIResponse><SaveTransactionalOrderResult><Exception>fake error</Exception></SaveTransactionalOrderResult></PostAPIResponse>");
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(r => wc.Object);
            var ac = new ApiClient();
            ac.InitializeWithSession("", "");
            var response = new OrderResponse(OrderType.FaxMessage);
            var report = ac.GetTransactionReport(response, ReportReturnType.XML);
            Assert.AreEqual(RequestResultType.Error, report.RequestResult);
            Assert.AreEqual("fake error", report.RequestErrorMessage);
        }

        [TestMethod]
        public void GetTransactionReportExceptionTest()
        {
            var error = new Exception("fake error");
            var wc = new Mock<IWebClientProxy>();
            wc.Setup(w => w.UploadString(It.IsAny<string>(), "", It.IsAny<string>()))
                .Throws(error);
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(r => wc.Object);

            var ac = new ApiClient();
            ac.InitializeWithSession("", "");

            var response = new OrderResponse(OrderType.FaxMessage);
            var report = ac.GetTransactionReport(response, ReportReturnType.XML);

            Assert.AreEqual(RequestResultType.Error, report.RequestResult);
            Assert.IsTrue(report.RequestErrorMessage.StartsWith("An error occurred"));
        }

        [TestMethod]
        public void AuthenticatedTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => "bad response");
            ApiClientResolver.Instance.Container.Register<IWebClientProxy>(x => mockWebClientProxy.Object);
            ApiClient apiClient = new ApiClient();
            apiClient.InitializeWithUser("blah", "user", "password");
            var thrown = false;
            try
            {
                apiClient.Authenticated();
                Assert.Fail();
            }
            catch (Exception)
            {
                thrown = true;
            }
            Assert.IsTrue(thrown);

            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => "<PostAPIResponse><CancelOrderResult><StatusCode>-1</StatusCode></CancelOrderResult></PostAPIResponse>");

            Assert.IsTrue(apiClient.Authenticated());
        }

        [TestCleanup]
        public void Cleanup()
        {
            ApiClientResolver.Instance.Reset();
        }
    }
}

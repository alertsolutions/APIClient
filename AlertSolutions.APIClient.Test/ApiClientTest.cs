using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSolutions.API;
using AlertSolutions.API.Broadcasts;
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
                Assert.AreEqual("APIClient.Initialized must be called prior to this method",ae.Message);
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
                Assert.AreEqual("APIClient.Initialized must be called prior to this method", ae.Message);
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
            ApiClientResolver.Instance.Container.Register(mockWebClientProxy.Object);
            ApiClient apiClient = new ApiClient();
            apiClient.Initialize("blah", "user", "password");
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
            ApiClientResolver.Instance.Container.Register(mockWebClientProxy.Object);
            ApiClient apiClient = new ApiClient();
            apiClient.Initialize("blah", "user", "password");
            string retval = apiClient.CancelOrder(new OrderResponse{OrderID = 1, OrderType = OrderType.EmailBroadcast});
            Assert.AreEqual("some result", retval);
        }

        [TestMethod]
        public void LaunchBroadcastTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "<PostAPIResponse><SaveTransactionalOrderResult><OrderID>12345</OrderID></SaveTransactionalOrderResult></PostAPIResponse>"; });
            ApiClientResolver.Instance.Container.Register(mockWebClientProxy.Object);

            var mockBroadcast = new Mock<IBroadcast>();
            mockBroadcast.Setup(x => x.ToXml()).Returns("<Orders><Order Type='EB'></Order></Orders>");

            ApiClient apiClient = new ApiClient("url", "user", "password");
            OrderResponse response = apiClient.LaunchBroadcast(mockBroadcast.Object);
            Assert.AreEqual(RequestResultType.Success, response.RequestResult);
            Assert.AreEqual(OrderType.EmailBroadcast, response.OrderType);
            Assert.AreEqual(12345, response.OrderID);
        }

        [TestMethod]
        public void GetOrderReportTest()
        {
            var mockWebClientProxy = new Mock<IWebClientProxy>();
            mockWebClientProxy.Setup(x => x.UploadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
                    () => { return "<PostAPIResponse><SaveTransactionalOrderResult><status>status</status></SaveTransactionalOrderResult></PostAPIResponse>"; });
            ApiClientResolver.Instance.Container.Register(mockWebClientProxy.Object);

            ApiClient apiClient = new ApiClient("url", "user", "password");
            OrderReport response = apiClient.GetOrderReport(12345, OrderType.EmailBroadcast, ReportReturnType.XML);
            Assert.AreEqual(RequestResultType.Success, response.RequestResult);
            Assert.AreEqual(OrderType.EmailBroadcast, response.OrderType);
            Assert.AreEqual(12345, response.OrderID);
            Assert.AreEqual("<PostAPIResponse><SaveTransactionalOrderResult><status>status</status></SaveTransactionalOrderResult></PostAPIResponse>", response.ReportData);         
        }

        [TestCleanup]
        public void Cleanup()
        {
            ApiClientResolver.Instance.Reset();
        }
    }
}

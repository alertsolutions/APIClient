using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSolutions.API;
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
        public void Test()
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
            string retval = apiClient.CancelOrder(1, OrderType.VoiceBroadcast);
            Assert.IsNotNull("some result");


        }

        [TestCleanup]
        public void Cleanup()
        {
            ApiClientResolver.Instance.Reset();
        }
    }
}

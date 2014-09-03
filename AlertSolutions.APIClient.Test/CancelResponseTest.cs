using AlertSolutions.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test
{
    [TestClass]
    public class CancelResponseTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var response =
@"<PostAPIResponse>
  <CancelOrderResult Type=""VL"" Id=""5"">
    <Status>An unexpected error occurred: System.Web.Services.Protocols.SoapException: Invalid Login at WebLaunch.DirectConnect.GetOrderDetails(Int32 orderid, String sUserid, String sPrivilage, String type) in c:\code\AlertSolutions\src\SOAP\WebLaunchWeb\DirectConnect.asmx.cs:line 581</Status>
    <StatusCode>0</StatusCode>
  </CancelOrderResult>
</PostAPIResponse>";
            var cr = new CancelResponse(response);
            Assert.AreEqual(CancelStatusCode.UnknownError, cr.StatusCode);
        }
    }
}

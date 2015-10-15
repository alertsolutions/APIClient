using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Messages
{
    [TestClass]
    public class FaxMessageTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var faxMessage = new FaxMessage();
            Assert.IsNotNull(faxMessage);
        }

        [TestMethod]
        public void XmlOutputTest()
        {
            var faxMessage = new FaxMessage();
            faxMessage.FaxFrom = "james";
            faxMessage.FaxNumber = "4013002095";
            faxMessage.DocumentStyle = DocumentStyle.Letter;
            faxMessage.Documents = new List<FaxDocument>()
            { 
                new FaxDocumentBuilder().FromText("message.txt", "This is a test fax sent using the API Client."),
            };

            string xml = faxMessage.ToXml();
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<DocumentStyle>Letter</DocumentStyle>"));
        }

        [TestMethod]
        public void SerializeTest()
        {
        }

    }
}

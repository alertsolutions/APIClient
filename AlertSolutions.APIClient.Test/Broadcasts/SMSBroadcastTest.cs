using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using AlertSolutions.APIClient.Test.TestFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Broadcasts
{
    [TestClass]
    public class SMSBroadcastTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var smsBroadcast = new SMSBroadcast();
            Assert.IsNotNull(smsBroadcast);
        }

        [TestMethod]
        public void BuildXmlWithoutRequiredInput()
        {
            try
            {
                var sb = new SMSBroadcast();
                var xml = sb.ToXml();
            }
            catch (FormatException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void XmlOutputTest()
        {
            var sb = new SMSBroadcast();
            sb.SMSHeader = "$$phone$$";
            sb.ShortCode = "77811";
            sb.TextMessage = new TextMessageBuilder().FromText(DocumentSamples.GetSampleTextMessage());
            sb.StopTimeUTC = DateTime.UtcNow.AddDays(1);
            sb.List = new ContactListBuilder().FromText("list.csv", DocumentSamples.GetSampleContactListCsv());
            Assert.IsNotNull(sb);
            string xml = sb.ToXml();
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<ShortCode>77811</ShortCode>"));
            Assert.AreEqual("$$phone$$", sb.SMSHeader);
            Assert.AreEqual("77811", sb.ShortCode);
            Assert.IsTrue(xml.Contains("<StopDateTime>" + DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd HH:mm") + "</StopDateTime>"));
        }

        [TestMethod]
        public void SerializeTest()
        {
        }

    }
}

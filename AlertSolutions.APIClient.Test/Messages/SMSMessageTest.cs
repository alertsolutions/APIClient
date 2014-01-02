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

namespace AlertSolutions.APIClient.Test.Messages
{
    [TestClass]
    public class SMSMessageTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var smsMessage = new SMSMessage();
            Assert.IsNotNull(smsMessage);
        }

        [TestMethod]
        public void XmlOutputTest()
        {
            string smsText = DocumentSamples.GetSampleTextMessage();

            var sm = new SMSMessage();
            sm.Number = "5086128160";
            sm.ShortCode = "77811";
            sm.TextMessage = TextMessage.FromText(smsText);
            Assert.IsNotNull(sm);
            string xml = sm.ToXml();
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<ShortCode>77811</ShortCode>"));
            Assert.AreEqual("5086128160", sm.Number);
            Assert.AreEqual("77811", sm.ShortCode);
        }

        [TestMethod]
        public void SerializeTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SMSMessage));
            Order order = new SMSMessage()
            {
                Number = "324-2342",
                SendTimeUTC = DateTime.UtcNow,
                OverType = "sdfsdf",
                TextMessage = TextMessage.FromText("Some random text")
            };

            StringBuilder sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, order);
            writer.Close();

            Assert.IsNotNull(sb);
            Assert.IsTrue(!string.IsNullOrEmpty(sb.ToString()));

            TextReader tr = new StringReader(sb.ToString());
            object obj = serializer.Deserialize(tr);
            var deserializedMessage = obj as SMSMessage;

            Assert.IsTrue(deserializedMessage.Number == "324-2342");
            var xml = deserializedMessage.ToXml();
        }

    }
}

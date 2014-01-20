using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Broadcasts
{
    [TestClass]
    public class VoiceBroadcastTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var voiceBroadcast = new VoiceBroadcast();
            Assert.IsNotNull(voiceBroadcast);
        }

        [TestMethod]
        public void BuildXmlWithoutRequiredInput()
        {
            try
            {
                var vb = new VoiceBroadcast();
                var xml = vb.ToXml();
            }
            catch (FormatException)
            {
                Assert.IsTrue(true);
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void BuildXmlWithRequiredInput()
        {
            var vb = new VoiceBroadcast();
            vb.SendTimeUTC = DateTime.UtcNow;
            vb.StopTimeUTC = DateTime.UtcNow.Date.AddHours(20);
            vb.RestartTimeUTC = DateTime.UtcNow.Date.AddDays(1).AddHours(8);
            vb.List = ContactList.FromText("requiredList.csv", "phone\r\n5555555555");
            vb.Documents = new List<Document>()
            {
                VoiceDocument.FromText("live01.txt", "this is a live voice call.", VoiceDocumentType.Live),
            };

            var xml = vb.ToXml();
            Assert.IsNotNull(xml);
            XDocument xDoc = XDocument.Parse(xml);
            Assert.IsNotNull(xDoc);
        }

        [TestMethod]
        public void XmlOutputAllFieldsTest()
        {
            var vb = new VoiceBroadcast();
            vb.SendTimeUTC = DateTime.UtcNow;
            vb.StopTimeUTC = DateTime.UtcNow.Date.AddHours(20);
            vb.StopTimeUTC = DateTime.UtcNow.Date.AddDays(1).AddHours(8);
            vb.BillCode = "BC";
            vb.ProjectCode = "PC";
            vb.CallerID = "4015555555";
            vb.ThrottleType = VoiceBroadcast.VoiceThrottleType.MaximumCalls;
            vb.ThrottleNumber = 2;
            vb.List = ContactList.FromText("requiredList.csv", "phone\r\n5555555555");
            vb.VoiceHeader = "phone";
            vb.Documents = new List<Document>()
            {
                VoiceDocument.FromText("live01.txt", "this is a live voice call.", VoiceDocumentType.Live),
            };

            var xml = vb.ToXml();
            Assert.IsNotNull(xml);
            XDocument xDoc = XDocument.Parse(xml);
            Assert.IsNotNull(xDoc);

            // make sure the necessary voice fields are there (as per postAPI documentation)
            VerifyFieldsExist(xDoc);
            
            // make sure the necessary VALUES exist in those fields (as per postAPI documentation)
            var orderTag = xDoc.Root.Elements("Order").FirstOrDefault();
            Assert.IsTrue(orderTag.FirstAttribute.Value == "VL");
            Assert.IsTrue(orderTag.Element("Project").Value == "PC");
            Assert.IsTrue(orderTag.Element("BillCode").Value == "BC");
            Assert.IsTrue(orderTag.Element("AutoLaunch").Value == "Yes");
            Assert.IsTrue(DateTime.Parse(orderTag.Element("Date").Value) == DateTime.UtcNow.Date);
            Assert.IsTrue(DateTime.Parse(orderTag.Element("Time").Value).Minute == DateTime.UtcNow.Minute);
            // 3 1 0-4 Documentation says StopTime, but I think the name changed since then.
            Assert.IsNotNull(DateTime.Parse(orderTag.Element("StopDateTime").Value));
            Assert.IsNotNull(DateTime.Parse(orderTag.Element("RestartTime").Value));
            Assert.IsTrue(orderTag.Element("Restart").Value == "No");
            Assert.IsTrue(Enumerable.Range(0, 5).Contains(int.Parse(orderTag.Element("NumberOfRedials").Value)));
            Assert.IsTrue(Enumerable.Range(0, 3).Contains(int.Parse(orderTag.Element("NumberOfResends").Value)));
            Assert.IsNotNull(orderTag.Element("CallerID").Value);
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotOne").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotTwo").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotThree").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotFour").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotFive").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotSix").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotSeven").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotEight").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotNine").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotPound").Value));
            Assert.IsTrue(string.IsNullOrEmpty(orderTag.Element("HotStar").Value));
            Assert.IsNotNull(string.IsNullOrEmpty(orderTag.Element("ThrType").Value));
            Assert.IsNotNull(string.IsNullOrEmpty(orderTag.Element("ThrNum").Value));


            // TODO : make tests for these conditional values and their output
            // conditionals
            Assert.IsNotNull(orderTag.Element("ListID"));
            Assert.IsNotNull(orderTag.Element("ListName"));
            Assert.IsNotNull(orderTag.Element("ListBinary"));
            Assert.IsNotNull(orderTag.Element("VoiceHeader"));

            // documents
            Assert.IsNotNull(orderTag.Element("Documents"));
            Assert.IsTrue(orderTag.Element("Documents").Elements().Count() == 1);
        }

        public void VerifyFieldsExist(XDocument orderXml)
        {
            Assert.IsNotNull(orderXml);
            var orderTag = orderXml.Root.Elements("Order").FirstOrDefault();
            Assert.IsTrue(orderTag.FirstAttribute.Value == "VL");
            Assert.IsNotNull(orderTag.Element("Project"));
            Assert.IsNotNull(orderTag.Element("BillCode"));
            Assert.IsNotNull(orderTag.Element("AutoLaunch"));
            Assert.IsNotNull(orderTag.Element("Date"));
            Assert.IsNotNull(orderTag.Element("Time"));
            // 3 1 0-4 Documentation says StopTime, but I think the name changed since then.
            Assert.IsNotNull(orderTag.Element("StopDateTime"));
            Assert.IsNotNull(orderTag.Element("Restart"));
            Assert.IsNotNull(orderTag.Element("RestartTime"));
            Assert.IsNotNull(orderTag.Element("NumberOfRedials"));
            Assert.IsNotNull(orderTag.Element("NumberOfResends"));
            Assert.IsNotNull(orderTag.Element("CallerID"));
            Assert.IsNotNull(orderTag.Element("HotOne"));
            Assert.IsNotNull(orderTag.Element("HotTwo"));
            Assert.IsNotNull(orderTag.Element("HotThree"));
            Assert.IsNotNull(orderTag.Element("HotFour"));
            Assert.IsNotNull(orderTag.Element("HotFive"));
            Assert.IsNotNull(orderTag.Element("HotSix"));
            Assert.IsNotNull(orderTag.Element("HotSeven"));
            Assert.IsNotNull(orderTag.Element("HotEight"));
            Assert.IsNotNull(orderTag.Element("HotNine"));
            Assert.IsNotNull(orderTag.Element("HotPound"));
            Assert.IsNotNull(orderTag.Element("HotStar"));
            Assert.IsNotNull(orderTag.Element("ThrType"));
            Assert.IsNotNull(orderTag.Element("ThrNum"));
            Assert.IsNotNull(orderTag.Element("ListID"));
            Assert.IsNotNull(orderTag.Element("ListName"));
            Assert.IsNotNull(orderTag.Element("ListBinary"));
            Assert.IsNotNull(orderTag.Element("VoiceHeader"));
            Assert.IsNotNull(orderTag.Element("Documents"));
            Assert.IsTrue(orderTag.Element("Documents").Elements().Any());
        }

        //[TestMethod]
        //public void XmlOutputVoiceDocumentsTest()
        //{
        //    //TODO : copy test similar to EB's attachment test
        //}

        [TestMethod, Ignore]
        public void SerializeTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EmailBroadcast));
            Order order = new EmailBroadcast()
            {
                DisplayName = "jt",
                EmailSubject = "test email message",
                SendTimeUTC = DateTime.UtcNow,
                TextBody = TextBody.FromText("This is a test email")
            };

            StringBuilder sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, order);
            writer.Close();

            Assert.IsNotNull(sb);
            Assert.IsTrue(!string.IsNullOrEmpty(sb.ToString()));

            TextReader tr = new StringReader(sb.ToString());
            object obj = serializer.Deserialize(tr);
            var deserializedMessage = obj as EmailBroadcast;
            Assert.IsTrue(deserializedMessage.DisplayName == "jt");
            Assert.IsTrue(deserializedMessage.EmailSubject == "test email message");
            var xml = deserializedMessage.ToXml();
        }
    }
}

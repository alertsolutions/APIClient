using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using AlertSolutions.APIClient.Test.TestFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Messages
{
    [TestClass]
    public class EmailMessageTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var emailMessage = new EmailMessage();
            Assert.IsNotNull(emailMessage);
        }

        [TestMethod]
        public void XmlOutputTest()
        {
            var et = new EmailMessage();

            string attachmentFile = DocumentSamples.GetSampleCsv();
            string textbody = DocumentSamples.GetSampleEmailText();
            string htmlbody = DocumentSamples.GetSampleEmailHtml();

            et.EmailTo = "jthomas@blimessaging.com";
            et.EmailSubject = "PostAPIClient Refactor Test (ET)";
            et.EmailReplyTo = "jthomas@blimessaging.com";
            et.EmailFrom = "jthomas@blimessaging.com";
            et.DisplayName = "JThomas from AlertSolutions";
            et.Attachments = new List<Attachment>() { Attachment.FromText("Attachment.txt",attachmentFile) };
            et.TextBody = TextBody.FromText(textbody);
            et.HtmlBody = HtmlBody.FromText(htmlbody);
            Assert.IsNotNull(et);
            Assert.IsNotNull(et.EmailTo);
            Assert.IsNotNull(et.ToXml());
        }

        [TestMethod, Ignore]
        public void SerializeTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EmailMessage));
            Order order = new EmailMessage()
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
            var deserializedMessage = obj as EmailMessage;
            Assert.IsTrue(deserializedMessage.DisplayName == "jt");
            Assert.IsTrue(deserializedMessage.EmailSubject == "test email message");
            var xml = deserializedMessage.ToXml();
        }
    }
}

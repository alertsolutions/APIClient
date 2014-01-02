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
    public class EmailBroadcastTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var emailBroadcast = new EmailBroadcast();
            Assert.IsNotNull(emailBroadcast);
        }

        [TestMethod]
        public void XmlOutputTest()
        {
            string attachmentFile = DocumentSamples.GetSampleCsv();
            string textbody = DocumentSamples.GetSampleEmailText();
            string htmlbody = DocumentSamples.GetSampleEmailHtml();
            string contactlist = DocumentSamples.GetSampleContactListCsv();

            var eb = new EmailBroadcast();
            eb.BillCode = "PostAPIClient Refactor Test";
            eb.ProjectCode = "PostAPIClient Refactor Test";
            eb.EmailSubject = "PostAPIClient Refactor Test (EB)";
            eb.EmailReplyTo = "jthomas@blimessaging.com";
            eb.EmailFrom = "jthomas@blimessaging.com";
            eb.DisplayName = "JThomas from AlertSolutions";
            eb.Attachments = new List<Attachment>() { Attachment.FromText("file.txt", attachmentFile) };
            eb.TextBody = TextBody.FromText(textbody);
            eb.HtmlBody = HtmlBody.FromText(htmlbody);
            eb.List = ContactList.FromText("ContactList.csv", contactlist);
            eb.EmailHeader = "email";
            eb.Proofs = new List<string>() { "jthomas@blimessaging.com" };
            Assert.IsNotNull(eb.ToXml());
            Assert.IsNotNull(eb.ResendInterval);
        }

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

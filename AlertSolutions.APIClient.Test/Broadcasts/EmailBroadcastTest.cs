using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        public void BuildXmlWithoutRequiredInput()
        {
            try
            {
                var eb = new EmailBroadcast();
                var xml = eb.ToXml();
            }
            catch (FormatException)
            {
                Assert.IsTrue(true);
            }
        }

        public XDocument Parse(EmailBroadcast eb)
        {
            var xml = eb.ToXml();
            Assert.IsNotNull(xml);
            XDocument xDoc = XDocument.Parse(xml);
            Assert.IsNotNull(xDoc);
            return xDoc;
        }

        [TestMethod]
        public void BuildXmlWithRequiredInput()
        {
            var eb = new EmailBroadcast();
            eb.TextBody = TextBody.FromText("Required content");
            eb.List = ContactList.FromText("requiredList.csv", "email\r\nsanta@northpole.com");

            Parse(eb);
        }

        [TestMethod]
        public void XmlOutputDefaultValuesTest()
        {
            var eb = new EmailBroadcast();
            eb.TextBody = TextBody.FromText("Required content");
            eb.List = ContactList.FromText("requiredList.csv", "email\r\nsanta@northpole.com");
            
            var xDoc = Parse(eb);
            VerifyFieldsExist(xDoc);
            var orderTag = xDoc.Root.Elements("Order").FirstOrDefault();
            Assert.IsTrue(orderTag.Element("AutoLaunch").Value == "Yes");
            //TODO : finish the rest of the default fields
        }
   
        [TestMethod]
        public void XmlOutputSetValuesTest()
        {
            var eb = new EmailBroadcast();
            eb.AutoLaunch = false;
            eb.BillCode = "BC";
            eb.DisplayName = "DN";
            eb.EmailFrom = "EF";
            eb.EmailHeader = "email";
            eb.EmailReplyTo = "ERT";
            eb.EmailSubject = "ES";
            eb.HtmlBody = HtmlBody.FromText("Required content <b>html</b>");
            eb.IsForward = true;
            eb.IsReplaceLink = true;
            eb.IsUnsubscribe = true;
            eb.LanguageHeader = "Dutch";
            eb.List = ContactList.FromText("requiredList.csv", "email\r\nsanta@northpole.com");
            eb.NumberOfRedials = 3;
            eb.NumberOfResends = 2;
            eb.ProjectCode = "PC";
            eb.Proofs = new List<string>();
            eb.ResendInterval = "4";
            eb.SendTimeUTC = new DateTime(2010, 10, 12, 16, 15, 35);
            eb.TextBody = TextBody.FromText("Required content plaintext");

            var xDoc = Parse(eb);
            VerifyFieldsExist(xDoc);

            // make sure the VALUES match what was set above
            var orderTag = xDoc.Root.Elements("Order").FirstOrDefault();
            Assert.IsTrue(orderTag.FirstAttribute.Value == "EB");
            Assert.IsTrue(orderTag.Element("AutoLaunch").Value == "No");
            Assert.IsTrue(orderTag.Element("BillCode").Value == "BC");
            Assert.IsTrue(orderTag.Element("Project").Value == "PC");
            Assert.IsTrue(orderTag.Element("DisplayName").Value == "DN");
            Assert.IsTrue(orderTag.Element("Date").Value == "2010-10-12");
            Assert.IsTrue(orderTag.Element("Time").Value == "16:15");
            Assert.IsTrue(orderTag.Element("From").Value == "EF");
            Assert.IsTrue(orderTag.Element("ReplyTo").Value == "ERT");
            Assert.IsTrue(orderTag.Element("Subject").Value == "ES");
            Assert.IsTrue(orderTag.Element("Forward").Value == "Yes");
            Assert.IsTrue(orderTag.Element("ReplaceLink").Value == "Yes");
            Assert.IsTrue(orderTag.Element("Unsubscribe").Value == "Yes");
            Assert.IsTrue(orderTag.Element("LanguageHeader").Value == "Dutch");
            Assert.IsTrue(orderTag.Element("NumberOfRedials").Value == "3");
            Assert.IsTrue(orderTag.Element("NumberOfResends").Value == "2");

            // TODO : make tests for these conditional fields and their output
            // conditionals
            Assert.IsTrue(orderTag.Element("ResendInterval").Value == "4");
            Assert.IsTrue(orderTag.Element("ListID").Value == "");
            Assert.IsTrue(orderTag.Element("ListName").Value == "requiredList.csv");
            Assert.IsTrue(Encoding.UTF8.GetString(Convert.FromBase64String(orderTag.Element("ListBinary").Value)) == "email\r\nsanta@northpole.com");
            Assert.IsTrue(orderTag.Element("EmailField").Value == "email"); // header <- what happens when not set?

            // documents
            Assert.IsTrue(orderTag.Element("HtmlID").Value == "");
            Assert.IsNotNull(orderTag.Element("HtmlFile").Value);
            Assert.IsTrue(Encoding.UTF8.GetString(Convert.FromBase64String(orderTag.Element("HtmlBinary").Value)) == "Required content <b>html</b>");
            Assert.IsTrue(orderTag.Element("TextID").Value == "");
            Assert.IsNotNull(orderTag.Element("TextFile").Value);
            Assert.IsTrue(Encoding.UTF8.GetString(Convert.FromBase64String(orderTag.Element("TextBinary").Value)) == "Required content plaintext");

        }

        public void VerifyFieldsExist(XDocument orderXml)
        {
            Assert.IsNotNull(orderXml);
            var orderTag = orderXml.Root.Elements("Order").FirstOrDefault();
            Assert.IsTrue(orderTag.FirstAttribute.Value == "EB");
            Assert.IsNotNull(orderTag.Element("Project"));
            Assert.IsNotNull(orderTag.Element("BillCode"));
            Assert.IsNotNull(orderTag.Element("AutoLaunch"));
            Assert.IsNotNull(orderTag.Element("Date"));
            Assert.IsNotNull(orderTag.Element("Time"));
            Assert.IsNotNull(orderTag.Element("DisplayName"));
            Assert.IsNotNull(orderTag.Element("From"));
            Assert.IsNotNull(orderTag.Element("ReplyTo"));
            Assert.IsNotNull(orderTag.Element("Subject"));
            Assert.IsNotNull(orderTag.Element("Forward"));
            Assert.IsNotNull(orderTag.Element("ReplaceLink"));
            Assert.IsNotNull(orderTag.Element("Unsubscribe"));
            Assert.IsNotNull(orderTag.Element("NumberOfRedials"));
            Assert.IsNotNull(orderTag.Element("NumberOfResends"));
            Assert.IsNotNull(orderTag.Element("ResendInterval"));
            Assert.IsNotNull(orderTag.Element("ListID"));
            Assert.IsNotNull(orderTag.Element("ListName"));
            Assert.IsNotNull(orderTag.Element("ListBinary"));
            Assert.IsNotNull(orderTag.Element("EmailField"));
            Assert.IsNotNull(orderTag.Element("HtmlID"));
            Assert.IsNotNull(orderTag.Element("HtmlFile"));
            Assert.IsNotNull(orderTag.Element("HtmlBinary"));
            Assert.IsNotNull(orderTag.Element("TextID"));
            Assert.IsNotNull(orderTag.Element("TextFile"));
            Assert.IsNotNull(orderTag.Element("TextBinary"));
            Assert.IsNotNull(orderTag.Element("Proofs").Elements("Proof").Any());
            Assert.IsNotNull(orderTag.Element("Attachments"));
            Assert.IsTrue(orderTag.Element("Attachments").Elements().Any());
        }

        [TestMethod]
        public void XmlOutputProofTest()
        {
            var eb = new EmailBroadcast();
            eb.TextBody = TextBody.FromText("required content");
            eb.List = ContactList.FromText("requiredList.csv", "email\r\nsanta@northpole.com");
            eb.Proofs = new List<string>() { "elf1@northpole.com", "elf2@northpole.com", "elf3@northpole.com", "elf4@northpole.com", "elf5@northpole.com" };

            var xml = eb.ToXml();
            XDocument xDoc = XDocument.Parse(xml);

            var orderTag = xDoc.Root.Elements("Order").FirstOrDefault();
            var proofs = orderTag.Element("Proofs").Elements().ToList();
            Assert.IsTrue(orderTag.Element("Proofs").Elements().Count() ==  5);
            Assert.IsTrue(proofs[0].Value == "elf1@northpole.com");
            Assert.IsTrue(proofs[1].Value == "elf2@northpole.com");
            Assert.IsTrue(proofs[2].Value == "elf3@northpole.com");
            Assert.IsTrue(proofs[3].Value == "elf4@northpole.com");
            Assert.IsTrue(proofs[4].Value == "elf5@northpole.com");

            // overloading the amount of allowable proofs
            eb.Proofs.Add("elf6@northpole.com");

            xml = eb.ToXml();
            xDoc = XDocument.Parse(xml);
            orderTag = xDoc.Root.Elements("Order").FirstOrDefault();
            Assert.IsTrue(orderTag.Element("Proofs").Elements().Count() == 6);
        }

        [TestMethod]
        public void XmlOutputAttachmentTest()
        {
            var eb = new EmailBroadcast();
            eb.TextBody = TextBody.FromText("required content");
            eb.List = ContactList.FromText("requiredList.csv", "email\r\nsanta@northpole.com");
            eb.Attachments = new List<Attachment>()
            {
                Attachment.FromText("file1.txt", "content #1"), 
                Attachment.FromText("file2.txt", "content #2"),
                Attachment.FromText("file3.txt", "content #3")
            };

            var xml = eb.ToXml();
            XDocument xDoc = XDocument.Parse(xml);
            var orderTag = xDoc.Root.Element("Order");
            var attachmentsTag = orderTag.Element("Attachments");

            Assert.IsNotNull(attachmentsTag);
            Assert.IsTrue(attachmentsTag.Elements().Count() == 3);

            var attachmentTag1 = attachmentsTag.Elements().ToList()[0];
            Assert.IsNotNull(attachmentTag1.Element("AttachmentID"));
            Assert.IsTrue(attachmentTag1.Element("AttachmentName").Value == "file1.txt");
            var value = attachmentTag1.Element("AttachmentBinary").Value;
            Assert.IsTrue(attachmentTag1.Element("AttachmentBinary").Value == Convert.ToBase64String(Encoding.UTF8.GetBytes("content #1")));
            var attachmentTag2 = attachmentsTag.Elements().ToList()[1];
            Assert.IsNotNull(attachmentTag2.Element("AttachmentID"));
            Assert.IsTrue(attachmentTag2.Element("AttachmentName").Value == "file2.txt");
            Assert.IsTrue(attachmentTag2.Element("AttachmentBinary").Value == Convert.ToBase64String(Encoding.UTF8.GetBytes("content #2")));
            var attachmentTag3 = attachmentsTag.Elements().ToList()[2];
            Assert.IsNotNull(attachmentTag3.Element("AttachmentID"));
            Assert.IsTrue(attachmentTag3.Element("AttachmentName").Value == "file3.txt");
            Assert.IsTrue(attachmentTag3.Element("AttachmentBinary").Value == Convert.ToBase64String(Encoding.UTF8.GetBytes("content #3")));
        }


        [TestMethod,Ignore]
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

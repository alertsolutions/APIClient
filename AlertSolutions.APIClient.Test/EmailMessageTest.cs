using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test
{
    [TestClass]
    public class EmailMessageTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var et = new EmailMessage();

            string attachmentFile =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.Attachment.txt")).ReadToEnd();
            string textbody =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.Email.txt")).ReadToEnd();
            string htmlbody =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.Email.html")).ReadToEnd();

            et.EmailTo = "jthomas@blimessaging.com";
            et.EmailSubject = "PostAPIClient Refactor Test (ET)";
            et.EmailReplyTo = "jthomas@blimessaging.com";
            et.EmailFrom = "jthomas@blimessaging.com";
            et.DisplayName = "JThomas from AlertSolutions";
            et.Attachments = new List<Attachment>() { Attachment.FromText("Attachment.txt",attachmentFile) };
            et.TextBody = TextBody.FromText(textbody);
            et.HtmlBody = HtmlBody.FromText(htmlbody);
            Assert.IsNotNull(et);
        }

    }
}

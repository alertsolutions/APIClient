using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test
{
    [TestClass]
    public class EmailBroadcastTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            string attachmentFile =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.Attachment.txt")).ReadToEnd();
            string textbody =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.Email.txt")).ReadToEnd();
            string htmlbody =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.Email.html")).ReadToEnd();
            string reportcardpdf =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.report-card-sample.pdf")).ReadToEnd();
            string contactlist =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.ContactList.csv")).ReadToEnd();


            var f =Encoding.UTF8.GetBytes(reportcardpdf);
            //var f = System.IO.File.ReadAllBytes("testfiles\\report-card-sample.pdf");
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
            Assert.IsNotNull(eb);
            Assert.IsNotNull(eb.ToXml());
            Assert.IsNotNull(eb.ResendInterval);
        }
    }
}

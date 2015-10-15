using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Broadcasts
{
    [TestClass]
    public class FaxBroadcastTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var faxBroadcast = new FaxBroadcast();
            Assert.IsNotNull(faxBroadcast);
        }

        [TestMethod]
        public void BuildXmlWithoutRequiredInput()
        {
            try
            {
                var fb = new FaxBroadcast();
                fb.ToXml();
                Assert.Fail();
            }
            catch (FormatException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void XmlOutputTest()
        {
            var fb = new FaxBroadcast();
            fb.BillCode = "testing-123";
            fb.ProjectCode = "test project";
            var builder = new ContactListBuilder();
            fb.List = builder.FromText("my_list.csv",
@"fax,name,h1,h2,h3,h4
14015551234,adam,header1,header2,header3,header4");
            fb.AutoLaunch = true;
            fb.Restart = false;
            fb.NumberOfResends = 0;
            fb.NumberOfRedials = 1;
            fb.ResendInterval = 0;
            fb.FaxHeader = "fax";
            fb.ToHeader1 = "h1";
            fb.ToHeader2 = "h2";
            fb.ToHeader3 = "h3";
            fb.ToHeader4 = "h4";
            fb.Dedup = true;
            fb.DocumentStyle = DocumentStyle.Letter;
            var db = new FaxDocumentBuilder();
            fb.Documents = new List<FaxDocument>();
            fb.Documents.Add(db.FromText("simplefax.txt", "this is a text fax"));
            var xml = XDocument.Parse(fb.ToXml());
            Assert.AreEqual("this is a text fax", 
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(
                        xml.XPathSelectElement("//DocumentBinary").Value)));
            Assert.AreEqual(DocumentStyle.Letter, Enum.Parse(typeof(DocumentStyle), xml.XPathSelectElement("//DocumentStyle").Value));
        }

        [TestMethod]
        public void SerializeTest()
        {
        }
    }
}

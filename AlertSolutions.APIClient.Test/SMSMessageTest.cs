using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test
{
    [TestClass]
    public class SMSMessageTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            string textMessage =
                new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.TextMessage.txt")).ReadToEnd();

            var mt = new SMSMessage();
            mt.Number = "5086128160";
            //mt.Number = 4013002095;
            mt.ShortCode = "77811";
            //mt.TextMessage = TextMessage.FromString("This is not in base 64!");
            mt.TextMessage = TextMessage.FromText(textMessage);
            Assert.IsNotNull(mt);
            string xml = mt.ToXml();
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<ShortCode>77811</ShortCode>"));
            Assert.AreEqual("5086128160", mt.Number);
            Assert.AreEqual("77811", mt.ShortCode);
        }
    }
}

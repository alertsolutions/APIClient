using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Messages
{
    [TestClass]
    public class VoiceMessageTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var voiceMessage = new VoiceMessage();
            Assert.IsNotNull(voiceMessage);
        }

        [TestMethod]
        public void XmlOutputTest()
        {
        }

        [TestMethod]
        public void SerializeTest()
        {
        }

    }
}

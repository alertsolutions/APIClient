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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Broadcasts
{
    [TestClass]
    public class SMSBroadcastTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var smsBroadcast = new SMSBroadcast();
            Assert.IsNotNull(smsBroadcast);
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

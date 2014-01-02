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
    public class FaxBroadcastTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var faxBroadcast = new FaxBroadcast();
            Assert.IsNotNull(faxBroadcast);
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

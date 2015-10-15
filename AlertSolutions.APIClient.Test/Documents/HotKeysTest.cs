using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertSolutions.API.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlertSolutions.APIClient.Test.Documents
{
    [TestClass]
    public class HotKeysTest
    {
        [TestMethod]
        public void StopTest()
        {
            var thrown = false;
            try
            {
                HotKey.CreateStop('a');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                thrown = true;
            }
            Assert.IsTrue(thrown);

            var stop = HotKey.CreateStop('1');
            Assert.AreEqual('1', stop.Key);
            Assert.AreEqual("STOP", stop.Command);
            Assert.AreEqual("<HotOne>STOP</HotOne>", stop.ToXml().ToString());
        }

        [TestMethod]
        public void RepeatTest()
        {
            var repeat = HotKey.CreateRepeat('*');
            Assert.AreEqual('*', repeat.Key);
            Assert.AreEqual("REPEAT", repeat.Command);
            Assert.AreEqual("<HotStar>REPEAT</HotStar>", repeat.ToXml().ToString());
        }

        [TestMethod]
        public void XferTest()
        {
            var xfer = HotKey.CreateTransfer('7', "14015550123");
            Assert.AreEqual('7', xfer.Key);
            Assert.AreEqual("DIALTO", xfer.Command);
            Assert.AreEqual("<HotSeven>DIALTO 14015550123</HotSeven>", xfer.ToXml().ToString());
        }
    }
}

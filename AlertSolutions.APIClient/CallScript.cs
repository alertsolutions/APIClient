using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace AlertSolutions.API
{
    [Serializable]
    public class CallScript
    {
        private XDocument _callScript { get; set; }

        private CallScript()
        {
            // private default constructor that can be called internally
        }

        public static CallScript FromFile(string filePath)
        {
            var text = System.IO.File.ReadAllText(filePath);
            var cs = new CallScript();
            cs._callScript = XDocument.Parse(text);
            return cs;
        }

        public static CallScript FromText(string callScriptText)
        {
            var cs = new CallScript();
            cs._callScript = XDocument.Parse(callScriptText);
            return cs;
        }

        public static CallScript FromByteArray(byte[] callScriptData)
        {
            var callScriptText = Encoding.UTF8.GetString(callScriptData);
            var cs = new CallScript();
            cs._callScript = XDocument.Parse(callScriptText);
            return cs;
        }

        public static CallScript FromBase64String(string encodedCallScriptData)
        {
            var callScriptText = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCallScriptData));
            var cs = new CallScript();
            cs._callScript = XDocument.Parse(callScriptText);
            return cs;
        }

        public XElement ToXml()
        {
            return (XElement) _callScript.FirstNode;
        }
    }
}

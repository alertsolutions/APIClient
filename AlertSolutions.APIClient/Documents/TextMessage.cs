using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class TextMessage
    {
        public int MessageID { get; internal set; }
        public string Message { get; internal set; }

        public List<XElement> ToXml()
        {
            var messageXml = new DocumentElementBuilder().ToXml("Message", MessageID, Message, new byte[0], "ID", "", "File");

            // only actually setting the <message> tag whether the user gets it from string or file
            Utilities.StripNodeFromElement(messageXml, "MessageFile");
            messageXml.Add(new XElement("MessageFile"));

            return messageXml.Elements().ToList();
        }
    }
}

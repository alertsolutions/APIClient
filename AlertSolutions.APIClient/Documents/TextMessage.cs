using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class TextMessage : DocumentBase
    {
        public TextMessage()
        {
        }

        public TextMessage(string filePath) : base(filePath){}
        public TextMessage(int fileID) : base(fileID){}
        public TextMessage(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public static TextMessage FromFile(string filePath)
        {
            var message = System.IO.File.ReadAllText(filePath);
            // bit of a hack passing the message into the fileName of
            // the DocumentBase class like this, since the message tag needs text instead of a binary
            var textMsg = new TextMessage(message, new byte[0]);
            return textMsg;
        }

        public static TextMessage FromText(string messageText)
        {
            var textMsg = new TextMessage(messageText, new byte[0]);
            return textMsg;
        }

        public static TextMessage FromByteArray(byte[] messageTextData)
        {
            return new TextMessage(Encoding.UTF8.GetString(messageTextData), new byte[0]);
        }

        public static TextMessage FromBase64String(string encodedMessageTextData)
        {
            var messageText = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMessageTextData));
            var textMsg = new TextMessage(messageText, new byte[0]);
            return textMsg;
        }

        // TODO should this be turned on?
        //public static TextMessage FromID(int listID)
        //{
            //var textMsg = new TextMessage(listID);
            //textMsg._source = "id";
            //return textMsg;
        //}

        public List<XElement> ToXml()
        {
            var messageXml = ToXml("Message", "ID", "", "File");

            // only actually setting the <message> tag whether the user gets it from string or file
            Utilities.StripNodeFromElement(messageXml, "MessageFile");
            messageXml.Add(new XElement("MessageFile"));

            return messageXml.Elements().ToList();
        }
    }
}

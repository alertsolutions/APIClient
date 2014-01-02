using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public abstract class EmailBody : DocumentBase
    {
        public EmailBody()
        {
        }

        internal EmailBody(string filePath) : base(filePath){}
        internal EmailBody(int fileID) : base(fileID){}
        internal EmailBody(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public abstract List<XElement> ToXml();
    }

    [Serializable]
    public class HtmlBody : EmailBody
    {
        public HtmlBody()
        {
        }

        internal HtmlBody(string filePath) : base(filePath){}
        internal HtmlBody(int fileID) : base(fileID){}
        internal HtmlBody(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public static HtmlBody FromFile(string filePath)
        {
            return new HtmlBody(filePath);
        }

        public static HtmlBody FromText(string messageHtmlText)
        {
            var bytes = Encoding.UTF8.GetBytes(messageHtmlText);
            return new HtmlBody("message.html", bytes);
        }

        public static HtmlBody FromByteArray(byte[] messageHtmlData)
        {
            return new HtmlBody("message.html", messageHtmlData);
        }

        public static HtmlBody FromBase64String(string encodedMessageHtmlData)
        {
            var bytes = Convert.FromBase64String(encodedMessageHtmlData);
            return new HtmlBody("message.html", bytes);
        }

        // TODO : should this part of PostAPI be made accessible through the client?
        //public static HtmlBody FromID(int listID)
        //{
        //    return new HtmlBody(listID);
        //}

        public override List<XElement> ToXml()
        {
            var htmlXml = ToXml("Html", "ID", "File", "Binary");
            return htmlXml.Elements().ToList();
        }
        public static List<XElement> EmptyTagsToXml()
        {
            var htmlXml = EmptyTagsToXml("Html", "ID", "File", "Binary");
            return htmlXml.Elements().ToList();
        }
    }

    [Serializable]
    public class TextBody : EmailBody
    {
        public TextBody()
        {
        }

        internal TextBody(string filePath) : base(filePath){}
        internal TextBody(int fileID) : base(fileID){}
        internal TextBody(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public static TextBody FromFile(string filePath)
        {
            return new TextBody(filePath);
        }

        public static TextBody FromText(string messageText)
        {
            var bytes = Encoding.UTF8.GetBytes(messageText);
            return new TextBody("message.txt", bytes);
        }

        public static TextBody FromByteArray(byte[] messageTextData)
        {
            return new TextBody("message.txt", messageTextData);
        }

        public static TextBody FromBase64String(string encodedMessageTextData)
        {
            var bytes = Convert.FromBase64String(encodedMessageTextData);
            return new TextBody("message.txt", bytes);
        }

        // TODO : should this part of PostAPI be made accessible through the client?
        //public static TextBody FromID(int listID)
        //{
        //    return new TextBody(listID);
        //}

        public override List<XElement> ToXml()
        {
            var textXml = ToXml("Text", "ID", "File", "Binary");
            return textXml.Elements().ToList();
        }

        public static List<XElement> EmptyTagsToXml()
        {
            var textXml = EmptyTagsToXml("Text", "ID", "File", "Binary");
            return textXml.Elements().ToList();
        }
    }
}

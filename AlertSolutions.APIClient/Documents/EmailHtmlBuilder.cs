using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    public class EmailHtmlBuilder
    {
        public EmailHtml FromFile(string filePath)
        {
            return new EmailHtml()
            {
                EmailHtmlName = System.IO.Path.GetFileName(filePath),
                EmailHtmlBinary = System.IO.File.ReadAllBytes(filePath)
            };
        }

        public EmailHtml FromText(string messageHtmlText)
        {
            var bytes = Encoding.UTF8.GetBytes(messageHtmlText);
            return new EmailHtml()
            {
                EmailHtmlName = "message.html",
                EmailHtmlBinary = bytes
            };
        }

        public EmailHtml FromByteArray(byte[] messageHtmlData)
        {
            return new EmailHtml()
            {
                EmailHtmlName = "message.html",
                EmailHtmlBinary = messageHtmlData
            };
        }

        public EmailHtml FromBase64String(string encodedMessageHtmlData)
        {
            var bytes = Convert.FromBase64String(encodedMessageHtmlData);
            return new EmailHtml()
            {
                EmailHtmlName = "message.html",
                EmailHtmlBinary = bytes
            };
        }

        public EmailHtml FromID(int htmlID)
        {
            return new EmailHtml()
            {
                EmailHtmlID = htmlID
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    public class EmailTextBuilder
    {
        public EmailText FromFile(string filePath)
        {
            return new EmailText()
            {
                EmailTextName = System.IO.Path.GetFileName(filePath),
                EmailTextBinary = System.IO.File.ReadAllBytes(filePath)
            };
        }

        public EmailText FromText(string messageHtmlText)
        {
            var bytes = Encoding.UTF8.GetBytes(messageHtmlText);
            return new EmailText()
            {
                EmailTextName = "message.txt",
                EmailTextBinary = bytes
            };
        }

        public EmailText FromByteArray(byte[] messageHtmlData)
        {
            return new EmailText()
            {
                EmailTextName = "message.txt",
                EmailTextBinary = messageHtmlData
            };
        }

        public EmailText FromBase64String(string encodedMessageHtmlData)
        {
            var bytes = Convert.FromBase64String(encodedMessageHtmlData);
            return new EmailText()
            {
                EmailTextName = "message.txt",
                EmailTextBinary = bytes
            };
        }

        public EmailText FromID(int htmlID)
        {
            return new EmailText()
            {
                EmailTextID = htmlID
            };
        }
    }
}

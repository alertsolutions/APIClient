using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    public class TextMessageBuilder
    {
        public TextMessage FromFile(string filePath)
        {
            return new TextMessage()
            {
                Message = System.IO.File.ReadAllText(filePath)
            };
        }

        public TextMessage FromText(string messageText)
        {
            return new TextMessage()
            {
                Message = messageText
            };
        }

        public TextMessage FromByteArray(byte[] messageTextData)
        {
            return new TextMessage()
            {
                Message = Encoding.UTF8.GetString(messageTextData)
            };
        }

        public TextMessage FromBase64String(string encodedMessageTextData)
        {
            return new TextMessage()
            {
                Message = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMessageTextData))
            };
        }

        public TextMessage FromID(int textMessageId)
        {
            return new TextMessage()
            {
                MessageID = textMessageId
            };
        }
    }
}

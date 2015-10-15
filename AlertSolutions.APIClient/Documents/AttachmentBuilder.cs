using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    public class AttachmentBuilder
    {
        public Attachment FromFile(string filePath)
        {
            return new Attachment()
            {
                AttachmentName = System.IO.Path.GetFileName(filePath),
                AttachmentBinary = System.IO.File.ReadAllBytes(filePath)
            };
        }

        public Attachment FromText(string attachmentFileName, string attachmentTextContent)
        {
            var bytes = Encoding.UTF8.GetBytes(attachmentTextContent);
            return new Attachment()
            {
                AttachmentName = attachmentFileName,
                AttachmentBinary = bytes
            };
        }

        public Attachment FromByteArray(string attachmentFileName, byte[] attachmentData)
        {
            return new Attachment()
            {
                AttachmentName = attachmentFileName,
                AttachmentBinary = attachmentData
            };
        }

        public Attachment FromBase64String(string attachmentFileName, string encodedAttachmentData)
        {
            var bytes = Convert.FromBase64String(encodedAttachmentData);
            return new Attachment()
            {
                AttachmentName = attachmentFileName,
                AttachmentBinary = bytes
            };
        }

        public Attachment FromID(int attachmentID)
        {
            return new Attachment()
            {
                AttachmentID = attachmentID
            };
        }
    }
}

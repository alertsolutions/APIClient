using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    public class Attachment : DocumentBase
    {
        internal Attachment(string filePath) : base(filePath){}
        internal Attachment(int fileID) : base(fileID){}
        internal Attachment(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public static Attachment FromFile(string filePath)
        {
            return new Attachment(filePath);
        }

        public static Attachment FromText(string attachmentFileName, string attachmentText)
        {
            var bytes = Encoding.UTF8.GetBytes(attachmentText);
            return new Attachment(attachmentFileName, bytes);
        }

        public static Attachment FromByteArray(string attachmentFileName, byte[] attachmentData)
        {
            return new Attachment(attachmentFileName, attachmentData);
        }

        public static Attachment FromBase64String(string attachmentFileName, string encodedAttachmentData)
        {
            var bytes = Convert.FromBase64String(encodedAttachmentData);
            return new Attachment(attachmentFileName, bytes);
        }

        // TODO should this be turned on?
        //public static Attachment FromID(int listID)
        //{
        //    return new Attachment(listID);
        //}


        public XElement ToXml()
        {
            return ToXml("Attachment");
        }
    }
}

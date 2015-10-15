using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class Attachment
    {
        public int AttachmentID { get; internal set; }
        public string AttachmentName { get; internal set; }
        public byte[] AttachmentBinary { get; internal set; }

        internal XElement ToXml()
        {
            return new DocumentElementBuilder().ToXml("Attachment", AttachmentID, AttachmentName, AttachmentBinary);
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class Attachment
    {
        internal int AttachmentID { get; set; }
        internal string AttachmentName { get; set; }
        internal byte[] AttachmentBinary { get; set; }

        internal XElement ToXml()
        {
            return new DocumentElementBuilder().ToXml("Attachment", AttachmentID, AttachmentName, AttachmentBinary);
        }
    }
}

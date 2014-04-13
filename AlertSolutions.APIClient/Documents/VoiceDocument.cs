using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    public enum VoiceDocumentType
    {
        Live,
        Message,
        CallScript,
    }

    [Serializable]
    public class VoiceDocument
    {
        internal int VoiceDocumentID { get; set; }
        internal string VoiceDocumentName { get; set; }
        internal byte[] VoiceDocumentBinary { get; set; }
        internal VoiceDocumentType VoiceDocumentType { get; set; }

        internal XElement ToXml()
        {
            var tags = new DocumentElementBuilder().ToXml("Document", VoiceDocumentID, VoiceDocumentName, VoiceDocumentBinary);
            tags.Add(new XElement("DocumentType", VoiceDocumentType));
            return tags;
        }
    }
}

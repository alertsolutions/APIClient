using System;
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
        public int VoiceDocumentID { get; internal set; }
        public string VoiceDocumentName { get; internal set; }
        public byte[] VoiceDocumentBinary { get; internal set; }
        public VoiceDocumentType VoiceDocumentType { get; internal set; }

        internal XElement ToXml()
        {
            var tags = new DocumentElementBuilder().ToXml("Document", VoiceDocumentID, VoiceDocumentName, VoiceDocumentBinary);
            tags.Add(new XElement("DocumentType", VoiceDocumentType));
            return tags;
        }
    }
}

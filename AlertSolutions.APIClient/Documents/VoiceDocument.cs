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

    // specific derived class specific to voice order documents since they need an extra documentType tag
    public class VoiceDocument : Document
    {
        internal VoiceDocumentType DocumentType { get; set; }

        internal VoiceDocument(string filePath) : base(filePath) { }
        internal VoiceDocument(int fileID) : base(fileID) { }
        internal VoiceDocument(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public static VoiceDocument FromBinary(string filePath, byte[] fileBinary, VoiceDocumentType type)
        {
            return new VoiceDocument(filePath, fileBinary) {DocumentType = type};
        }

        public static VoiceDocument FromFile(string filePath, VoiceDocumentType type)
        {
            return new VoiceDocument(filePath) {DocumentType = type};
        }

        public static VoiceDocument FromText(string voiceDocumentName, string voiceDocumentText, VoiceDocumentType type)
        {
            var bytes = Encoding.UTF8.GetBytes(voiceDocumentText);
            return new VoiceDocument(voiceDocumentName, bytes) { DocumentType = type };
        }

        public static VoiceDocument FromByteArray(string voiceDocumentName, byte[] voiceDocumentData, VoiceDocumentType type)
        {
            return new VoiceDocument(voiceDocumentName, voiceDocumentData) { DocumentType = type };
        }

        public static VoiceDocument FromBase64String(string voiceDocumentName, string encodedVoiceDocumentData, VoiceDocumentType type)
        {
            var bytes = Convert.FromBase64String(encodedVoiceDocumentData);
            return new VoiceDocument(voiceDocumentName, bytes) { DocumentType = type };
        }

        public static VoiceDocument FromID(int listID, VoiceDocumentType type)
        {
            return new VoiceDocument(listID) { DocumentType = type };
        }

        public override XElement ToXml()
        {
            var doc = base.ToXml();
            doc.Add(new XElement("DocumentType", DocumentType));
            return doc;
        }
    }
}

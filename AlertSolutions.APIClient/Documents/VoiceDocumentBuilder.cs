using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    public class VoiceDocumentBuilder
    {
        public VoiceDocument FromFile(string filePath, VoiceDocumentType type)
        {
            return new VoiceDocument()
            {
                VoiceDocumentName = System.IO.Path.GetFileName(filePath),
                VoiceDocumentBinary = System.IO.File.ReadAllBytes(filePath),
                VoiceDocumentType = type
            };
        }

        public VoiceDocument FromText(string voiceDocumentName, string voiceDocumentText, VoiceDocumentType type)
        {
            var bytes = Encoding.UTF8.GetBytes(voiceDocumentText);
            return new VoiceDocument()
            {
                VoiceDocumentName = voiceDocumentName,
                VoiceDocumentBinary = bytes,
                VoiceDocumentType = type
            };
        }

        public VoiceDocument FromByteArray(string voiceDocumentName, byte[] voiceDocumentData, VoiceDocumentType type)
        {
            return new VoiceDocument()
            {
                VoiceDocumentName = voiceDocumentName,
                VoiceDocumentBinary = voiceDocumentData,
                VoiceDocumentType = type
            };
        }

        public VoiceDocument FromBase64String(string voiceDocumentName, string encodedVoiceDocumentData, VoiceDocumentType type)
        {
            var bytes = Convert.FromBase64String(encodedVoiceDocumentData);
            return new VoiceDocument()
            {
                VoiceDocumentName = voiceDocumentName,
                VoiceDocumentBinary = bytes,
                VoiceDocumentType = type
            };
        }

        public VoiceDocument FromID(int voiceDocumentId, VoiceDocumentType type)
        {
            return new VoiceDocument()
            {
                VoiceDocumentID = voiceDocumentId,
                VoiceDocumentType = type
            };
        }
    }
}

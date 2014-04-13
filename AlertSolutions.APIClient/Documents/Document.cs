using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    // actual implementation of a document object
    // this class may seem a little redundant, but it's used where orders in
    // postAPI have lists of documents, so it has a generic name
    // This class should be thought of like a sibling to Attachments or ContactList
    // rather than something they would inherit from, which is DocumentBase
    [Serializable]
    public class Document : DocumentBase
    {
        public Document()
        {
        }

        internal Document(string filePath) : base(filePath){}
        internal Document(int fileID) : base(fileID){}
        internal Document(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public static Document FromFile(string filePath)
        {
            return new Document(filePath);
        }

        //public static Document FromString(string documentName, string document)
        //{
        //    var bytes = Utilities.ConvertToByteArray(documentName);
        //    return new Document(document, bytes);
        //}

        public static Document FromText(string documentName, string documentText)
        {
            var bytes = Encoding.UTF8.GetBytes(documentText);
            return new Document(documentName, bytes);
        }

        public static Document FromByteArray(string documentName, byte[] documentData)
        {
            return new Document(documentName, documentData);
        }

        public static Document FromBase64String(string documentName, string encodedDocumentData)
        {
            var bytes = Convert.FromBase64String(documentName);
            return new Document(documentName, bytes);
        }

        public static Document FromID(int listID)
        {
            return new Document(listID);
        }

        public virtual XElement ToXml()
        {
            return ToXml("Document");
        }

        public static XElement EmptyTagsToXml()
        {
            return EmptyTagsToXml("Document");
        }
    }
}

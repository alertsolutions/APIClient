using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    // base class with code/structure common to all document like objects in the API
    public abstract class DocumentBase
    {
        private int _fileID { get; set; }
        private string _fileName { get; set; }
        private byte[] _fileBinary { get; set; }

        internal DocumentBase(string filePath)
        {
            _fileName = System.IO.Path.GetFileName(filePath);
            _fileBinary = System.IO.File.ReadAllBytes(filePath);
            _fileID = -1; // default that indicates not to use it
        }

        internal DocumentBase(string fileName, byte[] fileBinary)
        {
            _fileName = fileName;
            _fileBinary = fileBinary;
            _fileID = -1; // default that indicates not to use it
        }

        internal DocumentBase(int fileID)
        {
            _fileID = fileID;
        }

        protected XElement ToXml(string tagName,
            string iDTag = "ID", string nameTag = "Name", string binaryTag = "Binary")
        {
            var xfileID = new XElement(tagName + iDTag);
            var xfileName = new XElement(tagName + nameTag);
            var xfileBinary = new XElement(tagName + binaryTag);

            if (_fileID == -1)
            {
                xfileName.Value = _fileName;
                xfileBinary.Value = Base64BinaryFile();
            }
            else
            {
                xfileID.Value = _fileID.ToString();
            }

            var xFile = new XElement(tagName);
            xFile.Add(xfileID);
            xFile.Add(xfileName);
            xFile.Add(xfileBinary);

            return xFile;
        }

        public static XElement EmptyTagsToXml(string tagName,
            string iDTag = "ID", string nameTag = "Name", string binaryTag = "Binary")
        {
            var xfileID = new XElement(tagName + iDTag);
            var xfileName = new XElement(tagName + nameTag);
            var xfileBinary = new XElement(tagName + binaryTag);
            
            var xFile = new XElement(tagName);
            xFile.Add(xfileID);
            xFile.Add(xfileName);
            xFile.Add(xfileBinary);

            return xFile;
        }

        public string GetName()
        {
            return _fileName;
        }

        private string Base64BinaryFile()
        {
            return _fileBinary == null ? "" : Convert.ToBase64String(_fileBinary);
        }
    }
}

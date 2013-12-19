using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AlertSolutions.API.Templates
{
    public class Template
    {
        protected string _fileName { get; set; }
        protected byte[] _fileBinary { get; set; }

        protected Template(string filePath)
        {
            _fileName = Path.GetFileName(filePath);
            _fileBinary = File.ReadAllBytes(filePath);
        }

        protected Template(string fileName, byte[] fileBinary)
        {
            _fileName = fileName;
            _fileBinary = fileBinary;
        }

        public static Template FromFile(string filePath)
        {
            return new Template(filePath);
        }

        public static Template FromBinary(string fileName, byte[] fileBinary)
        {
            return new Template(fileName, fileBinary);
        }

        public XElement ToXml()
        {
            var xfileName = new XElement("FileName");
            var xfileBinary = new XElement("FileBinary");

            xfileName.Value = _fileName;
            xfileBinary.Value = Base64BinaryFile();

            var xTemplate = new XElement("Template");
            xTemplate.Add(xfileName);
            xTemplate.Add(xfileBinary);

            var xTemplates = new XElement("Templates");
            xTemplates.Add(xTemplate);
            return xTemplates;
        }

        private string Base64BinaryFile()
        {
            return _fileBinary == null ? "" : Convert.ToBase64String(_fileBinary);
        }
    }
}

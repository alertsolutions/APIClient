using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    internal class DocumentElementBuilder
    {
        internal XElement ToXml(string tagName, int id, string name, byte[] value,
            string iDTag = "ID", string nameTag = "Name", string binaryTag = "Binary")
        {
            var xfileID = new XElement(tagName + iDTag);
            var xfileName = new XElement(tagName + nameTag);
            var xfileBinary = new XElement(tagName + binaryTag);

            if (id < 1)
            {
                xfileName.Value = name;
                xfileBinary.Value = value == null ? "" : Convert.ToBase64String(value);
            }
            else
            {
                xfileID.Value = id.ToString();
            }

            var xFile = new XElement(tagName);
            xFile.Add(xfileID);
            xFile.Add(xfileName);
            xFile.Add(xfileBinary);

            return xFile;
        }

        internal XElement EmptyTagsToXml(string tagName,
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
    }
}
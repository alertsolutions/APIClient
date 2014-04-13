using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class EmailText
    {
        internal int EmailTextID { get; set; }
        internal string EmailTextName { get; set; }
        internal byte[] EmailTextBinary { get; set; }

        internal XElement ToXml()
        {
            return new DocumentElementBuilder().ToXml("Text", EmailTextID, EmailTextName, EmailTextBinary, "ID", "File");
        }

        internal List<XElement> EmptyTagsToXml()
        {
            var textXml = new DocumentElementBuilder().EmptyTagsToXml("Text", "ID", "File", "Binary");
            return textXml.Elements().ToList();
        }
    }
}

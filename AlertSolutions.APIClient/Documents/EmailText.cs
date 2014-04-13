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

        internal List<XElement> ToXml()
        {
            var xml = new DocumentElementBuilder().ToXml("Text", EmailTextID, EmailTextName, EmailTextBinary, "ID", "File");
            return xml.Elements().ToList();
        }

        internal List<XElement> EmptyTagsToXml()
        {
            var xml = new DocumentElementBuilder().EmptyTagsToXml("Text", "ID", "File", "Binary");
            return xml.Elements().ToList();
        }
    }
}

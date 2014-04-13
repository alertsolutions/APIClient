using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class EmailHtml
    {
        internal int EmailHtmlID { get; set; }
        internal string EmailHtmlName { get; set; }
        internal byte[] EmailHtmlBinary { get; set; }

        internal List<XElement> ToXml()
        {
            var xml = new DocumentElementBuilder().ToXml("Html", EmailHtmlID, EmailHtmlName, EmailHtmlBinary, "ID", "File");
            return xml.Elements().ToList();
        }

        internal List<XElement> EmptyTagsToXml()
        {
            var xml = new DocumentElementBuilder().EmptyTagsToXml("Text", "ID", "File", "Binary");
            return xml.Elements().ToList();
        }
    }
}

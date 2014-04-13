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

        internal XElement ToXml()
        {
            return new DocumentElementBuilder().ToXml("Html", EmailHtmlID, EmailHtmlName, EmailHtmlBinary, "ID", "File");
        }

        internal List<XElement> EmptyTagsToXml()
        {
            var textXml = new DocumentElementBuilder().EmptyTagsToXml("Text", "ID", "File", "Binary");
            return textXml.Elements().ToList();
        }
    }
}

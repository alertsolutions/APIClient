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
        public int EmailHtmlID { get; internal set; }
        public string EmailHtmlName { get; internal set; }
        public byte[] EmailHtmlBinary { get; internal set; }

        internal List<XElement> ToXml()
        {
            var xml = new DocumentElementBuilder().ToXml("Html", EmailHtmlID, EmailHtmlName, EmailHtmlBinary, "ID", "File");
            return xml.Elements().ToList();
        }

        internal List<XElement> EmptyTagsToXml()
        {
            var xml = new DocumentElementBuilder().EmptyTagsToXml("Html", "ID", "File", "Binary");
            return xml.Elements().ToList();
        }
    }
}

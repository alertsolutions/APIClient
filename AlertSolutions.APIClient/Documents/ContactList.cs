using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class ContactList
    {
        public int ContactListID { get; internal set; }
        public string ContactListName { get; internal set; }
        public byte[] ContactListBinary { get; internal set; }

        internal List<XElement> ToXml()
        {
            var listXml = new DocumentElementBuilder().ToXml("List", ContactListID, ContactListName, ContactListBinary);
            return listXml.Elements().ToList();
        }
    }
}

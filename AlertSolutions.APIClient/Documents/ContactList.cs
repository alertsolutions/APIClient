using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class ContactList
    {
        public int ContactListID { get; set; }
        public string ContactListName { get; set; }
        public byte[] ContactListBinary { get; set; }

        public List<XElement> ToXml()
        {
            var listXml = new DocumentElementBuilder().ToXml("List", ContactListID, ContactListName, ContactListBinary);
            return listXml.Elements().ToList();
        }
    }
}

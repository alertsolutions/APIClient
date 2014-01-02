using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    [Serializable]
    public class ContactList : DocumentBase
    {
        public ContactList()
        {
        }

        internal ContactList(string filePath) : base(filePath) { }
        internal ContactList(int fileID) : base(fileID) { }
        internal ContactList(string fileName, byte[] fileBinary) : base(fileName, fileBinary) { }

        public static ContactList FromFile(string filePath)
        {
            return new ContactList(filePath);
        }

        public static ContactList FromText(string listName, string listText)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(listText);
            return new ContactList(listName, bytes);
        }

        public static ContactList FromByteArray(string listName, byte[] listData)
        {
            return new ContactList(listName, listData);
        }

        public static ContactList FromBase64String(string listName, string encodedListData)
        {
            var bytes = Convert.FromBase64String(encodedListData);
            return new ContactList(listName, bytes);
        }

        public static ContactList FromID(int listID)
        {
            return new ContactList(listID);
        }

        public List<XElement> ToXml()
        {
            var xList = new List<XElement>();

            var listXml = ToXml("List");

            foreach (var element in listXml.Elements())
                xList.Add(element);

            return xList;
        }
    }
}

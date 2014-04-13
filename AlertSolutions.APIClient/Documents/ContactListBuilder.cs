using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Documents
{
    public class ContactListBuilder
    {
        public ContactList FromFile(string filePath)
        {
            return new ContactList()
            {
                ContactListName = System.IO.Path.GetFileName(filePath),
                ContactListBinary = System.IO.File.ReadAllBytes(filePath)
            };
        }

        public ContactList FromText(string listName, string listText)
        {
            var bytes = Encoding.UTF8.GetBytes(listText);
            return new ContactList()
            {
                ContactListName = listName,
                ContactListBinary = bytes
            };
        }

        public ContactList FromByteArray(string listName, byte[] listData)
        {
            return new ContactList()
            {
                ContactListName = listName,
                ContactListBinary = listData
            };
        }

        public ContactList FromBase64String(string listName, string encodedListData)
        {
            var bytes = Convert.FromBase64String(encodedListData);
            return new ContactList()
            {
                ContactListName = listName,
                ContactListBinary = bytes
            };
        }

        public ContactList FromID(int listID)
        {
            return new ContactList()
            {
                ContactListID = listID
            };
        }
    }
}

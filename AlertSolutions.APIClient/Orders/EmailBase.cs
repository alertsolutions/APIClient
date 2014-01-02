using System;
using System.Collections.Generic;
using System.Xml.Linq;

using AlertSolutions.API.Documents;

namespace AlertSolutions.API.Orders
{
    [Serializable]
    // handle code common to ET and EB
    public abstract class EmailBase : Order
    {
        public string DisplayName { get; set; }
        public string EmailFrom { get; set; }
        public string EmailReplyTo { get; set; }
        public string EmailSubject { get; set; }

        public bool IsForward { get; set; }
        public bool IsReplaceLink { get; set; }
        public bool IsUnsubscribe { get; set; }

        public TextBody TextBody { get; set; }
        public HtmlBody HtmlBody { get; set; }

        private List<Attachment> _attachments;
        public List<Attachment> Attachments
        {
            get { return _attachments ?? (_attachments = new List<Attachment>()); }
            set { _attachments = value; }
        }

        internal EmailBase() : base()
        {
            this.TextBody = null; // default to null, users responsiblity to set
            this.HtmlBody = null;
            this.DisplayName = "";
            this.EmailFrom = "";
            this.EmailReplyTo = "";
            this.EmailSubject = "";
            this.IsForward = false;
            this.IsReplaceLink = false;
            this.IsUnsubscribe = false;
        }

        protected override XDocument BuildXml()
        {
            if (TextBody == null && HtmlBody == null)
            {
                throw new FormatException("Must set either an html email body, or a text email body.");
            }

            var xmlDoc = base.BuildXml();
            var orderTag = xmlDoc.Element("Orders").Element("Order");

            orderTag.Add(new XElement("DisplayName", this.DisplayName));
            orderTag.Add(GetAttachmentsAsElement());


            var htmlElements = HtmlBody == null ? HtmlBody.EmptyTagsToXml() : HtmlBody.ToXml();
            foreach (var el in htmlElements)
            {
                orderTag.Add(el);
            }

            var textElements = TextBody == null ? TextBody.EmptyTagsToXml() : TextBody.ToXml();
            foreach (var el in textElements)
            {
                orderTag.Add(el);
            }

            return xmlDoc;
        }

        private XElement GetAttachmentsAsElement()
        {
            var xAttachs = new XElement("Attachments");
            if (Attachments.Count > 0)
            {
                Attachments.ForEach(att => xAttachs.Add(att.ToXml()));
            }
            else
            {
                xAttachs.Add(new XElement("Attachment",
                        new XElement("AttachmentID"),
                        new XElement("AttachmentName"),
                        new XElement("AttachmentBinary")
                        ));
            }
            return xAttachs;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSolutions.API.Documents;

namespace AlertSolutions.APIClient.Test.TestFiles
{
    public class DocumentSamples
    {
        public static string GetSampleCsv()
        {
            return "header1, header2, header3\r\nrow1, value1, value2\r\nrow2, value1, value2\r\nrow3, value1, value2";
        }

        public static string GetSampleEmailHtml()
        {
            return "<b>HTML EMAIL</b><br/>This is a test email message <i>(in html)</i>.";
        }

        public static string GetSampleEmailText()
        {
            return "This is a test email message.";
        }

        public static string GetSampleContactListCsv()
        {
            return @"fname,lname,phone,email,fax
            james,thomas,4013002095,jthomas@blimessaging.com,4013002095
            james,thomas,4013002095,jthomas@alertsolutions.com,4013002095";
        }

        public static string GetSampleFaxMessage()
        {
            return "This is a test fax sent using the API Client.";
        }

        public static string GetSampleTextMessage()
        {
            return "This was sent to you phone via the Alert Solutions API Client!";
        }

        public static string GetSampleVoiceMessage()
        {
            return "Hi #fname# this is a voice message testing the API client.";
        }

        // what is the correct way to load / store this?
        //public static byte[] GetSamplePdf()
        //{
        //    var pdf = new StreamReader(GetType().Assembly.GetManifestResourceStream("AlertSolutions.APIClient.Test.TestFiles.Attachment.txt")).ReadToEnd();
        //    return Encoding.UTF8.GetBytes(pdf);
        //}
    }
}

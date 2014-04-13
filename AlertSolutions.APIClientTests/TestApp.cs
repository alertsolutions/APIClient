using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlertSolutions.API;
using AlertSolutions.API.Orders;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Broadcasts;

namespace AlertSolutions.APIClientExamples
{
    public static class TestApp
    {
        public static void Execute()
        {
            Console.WriteLine("Alert Solutions API Client Test\r\n\r\n");

            ApiClient client = new ApiClient("http://weblaunch.blifax.com/postapi", "jthomas@blimessaging.com", "pentagon625");
            List<IOrder> orders = new List<IOrder>();

            orders.Add(CreateET());
            orders.Add(CreateEB());
            orders.Add(CreateMT());
            orders.Add(CreateML());
            orders.Add(CreateVT());
            orders.Add(CreateVL());
            orders.Add(CreateTL());
            orders.Add(CreateWL());

            foreach (var order in orders)
            {
                Console.WriteLine("About to generate a " + order.TypeOfOrder + " order.");
                Console.WriteLine("[Enter] to continue...");
                Console.WriteLine("[s] to skip...");
                var input = Console.ReadLine();
                if (input.ToUpper() != "S")
                {
                    Console.WriteLine(order.ToXml());
                    SendOrder(client, order);
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        public static void SendOrder(ApiClient client, IOrder order)
        {
            Console.WriteLine("Press any key to send " + order.TypeOfOrder + " order.");
            Console.WriteLine("[s] to skip...");
            var input = Console.ReadLine();
            if (input.ToUpper() == "S")
            {
                return;
            }

            Console.WriteLine("\nSending " + order.TypeOfOrder);
            var response = client.SendOrder(order);
            if (response.RequestResult == RequestResultType.Error)
            {
                Console.WriteLine(response.RequestErrorMessage);
            }
            else
            {
                Console.WriteLine("Return Status: " + response.RequestResult);
                Console.WriteLine("OrderID: " + response.OrderID);
                if (order is IMessage)
                {
                    Console.WriteLine("TransID: " + response.Unqid);
                }
            }

            Console.WriteLine("Press any key to fetch the report.");
            Console.ReadLine();

            var report = client.GetOrderReport(response, ReportReturnType.XML);

            if (report.RequestResult == RequestResultType.Error)
            {
                Console.WriteLine("Error: " + report.RequestErrorMessage);
            }
            else
            {
                Console.WriteLine("Report: " + report.ReportData);
            }
        }


        public static IOrder CreateET()
        {
            var et = new EmailMessage();

            et.EmailTo = "jthomas@blimessaging.com";
            et.EmailSubject = "PostAPIClient Refactor Test (ET)";
            et.EmailReplyTo = "jthomas@blimessaging.com";
            et.EmailFrom = "jthomas@blimessaging.com";
            et.DisplayName = "JThomas from AlertSolutions";
            et.Attachments = new List<Attachment>() { new AttachmentBuilder().FromFile("TestFiles\\Attachment.txt") };
            et.TextBody = TextBody.FromFile("TestFiles\\Email.txt");
            et.HtmlBody = HtmlBody.FromFile("TestFiles\\Email.html");
            return et;
        }

        public static IOrder CreateEB()
        {
            var f = System.IO.File.ReadAllBytes("testfiles\\report-card-sample.pdf");
            var b = Convert.ToBase64String(f);
            var eb = new EmailBroadcast();
            
            eb.BillCode = "PostAPIClient Refactor Test";
            eb.ProjectCode = "PostAPIClient Refactor Test";
            eb.EmailSubject = "PostAPIClient Refactor Test (EB)";
            eb.EmailReplyTo = "jthomas@blimessaging.com";
            eb.EmailFrom = "jthomas@blimessaging.com";
            eb.DisplayName = "JThomas from AlertSolutions";
            //eb.Attachments = new List<Attachment>() { Attachment.FromBase64String("file.pdf",b) };
            eb.Attachments = new List<Attachment>() { new AttachmentBuilder().FromText("file.txt", "This is a test message attachment.") };
            eb.TextBody = TextBody.FromFile("TestFiles\\Email.txt");
            eb.HtmlBody = HtmlBody.FromFile("TestFiles\\Email.html");
            eb.List = new ContactListBuilder().FromFile("TestFiles\\ContactList.csv");
            eb.EmailHeader = "email";
            eb.Proofs = new List<string>() { "jthomas@blimessaging.com" };
            return eb;
        }

        public static IOrder CreateMT()
        {
            var mt = new SMSMessage();
            mt.Number = "5086128160";
            //mt.Number = 4013002095;
            mt.ShortCode = "77811";
            //mt.TextMessage = TextMessage.FromString("This is not in base 64!");
            mt.TextMessage = TextMessage.FromFile("TestFiles\\TextMessage.txt");
            return mt;
        }
        public static IOrder CreateML()
        {
            var ml = new SMSBroadcast();
            ml.BillCode = "PostAPIClient Refactor Test";
            ml.ProjectCode = "PostAPIClient Refactor Test";
            ml.ShortCode = "77811";
            ml.TextMessage = TextMessage.FromFile("TestFiles\\TextMessage.txt");
            ml.List = new ContactListBuilder().FromFile("TestFiles\\ContactList.csv");
            ml.SMSHeader = "phone";
            ml.Proofs = new List<string>() { "4013002095" };
            return ml;
        }
        public static IOrder CreateVT()
        {
            var vt = new VoiceMessage();
            //vt.CallScript = CallScript.FromString("");
            //vt.CallScript = CallScript.FromFile("");
            vt.Phone = "4013002095";
            vt.CallerID = "4015555555";
            vt.StopTimeUTC = DateTime.UtcNow;
            vt.Documents = new List<Document>()
            { 
                VoiceDocument.FromFile("TestFiles\\VoiceMessage.txt", VoiceDocumentType.Live),
                VoiceDocument.FromFile("TestFiles\\VoiceMessage.txt", VoiceDocumentType.Message),
            }; 
            return vt;
        }
        public static IOrder CreateVL()
        {
            var vl = new VoiceBroadcast();
            //vt.CallScript = CallScript.FromString("");
            //vt.CallScript = CallScript.FromFile("");
            vl.BillCode = "PostAPIClient Refactor Test";
            vl.ProjectCode = "PostAPIClient Refactor Test";
            vl.CallerID = "4015555555";
            vl.List = new ContactListBuilder().FromFile("TestFiles\\ContactList.csv");
            vl.VoiceHeader = "phone";
            vl.ThrottleType = VoiceBroadcast.VoiceThrottleType.MaximumCalls;
            vl.ThrottleNumber = 2;
            vl.Documents = new List<Document>()
            { 
                VoiceDocument.FromFile("TestFiles\\VoiceMessage.txt", VoiceDocumentType.Live),
                VoiceDocument.FromFile("TestFiles\\VoiceMessage.txt", VoiceDocumentType.Message),  
            };
            return vl;
        }
        public static IOrder CreateTL()
        {
            var tl = new FaxMessage();
            tl.FaxFrom = "james";
            tl.FaxNumber = "4013002095";
            tl.DocumentStyle = "Letter";
            tl.Documents = new List<Document>()
            { 
                Document.FromFile("TestFiles\\FaxText.txt"),
            };
            return tl;
        }
        public static IOrder CreateWL()
        {
            var wl = new FaxBroadcast();
            wl.BillCode = "PostAPIClient Refactor Test";
            wl.ProjectCode = "PostAPIClient Refactor Test";
            wl.FaxFrom = "JThomas";
            wl.List = new ContactListBuilder().FromFile("TestFiles\\FaxList1.csv");
            wl.FaxHeader = "faxnumber";
            wl.Dedup = true;
            wl.DocumentStyle = "Letter";
            wl.Documents = new List<Document>()
            { 
                Document.FromFile("TestFiles\\FaxText.txt"),
            };
            return wl;
        }
    }
}

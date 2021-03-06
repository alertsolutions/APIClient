﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlertSolutions.API;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Messages;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Orders;

namespace AlertSolutions.APIClientExamples.Demos
{
    public static class FullDemo
    {
        public static void Execute()
        {
            Console.WriteLine("Alert Solutions API Client FullDemo\n");

            // ===========================================
            //              BROADCASTS
            // ===========================================

            List<IBroadcast> broadcasts = new List<IBroadcast>();

            // EMAIL BROADCAST
            var eb = new EmailBroadcast();
            eb.BillCode = "APIClient Demo";
            eb.ProjectCode = "APIClient Demo";
            eb.DisplayName = "AlertSolutions";
            eb.EmailSubject = "Alert Solutions API Client Demo";
            eb.EmailFrom = "example@alertsolutions.com";
            eb.EmailReplyTo = "example@alertsolutions.com";
            eb.Attachments = new List<Attachment>() { new AttachmentBuilder().FromFile("Files\\Attachment.txt") };
            eb.List = new ContactListBuilder().FromFile("Files\\ContactList.csv");
            eb.EmailHeader = "email";
            eb.Proofs = new List<string>() { "example@alertsolutions.com" };
            // can use either Text or HTML
            eb.HtmlBody = new EmailHtmlBuilder().FromFile("Files\\Email.html");
            eb.TextBody = new EmailTextBuilder().FromFile("Files\\Email.txt");

            broadcasts.Add(eb);

            // SMS BROADCAST
            var sb = new SMSBroadcast();
            sb.BillCode = "APIClient Demo";
            sb.ProjectCode = "APIClient Demo";
            sb.ShortCode = "77811";
            sb.TextMessage = new TextMessageBuilder().FromFile("Files\\TextMessage.txt");
            sb.List = new ContactListBuilder().FromFile("Files\\ContactList.csv");
            sb.SMSHeader = "phone";
            sb.Proofs = new List<string>() { "5555555555" };

            broadcasts.Add(sb);

            // VOICE BROADCAST
            var vb = new VoiceBroadcast();
            vb.BillCode = "APIClient Demo";
            vb.ProjectCode = "APIClient Demo";
            vb.CallerID = "5555555555";
            vb.List = new ContactListBuilder().FromFile("Files\\ContactList.csv");
            vb.VoiceHeader = "phone";
            vb.ThrottleType = VoiceBroadcast.VoiceThrottleType.MaximumCalls;
            vb.ThrottleNumber = 2;
            // can be told to load an xml file, or be given the xml as text
            //vt.CallScript = CallScript.FromString("");
            //vt.CallScript = CallScript.FromFile("");
            vb.Documents = new List<VoiceDocument>()
            { 
                //VoiceDocument.FromFile("Files\\VoiceMessage.txt", VoiceDocumentType.Live),
                new VoiceDocumentBuilder().FromFile("Files\\VoiceMessage.txt", VoiceDocumentType.Message),  
            };

            broadcasts.Add(vb);

            // FAX BROADCAST
            var fb = new FaxBroadcast();
            fb.BillCode = "APIClient Demo";
            fb.ProjectCode = "APIClient Demo";
            fb.FaxFrom = "john doe";
            fb.List = new ContactListBuilder().FromFile("Files\\FaxList.csv");
            fb.FaxHeader = "faxnumber";
            fb.Dedup = true;
            fb.DocumentStyle = DocumentStyle.Letter;
            fb.Documents = new List<FaxDocument>()
            { 
                new FaxDocumentBuilder().FromFile("Files\\FaxText.txt"),
            };

            broadcasts.Add(fb);
            
            // ===========================================
            //             MESSAGES
            // ===========================================

            List<IMessage> messages = new List<IMessage>();

            // EMAIL MESSAGE
            var em = new EmailMessage();
            em.EmailTo = "example@alertsolutions.com";
            em.DisplayName = "AlertSolutions";
            em.EmailSubject = "Alert Solutions API Client Demo";
            em.EmailFrom = "example@alertsolutions.com";
            em.EmailReplyTo = "example@alertsolutions.com";
            em.Attachments = new List<Attachment>() { new AttachmentBuilder().FromFile("Files\\Attachment.txt") };
            // can use either Text or HTML
            em.TextBody = new EmailTextBuilder().FromFile("Files\\Email.txt");
            //em.EmailBody = HtmlBody.FromFile("Files\\Email.html");

            messages.Add(em);

            // SMS MESSAGE
            var sm = new SMSMessage();
            sm.Number = "5555555555";
            sm.ShortCode = "77811"; // shared Alert Solutions Shortcode
            sm.TextMessage = new TextMessageBuilder().FromFile("Files\\TextMessage.txt");

            messages.Add(sm);

            // VOICE MESSAGE
            var vm = new VoiceMessage();
            //vt.CallScript = CallScript.FromString("");
            //vt.CallScript = CallScript.FromFile("");
            vm.Phone = "5555555555";
            vm.CallerID = "5555555555";
            vm.Documents = new List<VoiceDocument>()
            { 
                new VoiceDocumentBuilder().FromFile("Files\\VoiceMessage.txt", VoiceDocumentType.Live),
                new VoiceDocumentBuilder().FromFile("Files\\VoiceMessage.txt", VoiceDocumentType.Message),
            };

            messages.Add(vm);

            // FAX MESSAGE
            var fm = new FaxMessage();
            fm.FaxFrom = "john doe";
            fm.FaxNumber = "4014271405";
            fm.DocumentStyle = DocumentStyle.Letter;
            fm.Documents = new List<FaxDocument>()
            { 
                new FaxDocumentBuilder().FromFile("Files\\FaxText.txt"),
            };

            messages.Add(fm);


            // ===========================================
            //             LAUNCH
            // ===========================================

            // change these values to your API login
            var user = "user";
            var password = "password";
            var url = "http://weblaunch.blifax.com/postapi";

            // create the client
            var client = new ApiClient(url, user, password);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("There is a pause of about 10 seconds before getting a report.");
            Console.ForegroundColor = ConsoleColor.Gray;

            // launch the broadcasts
            foreach (var broadcast in broadcasts)
            {
                Console.Write("\n\nSending " + broadcast.TypeOfOrder + ": ");
                
                var response = client.LaunchBroadcast(broadcast);

                Console.Write("** " + response.RequestResult + " **");
                Console.Write(response.RequestResult != RequestResultType.Error ?
                    " OrderID: " + response.OrderID : "Error: " + response.RequestErrorMessage);

                // wait for broadcast before request report
                System.Threading.Thread.Sleep(12000);
                var report = client.GetOrderReport(response, ReportReturnType.XML);

                Console.Write("\nReport:\n" + report.ReportData);
            }

            // launch the messages
            foreach (var message in messages)
            {
                Console.Write("\n\nSending " + message.TypeOfOrder + ": ");

                var response = client.LaunchMessage(message);

                Console.Write("** " + response.RequestResult + " **");
                Console.Write(response.RequestResult != RequestResultType.Error ?
                    " OrderID: " + response.OrderID  + " TransID: " + response.Unqid : "Error: " + response.RequestErrorMessage);

                // wait for message before request report
                System.Threading.Thread.Sleep(10000);
                var report = client.GetOrderReport(response, ReportReturnType.XML);

                Console.Write("\nReport:\n" + report.ReportData);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}

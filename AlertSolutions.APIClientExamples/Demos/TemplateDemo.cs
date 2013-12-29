using AlertSolutions.API;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Documents;
using AlertSolutions.API.Templates;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace AlertSolutions.APIClientExamples.Demos
{
    public class TemplateDemo
    {
        public static void Execute()
        {
            Console.WriteLine("Alert Solutions API Client TemplateDemo\n");

            Template template = Template.FromFile("Location\\Of\\Wav\\File.wav");

            // change these values to your API login
            // change your login details in the app config file
            var user = "";
            var password = "";
            var url = "http://weblaunch.blifax.com/postapi";

            var templateClient = new ApiClient(url, user, password);

            Console.WriteLine("\nPress any key to submit template.");
            Console.ReadLine();

            TemplateResponse response = templateClient.SendTemplates(template);


            var orderClient = new ApiClient(url, user, password);

            var vb = new VoiceBroadcast();
            vb.BillCode = "APIClient Demo";
            vb.ProjectCode = "APIClient Demo";
            vb.CallerID = "5555555555";
            vb.List = ContactList.FromFile("Files\\ContactList.csv");
            vb.VoiceHeader = "phone";
            vb.ThrottleType = VoiceBroadcast.VoiceThrottleType.MaximumCalls;
            vb.ThrottleNumber = 2;
            vb.Documents = new List<Document>()
            { 
                //VoiceDocument.FromFile("Files\\VoiceMessage.txt", VoiceDocumentType.Live),
                VoiceDocument.FromFile("Files\\VoiceMessage.txt", VoiceDocumentType.Message),  
            };

            OrderResponse broadcastResponse = orderClient.LaunchBroadcast(vb);

            Console.WriteLine(broadcastResponse.RequestErrorMessage);
        }
    }
}

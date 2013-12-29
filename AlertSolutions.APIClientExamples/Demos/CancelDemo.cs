using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertSolutions.API;
using AlertSolutions.API.Broadcasts;
using AlertSolutions.API.Documents;

namespace AlertSolutions.APIClientExamples.Demos
{
    public static class CancelDemo
    {
        public static void Execute()
        {
            Console.WriteLine("Alert Solutions API Client SimpleDemo\n");

            var attachments = new List<Attachment>();
            attachments.Add(Attachment.FromFile("Files\\Attachment.txt"));

            var proofs = new List<string>();
            proofs.Add("example@alertsolutions.com");

            // create an email broadcast and give it the desired values
            var eb = new EmailBroadcast();
            eb.BillCode = "APIClient Demo";
            eb.ProjectCode = "APIClient Demo";
            eb.DisplayName = "AlertSolutions";
            eb.EmailSubject = "Alert Solutions API Client Demo";
            eb.EmailFrom = "example@alertsolutions.com";
            eb.EmailReplyTo = "example@alertsolutions.com";
            eb.TextBody = TextBody.FromFile("Files\\Email.txt");
            // be sure to modify the ContactList.csv file so the email goes to your intended destinations
            eb.List = ContactList.FromFile("Files\\ContactList.csv");
            eb.EmailHeader = "email";
            eb.Attachments = attachments;
            eb.Proofs = proofs;

            // change these values to your API login
            var user = "jthomas@blimessaging.com";
            var password = "pentagon625";
            var url = "http://weblaunch.blifax.com/postapi";

            // create the client
            var client = new ApiClient(url, user, password);

            Console.WriteLine("\nPress any key to launch broadcast.");
            Console.ReadLine();

            // launch the broadcast
            var response = client.LaunchBroadcast(eb);
            Console.WriteLine("Response Status: " + response.RequestResult);
            if (response.RequestResult == RequestResultType.Error)
                Console.WriteLine("Error: " + response.RequestErrorMessage);
            else
                Console.WriteLine("OrderID: " + response.OrderID);

            Console.WriteLine("\nPress any key to cancel broadcast.");
            Console.ReadLine();

            var cancelResponse = client.CancelOrder(response);
            Console.WriteLine("\n\n"+ cancelResponse);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using AlertSolutions.API;

namespace AlertSolutions.APIClientExamples.Demos
{
    class ReportDemo
    {
        public static void Execute()
        {
            // change your login details in the app config file
            var user = ConfigurationManager.AppSettings["user"];
            var password = ConfigurationManager.AppSettings["password"];
            var url = ConfigurationManager.AppSettings["postAPIurl"];

            var client = new APIClient(url, user, password);
            Console.WriteLine("Enter TransactionID:");
            var transactionID = "1c44629a-5aa1-48f4-bc65-99d710412256";

            var orderType = OrderType.EmailMessage;

            var report = client.GetTransactionReport(transactionID, orderType, ReportReturnType.XML);

            if (report.RequestResult == RequestResultType.Error)
            {
                Console.WriteLine("Error: " + report.RequestErrorMessage);
            }
            else
            {
                Console.WriteLine("Report: " + report.ReportData);
            }
        }
    }
}

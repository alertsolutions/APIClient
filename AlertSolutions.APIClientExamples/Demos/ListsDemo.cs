using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertSolutions.API;

namespace AlertSolutions.APIClientExamples.Demos
{
    public static class ListsDemo
    {
        public static void Execute()
        {
            // change these values to your API login
            var user = "jthomas@blimessaging.com";
            var password = "pentagon625";
            var url = "http://weblaunch.blifax.com/postapi";

            var client = new APIClient(url, user, password);
            var response = client.GetLists(ReportReturnType.XML);
            Console.WriteLine(response);

            response = client.GetLists(ReportReturnType.CSV);
            Console.WriteLine("\n\n"+ response);
        }
    }
}

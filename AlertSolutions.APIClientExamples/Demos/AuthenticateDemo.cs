using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertSolutions.API;

namespace AlertSolutions.APIClientExamples.Demos
{
    public static class AuthenticateDemo
    {
        public static void Execute()
        {
            // change these values to your API login
            var user = "";
            var password = "";
            var url = "http://weblaunch.blifax.com/postapi";

            var client = new APIClient(url, user, password);

            if (client.Authenticated())
            {
                Console.WriteLine("The credentials provided to the client have been authenticated!");
            }
            else
            {
                Console.WriteLine("The credentials provided to the client were not authenticated.");   
            }
        }
    }
}

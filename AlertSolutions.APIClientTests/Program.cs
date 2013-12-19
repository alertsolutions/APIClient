using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlertSolutions.API;
using AlertSolutions.API.Orders;

namespace AlertSolutions.APIClientExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestApp.Execute();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}

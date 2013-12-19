using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.APIClientExamples
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Demos.SimpleDemo.Execute();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}

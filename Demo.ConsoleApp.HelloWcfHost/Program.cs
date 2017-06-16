using System;

using Demo.Services.Core;
using Demo.Services.HelloWcf;

namespace Demo.ConsoleApp.HelloWcfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var host = new CustomServiceHost<HelloWcf>();
                host.Open();

                Console.WriteLine("Service is hosted at " + DateTime.Now.ToString());
                Console.WriteLine("Host is running... Press any key to stop.");
                Console.ReadLine();
                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
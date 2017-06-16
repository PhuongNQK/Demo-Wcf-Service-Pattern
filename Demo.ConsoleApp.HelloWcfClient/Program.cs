using System;

using Interfaces.Demo.Services.HelloWcf.DataContracts;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;
using Interfaces.Demo.Services.HelloWcf.Proxies;

namespace Demo.ConsoleApp.HelloWcfClient
{
    class Program
    {
        private const string ClientEndpointName = "NetTcpBinding_IHelloWcf";

        static void Main(string[] args)
        {
            try
            {
                RunOneTestCall();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: {0}\n{1}", ex.Message, ex.StackTrace);
            }

            Console.ReadLine();
        }

        private static void RunOneTestCall()
        {
            Console.WriteLine("Run 1 test call...");
            var person = new Person()
            {
                Name = nameof(RunOneTestCall),
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };
            string message;
            using (var client = GetNewStyledHelloWcfClient())
            {
                message = client.SayHello.Operate(request).Message;
            }

            Console.WriteLine("Response: {0}", message);
        }

        private static NewStyledHelloWcfClient GetNewStyledHelloWcfClient()
        {
            return new NewStyledHelloWcfClient(ClientEndpointName);
        }
    }
}
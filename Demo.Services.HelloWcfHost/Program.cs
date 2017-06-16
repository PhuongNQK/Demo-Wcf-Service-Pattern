using System.ServiceProcess;

namespace Demo.Services.HelloWcfHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new HelloWcfHostService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
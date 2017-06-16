using System.ServiceProcess;

namespace Demo.Services.Core
{
    /// <summary>
    /// All services in your system should share certain properties and behaviors
    /// which are not already available in <see cref="ServiceBase"/>. That is why 
    /// this class is needed. It represents a service in your system with all shared
    /// properties and behaviors.
    /// </summary>
    public class CustomServiceBase: ServiceBase
    {
        // TODO Add needed properties and behaviors 
    }
}
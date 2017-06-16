using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Demo.Services.Core
{
    /// <summary>
    /// <para>All service hosts in your system should share certain properties and behaviors
    /// which are not already available in <see cref="ServiceHost"/>. That is why 
    /// this class is needed. It represents a service host in your system with all shared
    /// properties and behaviors.</para>
    /// <para>In this demo, I make it support these:</para>
    /// <list>    
    /// <item>A TCP-based endpoint that provides its clients with service metadata.</item>
    /// <item>Metadata can be retrieved via HTTP/GET requests.</item>
    /// </list>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomServiceHost<T> : ServiceHost
    {
        private static readonly Type IMetadataExchangeType = typeof(IMetadataExchange);

        public CustomServiceHost(params Uri[] baseAddresses)
            : base(typeof(T), baseAddresses)
        {
            const string MetadataEndpointAddress = "MEX";

            var metadataBehavior = Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                Description.Behaviors.Add(metadataBehavior);
            }

            var binding = MetadataExchangeBindings.CreateMexTcpBinding();
            AddServiceEndpoint(IMetadataExchangeType, binding, MetadataEndpointAddress);
        }
    }
}
using System.ServiceModel;

using Interfaces.Demo.Services.HelloWcf.Constants;
using Interfaces.Demo.Services.HelloWcf.DataContracts;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;

namespace Interfaces.Demo.Services.HelloWcf
{
    /// <summary>
    /// The methods here were designed for these purposes:
    /// - SayHello(): demonstrates that this pattern is possible    
    /// - SayHelloGeneric(): demonstrates that this pattern is possible with generics 
    /// and inheritance
    /// - SayHelloGeneric(), SayHelloGeneric2(): demonstrates that this pattern still 
    /// works when the same generics application is used in several places.
    /// - SayHelloGeneric(), SayHelloGeneric3(): demonstrates that this pattern still
    /// works when different generics applications are used in the same service.
    /// 
    /// Note: 
    /// - All Request/Response classes must have a parameterless constructor and 
    /// all properties should be get/set-able. Otherwise, serialization exceptions 
    /// may be thrown.
    /// </summary>    
    [ServiceContract(
        Name = Values.Service.InterfaceName, 
        ConfigurationName = Values.Service.ConfigurationName, 
        Namespace = Values.Service.Namespace
     )]
    public interface IHelloWcf
    {
        [OperationContract]
        SayHelloResponse SayHello(SayHelloRequest request);
        
        [OperationContract]
        GenericResponse<Person> SayHelloGeneric(GenericRequest<Person> request);

        [OperationContract]
        GenericResponse<Person> SayHelloGeneric2(GenericRequest<Person> request);

        [OperationContract]
        GenericResponse<string> SayHelloGeneric3(GenericRequest<int> request);
    }
}
using Demo.Services.Core;
using Interfaces.Demo.Services.HelloWcf.DataContracts;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;

namespace Interfaces.Demo.Services.HelloWcf.Proxies
{
    /// <summary>
    /// Using this style of interface definition, we only need 1 member per each service action.
    /// </summary>
    public interface INewStyledHelloWcfClient
    {
        ServiceOperation<SayHelloRequest, SayHelloResponse> SayHello { get; }
        
        ServiceOperation<GenericRequest<Person>, GenericResponse<Person>> SayHelloGeneric { get; }

        ServiceOperation<GenericRequest<Person>, GenericResponse<Person>> SayHelloGeneric2 { get; }

        ServiceOperation<GenericRequest<int>, GenericResponse<string>> SayHelloGeneric3 { get; }
    }
}
using System;
using System.ServiceModel;
using System.Threading.Tasks;

using Demo.Services.Core;
using Interfaces.Demo.Services.HelloWcf.Constants;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;

namespace Interfaces.Demo.Services.HelloWcf.Proxies
{
    /// <summary>
    /// Using this style of interface definition, we need up to 6 members for a 
    /// single service action:
    /// - 1 member that allows the action to be synchronously executed
    /// - 2 members that allow the action to be asynchronously executed
    /// using Begin/End style
    /// - 2 members that allow the action to be asynchronously executed
    /// using Async/Completed style
    /// - 1 member that allows the action to be asynchronously executed
    /// using Task style
    /// 
    /// Also, this interface needs attributing with <see cref="ServiceContractAttribute"/>
    /// and some of its actions need attributing with <see cref="OperationContractAttribute"/>.
    /// </summary>
    [ServiceContract(
        Name = Values.Service.InterfaceName,
        ConfigurationName = Values.Client.ConfigurationName,
        Namespace = Values.Service.Namespace
     )]
    public interface IOldStyledHelloWcfClient // Note that our client interface can extend the original service interface, but here I want to focus on SayHello and ignore SayHelloGeneric
    {
        [OperationContract] // Synchronous XXX must be marked with OperationContract like this
        SayHelloResponse SayHello(SayHelloRequest request);

        [OperationContract(AsyncPattern = true)] // BeginXXX must be marked with OperationContract like this
        IAsyncResult BeginSayHello(SayHelloRequest request, AsyncCallback callback, object asyncState);

        SayHelloResponse EndSayHello(IAsyncResult result);

        event EventHandler<CustomAsyncCompletedEventArgs<SayHelloResponse>> SayHelloCompleted;
                
        void SayHelloAsync(SayHelloRequest request, object userState = null);

        Task<SayHelloResponse> SayHelloAsTask(SayHelloRequest request);
    }
}
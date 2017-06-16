using System;
using System.Threading;
using System.Threading.Tasks;

using Demo.Services.Core;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;

namespace Interfaces.Demo.Services.HelloWcf.Proxies
{
    public class OldStyledHelloWcfClient 
        : OldStyledClientBase<IOldStyledHelloWcfClient>, IOldStyledHelloWcfClient // Note: our client must inherit like this: OldStyledClientBase<TInterface>, TInterface
    {
        public OldStyledHelloWcfClient() : base() { }

        public OldStyledHelloWcfClient(string endpointConfigName) : base(endpointConfigName) { }



        #region Synchronous-styled

        public SayHelloResponse SayHello(SayHelloRequest request)
        {
            return Channel.SayHello(request);
        }

        #endregion



        #region Begin/End-styled

        public IAsyncResult BeginSayHello(SayHelloRequest request, AsyncCallback callback, object asyncState)
        {
            return Channel.BeginSayHello(request, callback, asyncState);
        }

        public SayHelloResponse EndSayHello(IAsyncResult result)
        {
            return Channel.EndSayHello(result);
        }

        #endregion



        #region Async/Completed-styled

        private BeginOperationDelegate m_OnBeginSayHelloDelegate;
        private EndOperationDelegate m_OnEndSayHelloDelegate;
        private SendOrPostCallback m_OnSayHelloCompletedDelegate;

        public event EventHandler<CustomAsyncCompletedEventArgs<SayHelloResponse>> SayHelloCompleted;

        public void SayHelloAsync(SayHelloRequest request, object userState = null)
        {
            DoAsync(
                request, userState
                , ref m_OnBeginSayHelloDelegate, BeginSayHello
                , ref m_OnEndSayHelloDelegate, EndSayHello
                , ref m_OnSayHelloCompletedDelegate, SayHelloCompleted
            );
        }

        #endregion



        #region Task-styled

        public Task<SayHelloResponse> SayHelloAsTask(SayHelloRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, state) => Channel.BeginSayHello(request, callback, state),
                result => Channel.EndSayHello(result),
                null
            );
        }

        #endregion
    }
}
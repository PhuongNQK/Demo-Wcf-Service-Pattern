using System;
using System.ServiceModel;
using System.Threading;

using Demo.Services.Core;

namespace Interfaces.Demo.Services.HelloWcf.Proxies
{
    public class OldStyledClientBase<TInterface> : ClientBase<TInterface>
        where TInterface : class
    {
        public OldStyledClientBase() : base() { }

        public OldStyledClientBase(string endpointConfigName) : base(endpointConfigName) { }



        #region Common

        protected void DoAsync<TRequest, TResponse>(TRequest request, object userState
            , ref BeginOperationDelegate beginDelegate, Func<TRequest, AsyncCallback, object, IAsyncResult> beginDo
            , ref EndOperationDelegate endDelegate, Func<IAsyncResult, TResponse> endDo
            , ref SendOrPostCallback completedDelegate, EventHandler<CustomAsyncCompletedEventArgs<TResponse>> doCompleted)
        {
            if (beginDelegate == null)
            {
                BeginOperationDelegate defaultBeginDelegate = (inValues, callback, asyncState) =>
                {
                    var req = (TRequest)(inValues[0]);
                    return beginDo(req, callback, asyncState);
                };
                beginDelegate = new BeginOperationDelegate(defaultBeginDelegate);
            }

            if (endDelegate == null)
            {
                EndOperationDelegate defaultEndDelegate = (result) =>
                {
                    TResponse retVal = endDo(result);
                    return new object[] { retVal };
                };
                endDelegate = new EndOperationDelegate(defaultEndDelegate);
            }

            if (completedDelegate == null)
            {
                SendOrPostCallback defaultCompletedDelegate = (state) =>
                {
                    if (doCompleted != null)
                    {
                        var originalArgs = (InvokeAsyncCompletedEventArgs)state;
                        var newArgs = new CustomAsyncCompletedEventArgs<TResponse>(
                            originalArgs.Results == null ? default(TResponse) : (TResponse)originalArgs.Results[0],
                            originalArgs.Error, originalArgs.Cancelled, originalArgs.UserState
                        );
                        doCompleted(this, newArgs);
                    }
                };
                completedDelegate = new SendOrPostCallback(defaultCompletedDelegate);
            }

            InvokeAsync(beginDelegate, new object[] { request }, endDelegate, completedDelegate, userState);
        }

        #endregion
    }
}
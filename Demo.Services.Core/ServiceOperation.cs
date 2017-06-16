using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Services.Core
{
    /// <summary>
    /// This class is a key part in implementing the pattern the solution 
    /// tries to demonstrate.
    /// It represents a generic service operation that is built based on 
    /// some synchronous operation and that can be executed synchronously 
    /// and/or asynchronously.
    /// For demonstration purposes, this implementation includes support 
    /// for 3 common asynchronous patterns (see: https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/).
    /// But when you apply this class to your code, you can remove support
    /// for the asynchronous patterns you do not need.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ServiceOperation<TRequest, TResponse>
    {
        private Func<TRequest, TResponse> m_OperateDelegate;

        /// <summary>
        /// Create a new sevice operation based on a synchronous operation.
        /// </summary>
        /// <param name="operateDelegate">The synchronous operation</param>
        public ServiceOperation(Func<TRequest, TResponse> operateDelegate)
        {
            m_OperateDelegate = operateDelegate;
        }


        /*
         * - Here follow 4 regions, each contains a wrapper that allows the 
         * original synchronous operation to be executed in a specific way.
         * - When you apply this class to your production code, you can keep
         * only the needed regions or simply add your own. 
         */


        #region Synchronous-styled wrapper

        public TResponse Operate(TRequest request)
        {
            return m_OperateDelegate(request);
        }

        #endregion



        #region Begin/End-styled wrapper

        public IAsyncResult BeginOperate(TRequest request, AsyncCallback callback, object userState)
        {
            return m_OperateDelegate.BeginInvoke(request, callback, userState);
        }

        public TResponse EndOperate(IAsyncResult result)
        {
            return m_OperateDelegate.EndInvoke(result);
        }

        #endregion



        #region Async/Completed-styled wrapper

        private delegate void WorkerEventHandler(TRequest request, AsyncOperation operation);

        private readonly Dictionary<object, AsyncOperation> m_Operations = new Dictionary<object, AsyncOperation>();
        private SendOrPostCallback m_OnOperationCompletedDelegate;

        public event EventHandler<CustomAsyncCompletedEventArgs<TResponse>> OperationCompleted;

        public void OperateAsync(TRequest request)
        {
            OperateAsync(request, null);
        }

        public void OperateAsync(TRequest request, object userState)
        {
            if (userState == null)
            {
                userState = string.Empty;
            }

            if ((m_OnOperationCompletedDelegate == null))
            {
                m_OnOperationCompletedDelegate = new SendOrPostCallback(OnOperationCompleted);
            }

            var operation = AsyncOperationManager.CreateOperation(userState);

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (m_Operations)
            {
                if (m_Operations.ContainsKey(userState))
                {
                    throw new ArgumentException("User state parameter must be unique", nameof(userState));
                }

                m_Operations[userState] = operation;
            }

            // Start the asynchronous operation
            var workerDelegate = new WorkerEventHandler(PerformOperation);
            workerDelegate.BeginInvoke(request, operation, null, null);
        }

        /// <summary>
        /// This method cancels a pending asynchronous operation.
        /// </summary>
        /// <param name="userState"></param>
        public void CancelAsync(object userState)
        {
            lock (m_Operations)
            {
                if (m_Operations.ContainsKey(userState))
                {
                    m_Operations.Remove(userState);
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return m_Operations.Count > 0;
            }
        }

        private void OnOperationCompleted(object state)
        {
            if ((OperationCompleted != null))
            {
                var eventArgs = (CustomAsyncCompletedEventArgs<TResponse>)state;
                OperationCompleted(this, eventArgs);
            }
        }

        private void PerformOperation(TRequest request, AsyncOperation operation)
        {
            Exception exception = null;
            TResponse response = default(TResponse);

            // Check that the task is still active.
            // The operation may have been canceled before the thread was scheduled.
            bool cancelled = IsOperationCancelled(operation.UserSuppliedState);
            if (!cancelled)
            {
                try
                {
                    response = Operate(request);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            FinishOperation(response, exception, cancelled, operation);
        }

        private bool IsOperationCancelled(object userState)
        {
            return (m_Operations[userState] == null);
        }

        /// <summary>
        /// This is the method that the underlying, free-threaded 
        /// asynchronous behavior will invoke.  This will happen on
        /// an arbitrary thread.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="exception"></param>
        /// <param name="cancelled"></param>
        /// <param name="operation"></param>
        private void FinishOperation(TResponse response, 
            Exception exception, bool cancelled, AsyncOperation operation)
        {
            // If the task was not previously cancelled,
            // remove the task from the operation collection.
            if (!cancelled)
            {
                lock (m_Operations)
                {
                    m_Operations.Remove(operation.UserSuppliedState);
                }
            }

            // Package the results of the operation in a CustomCompletedEventArgs 
            // and end the task
            var eventArgs = new CustomAsyncCompletedEventArgs<TResponse>(
                response, exception, cancelled, operation.UserSuppliedState
            );
            operation.PostOperationCompleted(m_OnOperationCompletedDelegate, eventArgs);

            // Note that after the call to OperationCompleted, 
            // operation is no longer usable, and any attempt to use it
            // will cause an exception to be thrown.
        }

        #endregion



        #region Task-styled wrapper

        public Task<TResponse> OperateAsTask(TRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, state) => m_OperateDelegate.BeginInvoke(request, callback, state),
                result => m_OperateDelegate.EndInvoke(result),
                null
            );
        }

        #endregion
    }
}
using System;
using System.ComponentModel;

namespace Demo.Services.Core
{
    /// <summary>
    /// This class adds support for generics to the existing class <see cref="AsyncCompletedEventArgs"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public partial class CustomAsyncCompletedEventArgs<TResult> : AsyncCompletedEventArgs
    {
        private TResult m_Result;

        public CustomAsyncCompletedEventArgs(object userState)
            : base(null, true, userState)
        {
            m_Result = default(TResult);
        }

        public CustomAsyncCompletedEventArgs(TResult result, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            m_Result = result;
        }

        public TResult Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return m_Result;
            }
        }
    }
}
using System;
using System.Windows;
using System.Windows.Threading;

namespace DemiCode.Mvvm.Wpf
{
    ///<summary>
    /// Default implementation of <see cref="IDispatcherService"/> using the current <see cref="Application.Dispatcher"/>.
    ///</summary>
    public class ApplicationDispatcherService : DispatcherServiceBase
    {
        public ApplicationDispatcherService() : base(GetDispatcher)
        {
        }

        private static Dispatcher GetDispatcher()
        {
            if (Application.Current == null)
                return null;

            try
            {
                return Application.Current.Dispatcher;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
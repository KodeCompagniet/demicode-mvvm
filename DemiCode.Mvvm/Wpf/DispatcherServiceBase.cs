using System;
using System.Windows.Threading;

namespace DemiCode.Mvvm.Wpf
{
    ///<summary>
    /// Base class for Dispatcher services.
    ///</summary>
    public abstract class DispatcherServiceBase : IDispatcherService
    {
        private readonly Func<Dispatcher> _dispatcherFactory;

        protected DispatcherServiceBase(Func<Dispatcher> dispatcherFactory)
        {
            _dispatcherFactory = dispatcherFactory;
        }

        public void Execute(Action action)
        {
            var dispatcher = _dispatcherFactory();

            if (dispatcher == null || dispatcher.HasShutdownStarted || dispatcher.HasShutdownFinished)
                return;

            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(DispatcherPriority.Normal, action);
            }
        }
    }
}
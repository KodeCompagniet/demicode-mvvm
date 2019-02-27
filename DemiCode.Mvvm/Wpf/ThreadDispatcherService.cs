using System;
using System.Threading;
using System.Windows.Threading;

namespace DemiCode.Mvvm.Wpf
{
    public class ThreadDispatcherService : DispatcherServiceBase
    {
        public ThreadDispatcherService() : this(Thread.CurrentThread)
        {
        }

        public ThreadDispatcherService(Thread thread) : base(() => Dispatcher.FromThread(thread))
        {
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Threading;

namespace DemiCode.Mvvm.Wpf
{
    public static class ExceptionHelper
    {
        public static void ThrowApplicationException(this Exception exception)
        {
            var message = String.Format(CultureInfo.InvariantCulture,
                "An exception of type {0} occurred in the application. Check the inner exception for details.", exception.GetType().Name);

            Action throwException = () => { throw new ApplicationException(message, exception); };

            Dispatcher.CurrentDispatcher.BeginInvoke(throwException);
        }
    }
}

using System.Threading;

namespace DemiCode.Mvvm.Test.UserControlViewTests.Views
{
    public class HostedUserControlViewModel : ViewModelBase
    {
        /// <summary>
        /// The number of times the constructor of this class whas called.
        /// </summary>
        public static int NumberOfConstructs;

        public HostedUserControlViewModel()
        {
            Interlocked.Increment(ref NumberOfConstructs);
        }
    }
}
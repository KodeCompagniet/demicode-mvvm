using System.Threading;
using System.Windows;
using System.Windows.Threading;
using NUnit.Framework;

namespace DemiCode.Mvvm.Wpf.Test
{


    [TestFixture]
    [Ignore("Currently hard to test")]
    public class ThreadDispatcherServiceTest
    {
        // ReSharper disable InconsistentNaming

        [Test]
        public void Execute()
        {
            var t = new Thread(() =>
                                   {
                                       var disp = Dispatcher.CurrentDispatcher;
                                       while (true)
                                       {
                                           Dispatcher.Run();
                                           
                                       }
                                   });
            t.Start();

            try
            {
                var value = false;
                var d = new ThreadDispatcherService(t);

                d.Execute(() => value = true);

                Assert.That(value, Is.True);
            }
            finally
            {
                t.Abort();
            }
        }

        // ReSharper restore InconsistentNaming
    }


}

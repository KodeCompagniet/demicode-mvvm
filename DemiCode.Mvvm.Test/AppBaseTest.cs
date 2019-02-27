using System;
using DemiCode.Mvvm.Messaging;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Test
{

    [TestFixture]
    public class AppBaseTest
    {

         private class SomeApp : AppBase
         {
             public IMessenger Messenger { get; set; }

             protected override void OnActivated(EventArgs e)
             {
                 Shutdown();
             }
         }

        [Test]
        [STAThread]
        [Explicit("This test starts a WPF app. In some occations the test process will not release the app properly, requiring a manual kill of the test runner process")]
        public void App_WhenStarted_InjectsPropertyServices()
        {
            var app = new SomeApp();

            app.Run();

            Assert.That(app.Messenger, Is.Not.Null);
        }
    }

}

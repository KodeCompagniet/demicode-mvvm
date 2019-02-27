using System.Linq;
using System.Threading;
using System.Windows;
using Autofac;
using DemiCode.Mvvm.Autofac;
using DemiCode.Mvvm.Test.UserControlViewTests.Views;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.UserControlViewTests.Test
{
    [TestFixture]
    [Category("Integration")]
    public class UserControlViewTests_WhenHosted
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            builder.RegisterType<WindowWithUserControlViewModel>();
            builder.RegisterType<WindowWithNamedRootUserControlViewModel>();
            builder.RegisterType<HostedUserControlViewModel>();
            builder.RegisterType<HostedNamedRootUserControlViewModel>();
            _container = builder.Build();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct()
        {
            var window = new WindowWithUserControlView();
            IocContextHelper.SetIocContext(window, _container);
            window.Loaded += (sender, args) => window.Close();

            var app = new Application();
            app.Run(window);

            try
            {
                Assert.That(HostedUserControlViewModel.NumberOfConstructs, Is.EqualTo(1));
            }
            finally
            {
                app.Shutdown();
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        [Ignore("Unable to run new Application due to construct test + unable to get binding to work when running in ")]
        public void Construct_WithNamedRootTypedView_ModelBindingDoesNotBreak()
        {
            Assert.Inconclusive("TODO: unable to run new Application due to construct test + unable to get binding to work when running in ");

            var window = new WindowWithNamedRootUserControlView();
            IocContextHelper.SetIocContext(window, _container);
            window.Loaded += (sender, args) => window.Close();

            var app = new Application
                          {
                              MainWindow = window, 
                              ShutdownMode = ShutdownMode.OnMainWindowClose
                          };
            app.Run(window);
            
            
            try
            {
                var vm = HostedNamedRootUserControlViewModel.Instances.First();
                Assert.That(vm.TheModel, Is.EqualTo("Hello World"));
            }
            finally
            {
                app.Shutdown();
            }
            
        }
    }
}

using System.Threading;
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
    public class UserControlViewTests_WhenAddedToContainer
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            builder.RegisterType<TestWindowViewModel>();
            builder.RegisterType<TestUserControlViewModel>();
            _container = builder.Build();
        }

        [Test, Apartment(ApartmentState.STA)]
        [Ignore("Hangs the testrunner (both TestDriven and ReSharper)")]
        public void Construct()
        {
            Assert.Inconclusive("TODO: Hangs the testrunner (both TestDriven and ReSharper)");
        //    var window = new TestWindowView();
        //    IocContextHelper.SetIocContext(window, _container);

        //    ViewModelBase theViewModel = null;
        //    window.Loaded += (sender, args) =>
        //                         {
        //                             try
        //                             {
        //                                 var view = new TestUserControlView {Width = 100, Height = 100};
        //                                 var ctrl = new ViewController(_container);
        //                                 ctrl.OpenView(view, "child data", window);

        //                                 view.DataContext = "new child data";
        //                                 theViewModel = view.ViewModel;
        //                             }
        //                             finally
        //                             {
        //                                 window.Close();
        //                             }
        //                         };


        //    var app = new Application();
        //    app.Run(window);

        //    try
        //    {
        //        Assert.That(theViewModel, Is.InstanceOf<TestUserControlViewModel>());
        //    }
        //    finally
        //    {
        //        app.Shutdown();
        //    }

        }

    }
}


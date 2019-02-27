using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DemiCode.Mvvm.Autofac;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.UserControlViewTests.Test
{
    [TestFixture]
    public class UserControlViewTests_WithoutMessenger
    {

        private UserControlView _view;
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterViewModels(typeof (UserControlViewTests).Assembly);
            _container = builder.Build();
            _view = MockRepository.GenerateStub<UserControlView>();
            IocContextHelper.SetIocContext(_view, _container);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_WhenMessengerDoesntExist_DoesNotThrow()
        {
            _view.ViewModelType = typeof (TestViewModel);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.That(_view.ViewModel, Is.Not.Null);
        }
    }
}

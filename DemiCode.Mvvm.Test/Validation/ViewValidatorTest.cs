using System.Threading;
using System.Windows;
using Autofac;
using DemiCode.Mvvm.Autofac;
using DemiCode.Mvvm.UserControlViewTests.Test;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Validation.Test
{

    [TestFixture]
    public class ViewValidatorTest
    {

        private UserControlView _view;
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            builder.RegisterViewModels(typeof(UserControlViewTests.Test.UserControlViewTests).Assembly);
            _container = builder.Build();
            _view = MockRepository.GenerateStub<UserControlView>();
            IocContextHelper.SetIocContext(_view, _container);
            _view.ViewModelType = typeof(TestViewModel);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_ViewModel_IsInitialized()
        {
            Assert.That(ViewValidator.IsValid(_view));
        }

    }

}

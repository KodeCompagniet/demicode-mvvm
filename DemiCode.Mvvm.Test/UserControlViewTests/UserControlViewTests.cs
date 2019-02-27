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
    public class UserControlViewTests
    {
        private UserControlView _view;
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            builder.RegisterViewModels(typeof(UserControlViewTests).Assembly);
            _container = builder.Build();
            _view = MockRepository.GenerateStub<UserControlView>();
            IocContextHelper.SetIocContext(_view, _container);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_ViewModel_IsInitialized()
        {
            _view.ViewModelType = typeof(TestViewModel);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.That(_view.ViewModel, Is.Not.Null);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_WithViewModel_ViewValidationIsInitialized()
        {
            _view.ViewModelType = typeof(TestViewModel);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.That(_view.ViewModel.ViewValidation, Is.Not.Null);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_TypedViewModel_IsInitializedWithModel()
        {
            _view.SetValue(FrameworkElement.DataContextProperty, "model");

            _view.ViewModelType = typeof(TestTypedViewModel<string>);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.That(((TestTypedViewModel<string>)_view.ViewModel).TheModel, Is.EqualTo("model"));
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_TypedViewModelWhenViewIsReinitialized_IsInitializedWithModel()
        {
            _view.SetValue(FrameworkElement.DataContextProperty, "model");

            _view.ViewModelType = typeof(TestTypedViewModel<string>);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.That(((TestTypedViewModel<string>)_view.ViewModel).TheModel, Is.EqualTo("model"));
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_TypedViewModel_IsDiscoveredByConvention()
        {
            var view = new ByConventionView();
            IocContextHelper.SetIocContext(view, _container);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.That(view.ViewModel, Is.InstanceOf<ByConventionViewModel>());
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Construct_WithNamedRootElement_ModelIsNotNull()
        {
            var view = new NamedRootView { DataContext = "some data" };
            IocContextHelper.SetIocContext(view, _container);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            var vm = (NamedRootViewModel)view.ViewModel;

            Assert.That(vm.TheModel, Is.EqualTo("some data"));
        }

        [Test, Apartment(ApartmentState.STA)]
        public void DataContextChanged_WithTypedViewModel_IsInitializedWithNewModel()
        {
            _view.SetValue(FrameworkElement.DataContextProperty, "model");
            _view.ViewModelType = typeof(TestTypedViewModel<string>);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            _view.SetValue(FrameworkElement.DataContextProperty, "newmodel");
            _view.ForceInitializeViewModel();

            Assert.That(((TestTypedViewModel<string>)_view.ViewModel).TheModel, Is.EqualTo("newmodel"));
        }

        [Test, Apartment(ApartmentState.STA)]
        public void DataContextChanged_WhenContainerIsLost_ReusesExistingViewModel()
        {
            _view.SetValue(FrameworkElement.DataContextProperty, "model");
            _view.ViewModelType = typeof(TestTypedViewModel<string>);
            _view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            IocContextHelper.SetIocContext(_view, null);
            var existingVm = _view.ViewModel;

            _view.ForceInitializeViewModel();
            var currentVm = _view.ViewModel;

            Assert.That(currentVm, Is.SameAs(existingVm) & Is.Not.Null);
        }

    }

    internal class ByConventionView : UserControlView
    {
        public ByConventionView()
        {
            ViewModelType = null;
        }
    }

    internal class NamedRootView : UserControlView
    {
        public NamedRootView()
        {
            ViewModelType = null;

            var grid = new Grid();
            grid.Name = "root";

            Content = grid;
        }
    }

    internal class NamedRootViewModel : TypedViewModelBase<string>
    {
        public string TheModel { get; set; }

        protected override void OnInitialize()
        {
            TheModel = Model;
        }
    }

    internal class ByConventionViewModel : ViewModelBase
    {
    }

    internal class TestViewModel : ViewModelBase
    {
    }

    internal class TestTypedViewModel<TCargo> : TypedViewModelBase<TCargo>
    {
        public static int ConstructCount { get; private set; }
        public TCargo TheModel { get { return Model; } }

        public TestTypedViewModel()
        {
            ConstructCount++;
        }
    }






}

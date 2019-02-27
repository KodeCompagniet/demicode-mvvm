using System.Linq;
using System.Threading;
using System.Windows;
using Autofac;
using DemiCode.Mvvm.Autofac;
using DemiCode.Mvvm.Messaging;
using DemiCode.Mvvm.Test.UserControlViewTests.Views;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.UserControlViewTests.Test
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [Category("Integration")]
    public class UserControlViewTests_WhenViewModelUsesMessenger1
    {
        private IContainer _container;
        private IMessenger _messenger;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            builder.RegisterType<MessagingViewModel>();
            builder.RegisterType<TypedMessagingViewModel>();
            _container = builder.Build();
            _messenger = _container.Resolve<IMessenger>();

        }

        [Test]
        public void Construct_ViewModel_HaveMessenger()
        {
            var view = MockRepository.GenerateStub<UserControlView>();
            IocContextHelper.SetIocContext(view, _container);

            view.ViewModelType = typeof (MessagingViewModel);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            var vm = (MessagingViewModel) view.ViewModel;

            Assert.That(vm.Messenger, Is.SameAs(_messenger));
        }

        [Test]
        [Explicit("Must be executed alone due to Application/AppDomain conflict")]
        public void ViewModel_WhenReinitializingUntypedViewModel_RegistrationDoesNotAccumulate()
        {
            var view = new TestUserControlView();
            IocContextHelper.SetIocContext(view, _container);

            view.ViewModelType = typeof (MessagingViewModel);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            view.DataContext = "";

            Assert.That(MessagingViewModel.RegistrationsCalled, Is.EqualTo(2));
            Assert.That(_messenger.Registrations<MessagingViewModel>().Count(), Is.EqualTo(1));
        }

        [Test]
        [Explicit("Must be executed alone due to Application/AppDomain conflict")]
        public void ViewModel_WhenReinitializingTypedViewModel_OldViewModelIsAutomaticallyUnregisteredBeforeReinitialization()
        {
            var view = new TestUserControlView();
            IocContextHelper.SetIocContext(view, _container);

            view.ViewModelType = typeof (TypedMessagingViewModel);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            view.DataContext = "";

            Assert.That(TypedMessagingViewModel.RegistrationsCalled, Is.EqualTo(2));
            Assert.That(_messenger.Registrations<TypedMessagingViewModel>().Count(), Is.EqualTo(1));
        }

        [Test]
        [Explicit("Must be executed alone due to Application/AppDomain conflict")]
        public void ViewModel_WhenUnloaded_ViewModelIsAutomaticallyUnregisteredFirst()
        {
            var view = new TestUserControlView();
            IocContextHelper.SetIocContext(view, _container);

            view.ViewModelType = typeof (MessagingViewModel);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            
            // Simulate that we're loosing access to IOC context when unloaded
            IocContextHelper.SetIocContext(view, null);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));

            Assert.That(MessagingViewModel.RegistrationsCalled, Is.EqualTo(1));
            Assert.That(_messenger.Registrations<MessagingViewModel>().Count(), Is.EqualTo(0));
        }


        internal class SomeMessage : MessageBase
        {
        }

        internal class MessagingViewModel : ViewModelBase
        {
            private readonly IMessenger _messenger;

            public static int RegistrationsCalled { get; private set; }

            public MessagingViewModel(IMessenger messenger)
            {
                _messenger = messenger;
            }

            public override void Initialize(object model)
            {
                _messenger.Register<SomeMessage>(this, Handle);
                RegistrationsCalled++;
            }

            public IMessenger Messenger
            {
                get { return _messenger; }
            }

            private void Handle(SomeMessage msg)
            { }
        }

        internal class TypedMessagingViewModel : TypedViewModelBase<string>
        {
            private readonly IMessenger _messenger;

            public static int RegistrationsCalled { get; private set; }

            public TypedMessagingViewModel(IMessenger messenger)
            {
                _messenger = messenger;
            }

            protected override void OnInitialize()
            {
                _messenger.Register<SomeMessage>(this, Handle);
                RegistrationsCalled++;
            }

            public IMessenger Messenger
            {
                get { return _messenger; }
            }

            private void Handle(SomeMessage msg)
            { }
        }

    }

}


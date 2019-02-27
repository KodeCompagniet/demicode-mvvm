using System;
using System.Windows;
using Autofac;
using DemiCode.Mvvm.Messaging;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming

namespace DemiCode.Mvvm.Test
{
    [TestFixture]
    public class ViewHelperTest
    {

        [Test]
        public void GetViewModelType_NoExplicitViewModelType()
        {
            var viewModelType =
                ViewHelper.GetViewModelType(typeof (ViewHelperView), null);

            Assert.That(viewModelType, Is.EqualTo(typeof(ViewHelperViewModel)));
        }

        [Test]
        public void GetViewModelType_NullType_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ViewHelper.GetViewModelType(null, null));
        }

        [Test]
        public void GetViewModelType_ExplicitViewModelType_YieldsTheExplicitViewModelType()
        {
            var viewModelType =
                ViewHelper.GetViewModelType(typeof (ViewHelperView), typeof(string));

            Assert.That(viewModelType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetViewModelType_WhenViewDoesNotHaveTheViewPostfix_ConventionStillResolvesToViewModel()
        {
            var viewModelType =
                ViewHelper.GetViewModelType(typeof(TestViewThatDoesNotHaveTheViewPostfix), null);

            Assert.That(viewModelType, Is.EqualTo(typeof(TestViewThatDoesNotHaveTheViewPostfixViewModel)));
        }

        [Test]
        public void InitializeComponent_InstanceWithoutInitializeComponentsMethod_Throws()
        {
            Assert.DoesNotThrow(() => ViewHelper.InitializeComponent(new TestUiElement()));
        }

        [Test]
        public void UnregisterFromMessenger_WhenScopeHaveMessenger_UnregistersRecipientFromMessenger()
        {
            var messenger = MockRepository.GenerateMock<IMessenger>();
            var cb = new ContainerBuilder();
            cb.RegisterInstance(messenger);
            var scope = cb.Build();
            var recipient = new object();
            
            ViewHelper.UnregisterFromMessenger(recipient, scope);

            messenger.AssertWasCalled(x => x.Unregister(Arg<object>.Is.Same(recipient)));
        }

        [Test]
        public void UnregisterFromMessenger_ComponentContext_UnregistersRecipientFromMessenger()
        {
            var messenger = MockRepository.GenerateMock<IMessenger>();
            var cb = new ContainerBuilder();
            cb.RegisterInstance(messenger);
            var scope = (IComponentContext) cb.Build();
            var recipient = new object();
            
            ViewHelper.UnregisterFromMessenger(recipient, scope);

            messenger.AssertWasCalled(x => x.Unregister(Arg<object>.Is.Same(recipient)));
        }

        [Test]
        public void UnregisterFromMessenger_WhenScopeHaveNoMessenger_DoesNotThrow()
        {
            var scope = new ContainerBuilder().Build();
            var recipient = new object();

            Assert.DoesNotThrow(() => ViewHelper.UnregisterFromMessenger(recipient, scope));
        }

        [Test]
        public void UnregisterFromMessenger_WhenRecipientIsNull_DoesNothing()
        {
            var messenger = MockRepository.GenerateMock<IMessenger>();
            var cb = new ContainerBuilder();
            cb.RegisterInstance(messenger);
            var scope = cb.Build();

            ViewHelper.UnregisterFromMessenger(null, scope);

            messenger.AssertWasNotCalled(x => x.Unregister(Arg<object>.Is.Anything));
        }

        [Test]
        public void UnregisterFromMessenger_WhenLifetimeScopeIsNull_DoesNothing()
        {
            Assert.DoesNotThrow(() => ViewHelper.UnregisterFromMessenger(new object(), (ILifetimeScope)null));
        }

        [Test]
        public void UnregisterFromMessenger_WhenComponentContextIsNull_DoesNothing()
        {
            Assert.DoesNotThrow(() => ViewHelper.UnregisterFromMessenger(new object(), (IComponentContext)null));
        }

        [Test]
        public void UnregisterFromMessenger_WithMessenger()
        {
            var messenger = MockRepository.GenerateMock<IMessenger>();
            var recipient = new object();

            ViewHelper.UnregisterFromMessenger(recipient, messenger);

            messenger.AssertWasCalled(x => x.Unregister(Arg<object>.Is.Same(recipient)));
        }

        [Test]
        public void UnregisterFromMessenger_WithMessengerAndNullRecipient_DoesNothing()
        {
            var messenger = MockRepository.GenerateMock<IMessenger>();

            ViewHelper.UnregisterFromMessenger(null, messenger);

            messenger.AssertWasNotCalled(x => x.Unregister(Arg<object>.Is.Anything));
        }

        [Test]
        public void UnregisterFromMessenger_WithNullMessenger_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => ViewHelper.UnregisterFromMessenger(new object(), (IMessenger)null));
        }

        [Test]
        public void IsTypedViewModel_WithTypedViewModelType_IsTrue()
        {
            var type = typeof (TypedViewModel);

            Assert.That(ViewHelper.IsTypedViewModel(type), Is.True);
        }

        [Test]
        public void IsTypedViewModel_WithUntypedViewModelType_IsFalse()
        {
            var type = typeof (UntypedViewModel);

            Assert.That(ViewHelper.IsTypedViewModel(type), Is.False);
        }

        [Test]
        public void IsTypedViewModel_WithNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ViewHelper.IsTypedViewModel(null));
        }

        public class TestUiElement : UIElement {}
        public class TestViewThatDoesNotHaveTheViewPostfix{}
        public class TestViewThatDoesNotHaveTheViewPostfixViewModel{}
        public class ViewHelperView{}
        public class ViewHelperViewModel{}
        public class TypedViewModel : TypedViewModelBase<int> {}
        public class UntypedViewModel : ViewModelBase {}
    }
}

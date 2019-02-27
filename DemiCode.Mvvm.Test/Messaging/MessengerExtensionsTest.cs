using System;
using System.Linq;
using DemiCode.Mvvm.Messaging;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Test.Messaging
{

    [TestFixture]
    public class MessengerExtensionsTest
    {
        [Test]
        public void Registrations_WithExternalMessangerImplementation_Throws()
        {
            var messenger = new CustomMessenger();

            Assert.Throws<ArgumentException>(() => messenger.Registrations<object>());
        }

        [Test]
        public void Registrations()
        {
            var messenger = new Messenger();
            messenger.Register<object>(this, SomeMessageHandler);

            var registrations = messenger.Registrations<MessengerExtensionsTest>();

            Assert.That(registrations.Count(), Is.EqualTo(1));
        }

        private void SomeMessageHandler(object message) { }

        private class CustomMessenger : IMessenger
        {
            public void Register<TMessage>(object recipient, Action<TMessage> action)
            {
                throw new NotImplementedException();
            }

            public void Register<TMessage>(object recipient, object token, Action<TMessage> action)
            {
                throw new NotImplementedException();
            }

            public void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
            {
                throw new NotImplementedException();
            }

            public void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
            {
                throw new NotImplementedException();
            }

            public void Send<TMessage>(TMessage message)
            {
                throw new NotImplementedException();
            }

            public void Send<TMessage, TTarget>(TMessage message)
            {
                throw new NotImplementedException();
            }

            public void Send<TMessage>(TMessage message, object token)
            {
                throw new NotImplementedException();
            }

            public void Unregister(object recipient)
            {
                throw new NotImplementedException();
            }

            public void Unregister<TMessage>(object recipient)
            {
                throw new NotImplementedException();
            }

            public void Unregister<TMessage>(object recipient, object token)
            {
                throw new NotImplementedException();
            }

            public void Unregister<TMessage>(object recipient, Action<TMessage> action)
            {
                throw new NotImplementedException();
            }

            public void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
            {
                throw new NotImplementedException();
            }

            public void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action, Predicate<TMessage> filter)
            {
                throw new NotImplementedException();
            }

            public void Register<TMessage>(object recipient, object token, Action<TMessage> action, Predicate<TMessage> filter)
            {
                throw new NotImplementedException();
            }

            public void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action, Predicate<TMessage> filter)
            {
                throw new NotImplementedException();
            }

            public void Register<TMessage>(object recipient, Action<TMessage> action, Predicate<TMessage> filter)
            {
                throw new NotImplementedException();
            }
        }
    }

}

using System;
using System.Collections.Generic;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Messaging.Test
{

    [TestFixture]
    public class MessengerTest
    {
        private Messenger _messenger;

        [SetUp]
        public void SetUp()
        {
            _messenger = new Messenger();
        }

        [Test]
        public void Register_WithSameActionMultipleRegistrations_OnlyOneInstanceIsRegistered()
        {
            var msgReceived = 0;
            Action<SomeMessage> action = msg => msgReceived++;

            _messenger.Register(this, action);
            _messenger.Register(this, action);
            _messenger.Register(this, action);

            _messenger.Send(new SomeMessage(this));

            Assert.That(msgReceived, Is.EqualTo(1));
        }

        [Test]
        public void Register_WithSameActionDifferentTokens_OneInstancePerTokenIsRegistered()
        {
            var msgReceived = 0;
            Action<SomeMessage> action = msg => msgReceived++;

            _messenger.Register(this, "1", action);
            _messenger.Register(this, "2", action);
            _messenger.Register(this, "3", action);

            _messenger.Send(new SomeMessage(this), "1");
            _messenger.Send(new SomeMessage(this), "2");

            Assert.That(msgReceived, Is.EqualTo(2));
        }

        [Test]
        public void Register_WithSameActionAndFilters()
        {
            var msgReceived = 0;
            Action<FilteredMessage> action = msg => msgReceived++;
            Predicate<FilteredMessage> filter = msg => true;

            _messenger.Register(this, action, filter);
            _messenger.Register(this, action, filter);
            _messenger.Register(this, action, filter);

            _messenger.Send(new FilteredMessage{Id = 1});
            _messenger.Send(new FilteredMessage{Id = 2});

            Assert.That(msgReceived, Is.EqualTo(2));
        }

        [Test]
        public void Register_WithSameActionDifferentFilters()
        {
            var msgReceived = 0;
            Action<FilteredMessage> action = msg => msgReceived++;

            _messenger.Register(this, action, msg => msg.Id == 1);
            _messenger.Register(this, action, msg => msg.Id == 2);
            _messenger.Register(this, action, msg => msg.Id == 3);

            _messenger.Send(new FilteredMessage{Id = 1});
            _messenger.Send(new FilteredMessage{Id = 2});

            Assert.That(msgReceived, Is.EqualTo(2));
        }

        [Test]
        public void Unregister_WithEqualAction_UnregistersHandler()
        {
            var msgReceived = false;
            Action<SomeMessage> action = msg => msgReceived = true;
            _messenger.Register(this, action);
            
            _messenger.Unregister(this, action);
            _messenger.Send(new SomeMessage(this));

            Assert.That(msgReceived, Is.False);
        }

        [Test]
        public void Unregister_WithEqualActionAndFilter_UnregistersHandler()
        {
            var wasDelivered = false;
            Action<SomeMessage> action = msg => wasDelivered = true;
            Predicate<SomeMessage> filter = msg => true;
            _messenger.Register(this, action, filter);

            _messenger.Unregister(this, action, filter);
            _messenger.Send(new SomeMessage(this));

            Assert.That(wasDelivered, Is.False);
        }

        [Test]
        public void Unregister_WithEqualActionAndDifferentFilter_DoesNotUnregisterHandler()
        {
            var msgReceived = false;
            Action<SomeMessage> action = msg => msgReceived = true;
            Predicate<SomeMessage> filter = msg => true;
            _messenger.Register(this, action);

            _messenger.Unregister(this, action, filter);
            _messenger.Send(new SomeMessage(this));

            Assert.That(msgReceived, Is.True);
        }

        [Test]
        public void Register_WithUnfilteredAndFilteredHandlers_MessageIsDeliveredCorrectly()
        {
            var nonFilteredDelivered = new List<int>();
            var filteredId = new List<int>();
            _messenger.Register<FilteredMessage>(this, msg => nonFilteredDelivered.Add(msg.Id));
            _messenger.Register<FilteredMessage>(this, msg => filteredId.Add(msg.Id), msg => msg.Id == 42);

            _messenger.Send(new FilteredMessage { Id = 42 });
            _messenger.Send(new FilteredMessage { Id = 32 });

            Assert.That(nonFilteredDelivered.Count, Is.EqualTo(2));
            Assert.That(nonFilteredDelivered.Contains(32));
            Assert.That(nonFilteredDelivered.Contains(42));
            Assert.That(filteredId.Count, Is.EqualTo(1));
            Assert.That(filteredId.Contains(42));
        }

        [Test]
        public void Unregister_WithObjectAndRegisteredAction()
        {
            var wasDelivered = false;
            _messenger.Register<SomeMessage>(this, msg => wasDelivered = true);

            _messenger.Unregister(this);
            _messenger.Send(new SomeMessage(this));

            Assert.That(wasDelivered, Is.False);
        }

        [Test]
        public void Unregister_WithObjectAndRegisteredActionAndFilter()
        {
            var wasDelivered = false;
            _messenger.Register<FilteredMessage>(this, msg => wasDelivered = true, msg => true);

            _messenger.Unregister(this);
            _messenger.Send(new FilteredMessage());

            Assert.That(wasDelivered, Is.False);
        }

        [Test]
        public void Send_WithSelfAsSender_MessageIsReceivedToSelf()
        {
            var msgReceived = false;
            _messenger.Register<SomeMessage>(this, msg => msgReceived = true);

            _messenger.Send(new SomeMessage(this));

            Assert.That(msgReceived, Is.True);
        }

        [Test]
        public void Send_WithDifferentActionsAndEqualFilters_DeliveresToBothActions()
        {
            var wasDelivered1 = false;
            var wasDelivered2 = false;
            Predicate<FilteredMessage> filter = msg => msg.Id == 42;
            _messenger.Register(this, msg => wasDelivered1 = true, filter);
            _messenger.Register(this, msg => wasDelivered2 = true, filter);

            _messenger.Send(new FilteredMessage { Id = 42 });

            Assert.That(wasDelivered1, Is.True);
            Assert.That(wasDelivered2, Is.True);
        }

        [Test]
        public void Send_WhenMessageFilterYieldsFalse_MessageIsNotDelivered()
        {
            var wasDelivered = false;
            _messenger.Register<FilteredMessage>(this, msg => wasDelivered = true, msg => false);

            _messenger.Send(new FilteredMessage { Id = 42 });

            Assert.That(wasDelivered, Is.False);
        }

        [Test]
        public void Send_WithMessageFilter()
        {
            var wasFiltered = false;
            _messenger.Register<FilteredMessage>(this, msg => wasFiltered = true, msg => true);

            _messenger.Send(new FilteredMessage { Id = 42 });

            Assert.That(wasFiltered);
        }

        [Test]
        public void Send_WithMessageFilterAndDerivedMessages()
        {
            var wasFiltered = false;
            _messenger.Register<FilteredMessage>(this, true, msg => wasFiltered = true, msg => true);

            _messenger.Send(new FilteredDerivedMessage { Id = 42 });

            Assert.That(wasFiltered);
        }

        [Test]
        public void Send_WithMessageFilterAndToken()
        {
            var wasFiltered = false;
            var token = new object();
            _messenger.Register<FilteredMessage>(this, token, msg => wasFiltered = true, msg => true);

            _messenger.Send(new FilteredMessage { Id = 42 }, token);

            Assert.That(wasFiltered);
        }

        [Test]
        public void Send_WithMessageFilterAndNotMatchingToken_MessageIsNotDelivered()
        {
            var wasDelivered = false;
            var token = new object();
            _messenger.Register<FilteredMessage>(this, token, msg => wasDelivered = true, msg => true);

            _messenger.Send(new FilteredMessage { Id = 42 }, new object());

            Assert.That(wasDelivered, Is.False);
        }

        [Test]
        public void Send_WithMultipleMessageFilters_MatchingMessagesAreDelivered()
        {
            var ids = new List<int>();
            _messenger.Register<FilteredMessage>(this, msg => ids.Add(msg.Id), msg => msg.Id == 42);
            _messenger.Register<FilteredMessage>(this, msg => ids.Add(msg.Id), msg => msg.Id == 37);

            _messenger.Send(new FilteredMessage { Id = 37 });
            _messenger.Send(new FilteredMessage { Id = 42 });
            _messenger.Send(new FilteredMessage { Id = 546 });

            Assert.That(ids.Contains(42));
            Assert.That(ids.Contains(37));
            Assert.That(!ids.Contains(546));
        }


        public class FilteredMessage
        {
            public int Id { get; set; }
        }

        public class FilteredDerivedMessage : FilteredMessage
        {
        }

        public class SomeMessage : MessageBase
        {
            public SomeMessage(object sender) : base(sender)
            {
            }
        }

    }

}

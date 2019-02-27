using System;
using System.ComponentModel;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Wpf.Test
{

    [TestFixture]
    public class PropertyChangedAggregateTest
    {

        [Test]
        public void Ctor_WithoutArguments_Throws()
        {
            Assert.Throws<ArgumentException>(() => new PropertyChangedAggregate());
        }

        [Test]
        public void Ctor_AggregatesEventsFromArguments()
        {
            var event1WasRaised = false;
            var event2WasRaised = false;
            var n1 = new TestNotifier();
            var n2 = new TestNotifier();
            var pca = new PropertyChangedAggregate(n1, n2);
            pca.PropertyChanged += (s, a) => { if (a.PropertyName == "testProp1") event1WasRaised = true; };
            pca.PropertyChanged += (s, a) => { if (a.PropertyName == "testProp2") event2WasRaised = true; };

            n1.RaisePropertyChanged("testProp1");
            n2.RaisePropertyChanged("testProp2");

            Assert.That(event1WasRaised);
            Assert.That(event2WasRaised);
        }

        [Test]
        public void AggregatedEvent_PreservesOriginalSender()
        {
            var event1WasRaisedAsSender = false;
            var event2WasRaisedAsSender = false;
            var n1 = new TestNotifier();
            var n2 = new TestNotifier();
            var pca = new PropertyChangedAggregate(n1, n2);
            pca.PropertyChanged += (s, a) => { if (a.PropertyName == "testProp1") event1WasRaisedAsSender = s.Equals(n1); };
            pca.PropertyChanged += (s, a) => { if (a.PropertyName == "testProp2") event2WasRaisedAsSender = s.Equals(n2); };

            n1.RaisePropertyChanged("testProp1");
            n2.RaisePropertyChanged("testProp2");

            Assert.That(event1WasRaisedAsSender);
            Assert.That(event2WasRaisedAsSender);
        }


        private class TestNotifier : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(string propertyName)
            {
                var e = PropertyChanged;
                if (e != null)
                {
                    e(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }

}

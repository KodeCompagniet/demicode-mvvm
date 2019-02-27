using System;
using Autofac;
using Autofac.Builder;
using DemiCode.Mvvm.Messaging;
using DemiCode.Mvvm.Wpf;
using NUnit.Framework;

namespace DemiCode.Mvvm.Test
{
    [TestFixture]
    public class AssemblyModuleTests
    {
        private IContainer _container;

        // ReSharper disable InconsistentNaming

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AssemblyModule>();
            _container = builder.Build();
        }

        [Test]
        public void Resolve_ViewController()
        {
            var vc = _container.Resolve<IViewController>();

            Assert.That(vc, Is.Not.Null);
        }

        [Test]
        public void Resolve_ViewController_IsSingleton()
        {
            var vc1 = _container.Resolve<IViewController>();
            var vc2 = _container.Resolve<IViewController>();

            Assert.That(vc1, Is.SameAs(vc2));
        }

        [Test]
        public void Resolve_MessageBoxService()
        {
            var mbs = _container.Resolve<IMessageBoxService>();

            Assert.That(mbs, Is.Not.Null);
        }

        [Test]
        public void Resolve_MessageBoxService_IsSingleton()
        {
            var mbs1 = _container.Resolve<IMessageBoxService>();
            var mbs2 = _container.Resolve<IMessageBoxService>();

            Assert.That(mbs1, Is.SameAs(mbs2));
        }

        [Test]
        public void Messenger_IsRegistered()
        {
            Assert.That(_container.IsRegistered<IMessenger>());
        }

        [Test]
        public void DispatcherService_IsRegistered()
        {
            Assert.That(_container.IsRegistered<IDispatcherService>());
        }

        // ReSharper restore InconsistentNaming
    }
}

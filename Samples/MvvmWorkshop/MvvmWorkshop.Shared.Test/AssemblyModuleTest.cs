using System;
using Autofac;
using NUnit.Framework;

namespace MvvmWorkshop.Shared.Test
{
    [TestFixture]
    public class AssemblyModuleTest
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AssemblyModule>();
            _container = builder.Build();
        }

        [Test]
        public void IsRegistered_IDataService()
        {
            Assert.That(_container.IsRegistered<IDataService>());
        }

        [Test]
        public void IsRegistered_IConnection()
        {
            Assert.That(_container.IsRegistered<IConnection>());
        }

        [Test]
        public void Resolve_IDataService()
        {
            var dataService = _container.Resolve<IDataService>();
            Assert.That(dataService, Is.Not.Null);
        }
    }
}

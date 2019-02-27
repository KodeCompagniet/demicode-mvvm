using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using DemiCode.Hours.Shared.Services;
using DemiCode.Hours.Sql.Services;
using DemiCode.Hours.Win.Employees;
using NUnit.Framework;


namespace DemiCode.Hours.Win.Test
{
    [TestFixture]
    public class AppModuleTest
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AppModule());
            _container = builder.Build();
        }

        [Test]
        public void IsRegistered_SqlConnectionFactory()
        {
            Assert.That(_container.IsRegistered<SqlConnectionFactory>());
        }

        [Test]
        public void Resolve_HoursDataService()
        {
            var service = _container.Resolve<IHoursDataService>();
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void IsRegistered_ViewModels()
        {
            Assert.That(_container.IsRegistered<MainViewModel>());
            Assert.That(_container.IsRegistered<EmployeeListViewModel>());
        }
    }
}

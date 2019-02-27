using System.Threading;
using Autofac;
using DemiCode.Mvvm.Test.Autofac.Views;
using NUnit.Framework;

namespace DemiCode.Mvvm.Autofac.Test
{
    [TestFixture]
    public class ComponentContextExtensionsTest
    {
        private IComponentContext _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            builder.RegisterType<TestWindowView>();
            _container = builder.Build();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void ResolveWindowView()
        {
            var view = _container.ResolveWindowView<TestWindowView>();
            Assert.That(view, Is.Not.Null);
        }
    }
}

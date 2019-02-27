using System;
using Autofac;
using DemiCode.Mvvm.Autofac;
using DemiCode.Mvvm.Test.Autofac.Views;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Test
{

    [TestFixture]
    public class ViewControllerTest
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            builder.RegisterType<TestWindowView>();
            _container = builder.Build();
        }

        [Test]
        public void Construct_IocContext_IsContainer()
        {
            var vc = new ViewController(_container, () => null);

            Assert.That(vc.IocContext, Is.SameAs(_container));
        } 

    }

}

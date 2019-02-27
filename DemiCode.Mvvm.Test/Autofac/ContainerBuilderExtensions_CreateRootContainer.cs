using System;
using Autofac;
using NUnit.Framework;

namespace DemiCode.Mvvm.Autofac.Test
{
    [TestFixture]
    public class ContainerBuilderExtensions_CreateRootContainer
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = ContainerBuilderExtensions.CreateRootContainer(GetType().Assembly);
        }

        [TearDown]
        public void TearDown()
        {
            if (_container != null)
                _container.Dispose();
        }


        // ReSharper disable InconsistentNaming

        [Test]
        public void RootContainer_Have_MvvmModule()
        {
            Assert.That(_container.IsRegistered<IViewController>());
        }

        [Test]
        public void RootContainer_Have_MainView_RegisteredWithKey()
        {
            Assert.That(_container.IsRegisteredWithKey<WindowView>("MainView"));
        }

        [Test]
        public void RootContainer_Have_MainView_RegisteredWithSelf()
        {
            Assert.That(_container.IsRegistered<MainView>());
        }

        [Test]
        public void RootContainer_Have_AnyViewsRegistered()
        {
            Assert.That(_container.IsRegisteredWithKey<WindowView>("SomeOtherView"));
            Assert.That(_container.IsRegistered<SomeOtherView>());
        }

        [Test]
        public void RootContainer_Have_AppModule()
        {
            Assert.That(_container.IsRegistered<SomeService>());
        }


        // ReSharper restore InconsistentNaming
    }

    public class SomeService {}

    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SomeService>();
        }
    }

    public class MainView : WindowView
    {

    }

    public class SomeOtherView : WindowView
    {

    }

}

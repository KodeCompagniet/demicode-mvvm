using Autofac;
using DemiCode.Mvvm.Messaging;
using DemiCode.Mvvm.UserControlViewTests.Test;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Autofac.Test
{
    [TestFixture]
    public class ContainerBuilderExtensionsTest
    {
        internal class TestViewModel : ViewModelBase{}

        [Test]
        public void RegisterMvvmModule_WithViewModelAssemblies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule(typeof(TestViewModel).Assembly);
            var container = builder.Build();

            Assert.That(container.IsRegistered<TestViewModel>());
        }

        [Test]
        public void RegisterMvvmModule_RegistersMessengerInstance()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmModule();
            var container = builder.Build();

            Assert.That(container.IsRegistered<IMessenger>());
        }

        [Test]
        public void RegisterViewModels_RegistersViewModelsByConvention()
        {
            var builder = new ContainerBuilder();
            builder.RegisterViewModels(typeof(TestViewModel).Assembly);
            var container = builder.Build();

            Assert.That(container.IsRegistered<TestViewModel>());
        }

        [Test]
        public void RegisterViewModels_RegistersGenericViewModels()
        {
            var type = typeof (TestTypedViewModel<string>);
            var builder = new ContainerBuilder();
            builder.RegisterViewModels(type.Assembly);
            var container = builder.Build();

            Assert.That(container.IsRegistered(type));
        }


    }
}
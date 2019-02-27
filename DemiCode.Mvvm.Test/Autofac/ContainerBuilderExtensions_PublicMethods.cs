using Autofac;
using DemiCode.Mvvm.Autofac;
using DemiCode.Mvvm.Test.Autofac.Views;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Test
{

    [TestFixture]
    public class ContainerBuilderExtensions_PublicMethods
    {

        [Test]
        public void RegisterMvvmAssembly_()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMvvmAssemblies(typeof(TestWindowViewModel).Assembly);
            var container = builder.Build();

            Assert.That(container.IsRegistered<TestWindowView>());
            Assert.That(container.IsRegistered<TestWindowViewModel>());
        }


    }

}

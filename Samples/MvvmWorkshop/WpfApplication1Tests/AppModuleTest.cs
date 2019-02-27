using Autofac;
using NUnit.Framework;
using WpfApplication1;

namespace WpfApplication1Tests
{
	[TestFixture]
	public class AppModuleTest
	{
		private IContainer _container;

		[SetUp]
		public void SetUp()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<AppModule>();
			_container = builder.Build();
		}

		[Test]
		public void IsRegistered_MainView()
		{
			Assert.That(_container.IsRegistered<MainView>());
		}

	    [Test]
	    public void Resolve_MainViewModel()
	    {
	        var vm = _container.Resolve<MainViewModel>();
            Assert.That(vm, Is.Not.Null);
	    }

	}
}
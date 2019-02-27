using Autofac;

namespace WpfApplication1
{
	public class AppModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
            // Register shared module
            builder.RegisterModule<MvvmWorkshop.Shared.AssemblyModule>();
        }
	}
}
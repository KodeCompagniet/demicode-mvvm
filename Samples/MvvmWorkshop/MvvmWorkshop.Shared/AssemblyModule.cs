using Autofac;

namespace MvvmWorkshop.Shared
{
    public class AssemblyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<DataService>()
                .As<IDataService>()
                .SingleInstance();

            builder
                .RegisterType<Connection>()
                .As<IConnection>();
        }
    }
}
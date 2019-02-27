using Autofac;

namespace WpfProjectTemplate
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register services
            RegisterServices(builder);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            // TODO: Register your application's services here

            //builder.Register<MyService>(c => new MyService(c.Resolve<IMyProvider>())).As<IMyService>();
            //builder.RegisterType<MyService>().As<IMyService>();
        }
    }
}

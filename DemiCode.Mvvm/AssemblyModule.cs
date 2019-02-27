using Autofac;
using DemiCode.Mvvm.Messaging;
using DemiCode.Mvvm.Wpf;

namespace DemiCode.Mvvm
{
    ///<summary>
    /// Autofac registrations for this assembly.
    ///</summary>
    public class AssemblyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewController>().As<IViewController>().SingleInstance();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>().SingleInstance();
            builder.RegisterType<Messenger>().As<IMessenger>().SingleInstance();
            builder.RegisterType<ApplicationDispatcherService>().As<IDispatcherService>().SingleInstance();
        }
    }
}

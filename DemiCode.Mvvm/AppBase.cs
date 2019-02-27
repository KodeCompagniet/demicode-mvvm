using System;
using System.Windows;
using System.Windows.Input;
using Autofac;
using DemiCode.Mvvm.Autofac;
using DemiCode.Mvvm.Messaging;

namespace DemiCode.Mvvm
{
    ///<summary>
    /// Base class for App.xaml.
    ///</summary>
    public abstract class AppBase : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            var assembly = GetType().Assembly;

            _container = ContainerBuilderExtensions.CreateRootContainer(assembly);
            _container.InjectUnsetProperties(this);

            var startupView = _container.ResolveMainView();
            if (startupView == null)
                throw new ApplicationException("A MainView window could not be found");

            RegisterMessages();

            startupView.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_container != null)
                _container.Dispose();
        }

        private void RegisterMessages()
        {
            var msg = _container.Resolve<IMessenger>();
            msg.Register<ReevaluateCommandsMessage>(this, ReevaluateCommands);
        }

        private static void ReevaluateCommands(ReevaluateCommandsMessage obj)
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
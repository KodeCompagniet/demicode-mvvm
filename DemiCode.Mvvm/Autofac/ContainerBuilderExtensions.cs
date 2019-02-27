using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Autofac.Core;

namespace DemiCode.Mvvm.Autofac
{
    ///<summary>
    /// Container building methods.
    ///</summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Create a root container for the given assembly, automatically registering an AppModule (if it exists)
        /// and registering a MainView (if it exists);
        /// </summary>
        /// <param name="fromAssembly"></param>
        /// <returns></returns>
        internal static IContainer CreateRootContainer(Assembly fromAssembly)
        {
            if (fromAssembly == null)
                throw new ArgumentNullException("fromAssembly");

            var builder = new ContainerBuilder();

            // Register our mvvm stuff
            builder.RegisterMvvmModule(fromAssembly);

            // Optionally register an AppModule if we found one));
            var appModule = GetAppModule(fromAssembly);
            if (appModule != null)
                builder.RegisterModule(appModule);

            return builder.Build();
        }

        private static IModule GetAppModule(Assembly fromAssembly)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(fromAssembly)
                .Where(type => type.Name.EndsWith("AppModule"))
                .AsImplementedInterfaces();
            using (var moduleContainer = builder.Build())
            {
                return moduleContainer.ResolveOptional<IModule>();
            }
        }


        ///<summary>
        /// Registers the MVVM AssemblyModule and classes inheriting <see cref="ViewModelBase"/> in the
        /// specified assemblies.
        ///</summary>
        /// <param name="builder">The Autofac container builder.</param>
        /// <param name="viewModelAssemblies">Optional list of assemblies from which to register views and viewmodels.</param>
        internal static void RegisterMvvmModule(this ContainerBuilder builder, params Assembly[] viewModelAssemblies)
        {
            builder.RegisterModule<AssemblyModule>();
            builder.RegisterMvvmAssemblies(viewModelAssemblies);
        }

        ///<summary>
        /// Register <see cref="WindowView"/>s (and in time <see cref="PageView"/>s) from the specified assemblies.
        ///</summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to use.</param>
        /// <param name="assemblies">The <see cref="Assembly">assemblies</see> in which to search for view types.</param>
        internal static void RegisterViews(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                // Register WindowViews
                builder
                    .RegisterAssemblyTypes(assembly)
                    .AssignableTo<WindowView>()
                    .OnActivated(ConfigureIocContext)
                    .AsSelf()
                    .Keyed(type => type.Name, typeof(WindowView))
                    .InstancePerDependency();

                // TODO: Register PageViews?
            }
        }

        /// <summary>
        /// Register assemblies containing views and viewmodels.
        /// </summary>
        public static void RegisterMvvmAssemblies(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterViews(assemblies);
            builder.RegisterViewModels(assemblies);
        }

        private static void ConfigureIocContext(IActivatedEventArgs<object> e)
        {
            IocContextHelper.SetIocContext((UIElement) e.Instance, e.Context.Resolve<IComponentContext>());
        }

        ///<summary>
        /// Register ViewModels (classes inheriting <see cref="ViewModelBase"/>) from the specified assemblies.
        ///</summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to use.</param>
        /// <param name="assemblies">The <see cref="Assembly">assemblies</see> in which to search for viewmodel types.</param>
        internal static void RegisterViewModels(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                builder
                    .RegisterAssemblyTypes(assembly)
                    .AssignableTo<ViewModelBase>()
                    .InstancePerDependency();
                foreach (
                    var type in assembly.GetTypes().Where(t => typeof (ViewModelBase).IsAssignableFrom(t) &&
                                                                        t.GetGenericArguments().Length > 0))
                {
                    builder
                        .RegisterGeneric(type)
                        .InstancePerDependency();
                }
            }
        }
    }
}
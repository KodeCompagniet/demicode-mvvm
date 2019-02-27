using System;
using System.Windows;
using Autofac;

namespace DemiCode.Mvvm.Autofac
{
    ///<summary>
    /// MVVM extension methods for <see cref="IComponentContext"/>.
    ///</summary>
    public static class ComponentContextExtensions
    {
        /// <summary>
        /// Try to resolve a "MainView" or return null if it doesn't exist.
        /// </summary>
        internal static WindowView ResolveMainView(this IComponentContext context)
        {
            return context.ResolveOptionalKeyed<WindowView>("MainView");
        }

        ///<summary>
        /// Resolve a view and set its <see cref="IComponentContext"/>.
        ///</summary>
        ///<param name="context">The Autofac context.</param>
        ///<typeparam name="TView">The view type to resolve.</typeparam>
        ///<returns>An instance of the view.</returns>
        public static TView ResolveWindowView<TView>(this IComponentContext context) where TView : UIElement, IView
        {
            // Resolve the view without assuming an IComponentContext constructor parameter to allow for views with no codebehind
            // (and hence only a default constructor)
            var view = context.Resolve<TView>();

            // Set the context
            IocContextHelper.SetIocContext(view, context);

            return view;
        }
    }
}
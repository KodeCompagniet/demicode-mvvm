using System.Windows;
using Autofac;
using DemiCode.Mvvm.Wpf;

namespace DemiCode.Mvvm
{
    ///<summary>
    /// Helper class that enables retrieval of the container currently attached to a view.
    ///</summary>
    public sealed class IocContextHelper 
    {
        ///<summary>
        /// The dependency property holding an IOC context.
        ///</summary>
        public static readonly DependencyProperty IocContextProperty = DependencyHelper<IocContextHelper>.RegisterAttachedInherited("IocContext", typeof(IComponentContext));

        /// <summary>
        /// Static getter for the IocContext attached inherited property.
        /// </summary>
        /// <param name="element">The element to get the attach property for.</param>
        /// <returns>The <see cref="IComponentContext"/> for the element.</returns>
        public static IComponentContext GetIocContext(UIElement element)
        {
            return element.GetValue(IocContextProperty) as IComponentContext;
        }

        /// <summary>
        /// Static getter for the IocContext attached inherited property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object to get the attach property for.</param>
        /// <returns>The <see cref="IComponentContext"/> for the dependencyObject.</returns>
        public static IComponentContext GetIocContext(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(IocContextProperty) as IComponentContext;
        }

        /// <summary>
        /// Static setter for the IocContext attached inherited property.
        /// </summary>
        /// <param name="element">The element to get the attach property for.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIocContext(UIElement element, IComponentContext value)
        {
            element.SetValue(IocContextProperty, value);
        }

    }
}

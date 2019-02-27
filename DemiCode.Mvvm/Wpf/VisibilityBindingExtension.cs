using System;
using System.Windows;
using System.Windows.Markup;

namespace DemiCode.Mvvm.Wpf
{
    /// <summary>
    /// A binding extension that converts boolean values to <see cref="Visibility"/> values.
    /// True yields <see cref="Visibility.Visible"/>, while false yields <see cref="Visibility.Collapsed"/>.
    /// Also if the value source is not a boolean, yields <see cref="Visibility.Visible"/>.
    /// 
    /// TODO: must fix implementation
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    internal class VisibilityBindingExtension : BindingDecoratorBase
    {
        public override object ProvideValue(IServiceProvider provider)
        {
            //delegate binding creation etc. to the base class
            object val = base.ProvideValue(provider);
            if (val is bool)
            {
                var v = (bool) val;
                return v ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Visible;
        }
    }
}
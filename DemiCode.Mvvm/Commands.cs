using System;
using System.Windows;
using DemiCode.Mvvm.Wpf;
using System.Windows.Input;
using System.Windows.Controls;

namespace DemiCode.Mvvm
{
    public class Commands
    {

        /// <summary>
        /// A command that opens a given view.
        /// </summary>
        public static readonly RoutedUICommand OpenView = new RoutedUICommand("Open a view", "OpenView", typeof(Commands));

        /// <summary>
        /// Command for opening a view as a modal dialog window.
        /// </summary>
        public static readonly RoutedUICommand OpenModalView = new RoutedUICommand("Open a view", "OpenView", typeof(Commands));


        ///<summary>
        /// The "ViewType" attached property.
        ///</summary>
        public static readonly DependencyProperty ViewTypeProperty = DependencyHelper<Commands>.RegisterAttached("ViewType", typeof(Type));

        ///<summary>
        /// The "TargetContainer" attached property.
        ///</summary>
        public static readonly DependencyProperty TargetContainerProperty = DependencyHelper<Commands>.RegisterAttachedInherited("TargetContainer", typeof(ContentControl));

        #region Attached Property getters/setters

        public static Type GetViewType(UIElement element)
        {
            return element.GetValue(ViewTypeProperty) as Type;
        }

        public static void SetViewType(UIElement element, Type value)
        {
            element.SetValue(ViewTypeProperty, value);
        }

        public static ContentControl GetTargetContainer(UIElement element)
        {
            return element.GetValue(TargetContainerProperty) as ContentControl;
        }

        public static void SetTargetContainer(UIElement element, ContentControl value)
        {
            element.SetValue(TargetContainerProperty, value);
        }

        #endregion

    }
}

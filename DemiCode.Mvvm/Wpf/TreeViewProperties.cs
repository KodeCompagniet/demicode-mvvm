using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DemiCode.Mvvm.Wpf
{
    ///<summary>
    /// Supports attached properties for helping with binding <see cref="TreeView.SelectedItem"/> to viewmodel properties.
    ///</summary>
    public class TreeViewProperties
    {
        private static readonly Dictionary<DependencyObject, TreeViewSelectedItemBehavior> Behaviors = new Dictionary<DependencyObject, TreeViewSelectedItemBehavior>();

        /// <summary>
        /// SelectedItems Attached Dependency Property. Attatching this property to a multiselection mode control
        /// enables binding the current list of selected items into another dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyHelper<TreeViewProperties>.RegisterAttachedWithUIPropertyChangedCallback<object>("SelectedItem", OnSelectedItemChanged);

        /// <summary>
        /// Gets the SelectedItem property.
        /// </summary>
        public static object GetSelectedItem(DependencyObject d)
        {
            return d.GetValue(SelectedItemProperty);
        }

        /// <summary>
        /// Sets the SelectedItem property.
        /// </summary>
        public static void SetSelectedItem(DependencyObject d, object value)
        {
            d.SetValue(SelectedItemProperty, value);
        }

        private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var treeView = obj as TreeView;
            if (treeView == null)
                return;

            if (!Behaviors.ContainsKey(treeView))
                Behaviors.Add(obj, new TreeViewSelectedItemBehavior(treeView));

            var view = Behaviors[obj];
            view.ChangeSelectedItem(e.NewValue);
        }


        private class TreeViewSelectedItemBehavior
        {
            private readonly TreeView _view;
            public TreeViewSelectedItemBehavior(TreeView view)
            {
                _view = view;
                view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
            }

            internal void ChangeSelectedItem(object p)
            {
                var item = (TreeViewItem)_view.ItemContainerGenerator.ContainerFromItem(p);
                item.IsSelected = true;
            }
        }
    }
}
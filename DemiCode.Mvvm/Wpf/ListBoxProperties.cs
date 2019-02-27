using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace DemiCode.Mvvm.Wpf
{
    ///<summary>
    /// Supports attached properties for helping with binding <see cref="System.Windows.Controls.ListBox.SelectedItems"/> to viewmodel properties.
    ///</summary>
    /// <remarks>http://marlongrech.wordpress.com/2009/06/02/sync-multi-select-listbox-with-viewmodel/</remarks>
    public class ListBoxProperties
    {
        /// <summary>
        /// SelectedItems Attached Dependency Property. Attatching this property to a multiselection mode control
        /// enables binding the current list of selected items into another dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyHelper<ListBoxProperties>.RegisterAttachedWithPropertyChangedCallback<IList>("SelectedItems", OnSelectedItemsChanged);

        /// <summary>
        /// Gets the SelectedItems property.
        /// </summary>
        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        /// <summary>
        /// Sets the SelectedItems property.
        /// </summary>
        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Handles changes to the SelectedItems property.
        /// </summary>
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (ListBox)d;
            ResetSelectedItems(listBox);
            listBox.SelectionChanged += (sender, args) => ResetSelectedItems(listBox);
        }

        private static void ResetSelectedItems(ListBox listBox)
        {
            var selectedItems = GetSelectedItems(listBox);
            selectedItems.Clear();
            
            if (listBox.SelectedItems == null) return;

            foreach (var item in listBox.SelectedItems)
            {
                selectedItems.Add(item);
            }
        }

    }
}
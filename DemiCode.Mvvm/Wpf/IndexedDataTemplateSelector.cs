using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DemiCode.Mvvm.Wpf
{
    ///<summary>
    /// A data template selector that will, based on a numeric index, return the corresponding template in <see cref="Templates"/>.
    ///</summary>
    public class IndexedDataTemplateSelector : DataTemplateSelector
    {
        ///<summary>
        /// The template collection.
        ///</summary>
        public ObservableCollection<DataTemplate> Templates { get; set; }

        ///<summary>
        /// The number of templates in the <see cref="Templates"/> collection.
        ///</summary>
        public int TemplateCount { get { return Templates.Count; } }

        ///<summary>
        /// Construct a new instance of <see cref="IndexedDataTemplateSelector"/>.
        ///</summary>
        public IndexedDataTemplateSelector()
        {
            Templates = new ObservableCollection<DataTemplate>();
        }

        /// <summary>
        /// Based on the value of <paramref name="item"/>, return the corresponding template from <see cref="Templates"/>.
        /// </summary>
        /// <returns>Retruns a data template, or null of <paramref name="item"/> is null, or does not contain an integer</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (Equals(item, null) || Templates.Count == 0)
                return null;

            var step = Convert.ToInt32(item);
            if (step >= Templates.Count)
                return null;

            return Templates[step];
        }
    }
}
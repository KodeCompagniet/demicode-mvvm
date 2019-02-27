using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Controls;
using DemiCode.Mvvm.Helpers;

namespace DemiCode.Mvvm.Wpf
{
    ///<summary>
    /// A base class for usercontrols that also needs to implement <see cref="INotifyPropertyChanged"/>.
    ///</summary>
    public class UserControlWithNotifyPropertyChangedBase : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event in a typesafe way. Call as
        /// <c>RaisePropertyChanged(() => MyProperty)</c>.
        /// </summary>
        /// <param name="property">An expression which specifies the property that has changed.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            RaisePropertyChanged(NotifyPropertyChangedHelper.GetPropertyNameFromExpression(property));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property which has changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event. Call as
        /// <c>RaisePropertyChanged(() => MyProperty)</c>.
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }
}